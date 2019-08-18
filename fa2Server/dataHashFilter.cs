using log4net;
using MakC.Common;
using MakC.Data;
using MakC.Data.Model;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace fa2Server
{
    public class dataHashFilter
    {
        private RequestDelegate _next;
        private ILog log = LogManager.GetLogger(Startup.Repository.Name, "dataHashFilter");
        public dataHashFilter(RequestDelegate next)
        {
            _next = next;
        }

        public async Task NormalRequest(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exex)
            {
                log.Error(exex.Message);
                log.Error(exex.StackTrace);
            }
        }

        public async Task ApiRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            var request = context.Request.Body;
            var response = context.Response.Body;
            string ResponseBody = "";
            string ErrorMessage = "";
            JObject JsonReqdata = new JObject();
            string uuid = "";
            int sg_version = 0;
            try
            {
                if (context.Request.Method == "POST")
                {
                    context.Request.Path = context.Request.Path.ToString().Replace("//", "/");
                    using (var newRequest = new MemoryStream())
                    {
                        string RequestBody = "";
                        using (var reader = new StreamReader(request))
                        {
                            RequestBody = await reader.ReadToEndAsync();
                        }
                        JsonReqdata = (JObject)JsonConvert.DeserializeObject(RequestBody);
                        if (JsonReqdata["uuid"] != null)
                        {
                            uuid = JsonReqdata["uuid"].ToString();
                        }
                        sg_version = JsonReqdata["sg_version"].AsInt();
                        if (!checkReqSign(context, RequestBody, uuid, sg_version))
                        {
                            ErrorMessage = "参数错误.";
                            return;
                        }
                        ErrorMessage = checkUserInfo(context, JsonReqdata);
                        if (!string.IsNullOrEmpty(ErrorMessage))
                        {
                            return;
                        }
                        using (var newResponse = new MemoryStream())
                        {
                            context.Response.Body = newResponse;
                            using (var writer = new StreamWriter(newRequest))
                            {
                                await writer.WriteAsync(RequestBody);
                                await writer.FlushAsync();
                                newRequest.Position = 0;
                                context.Request.Body = newRequest;
                                await _next(context);
                            }
                            using (var reader = new StreamReader(newResponse))
                            {
                                newResponse.Position = 0;
                                ResponseBody = await reader.ReadToEndAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception exex)
            {                
                log.Error(exex.Message);
                log.Error(exex.StackTrace);
                ErrorMessage = "系统错误";
            }
            finally
            {

                MemoryCacheService.Default.RemoveCache("account_" + context.TraceIdentifier);
                if (string.IsNullOrEmpty(ResponseBody))
                {
                    JObject ResObj = new JObject();
                    ResObj["code"] = 1;
                    ResObj["type"] = 1;
                    ResObj["message"] = ErrorMessage;
                    ResponseBody = ResObj.ToString(Newtonsoft.Json.Formatting.None);
                }
                string ServerTime = ((DateTime.Now.AddHours(8).ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
                context.Response.Headers.Add("Server-Time", ServerTime);
                if (context.Request.Headers["User-Agent"].ToString().IndexOf("Darwin") == -1)
                {
                    //context.Response.Headers.Add("Sign", SignData_Andorid(ServerTime, ResponseBody));
                    if (sg_version >= 138)
                    {
                        context.Response.Headers.Add("Sign2", SignData_Andorid(ServerTime, ResponseBody, uuid));
                    }
                    context.Response.Headers.Add("Sign", SignData_Andorid(ServerTime, ResponseBody, ""));
                }
                else
                {
                    if (sg_version >= 455)
                    {
                        context.Response.Headers.Add("Sign2", SignData_Ios428Plus(ServerTime, ResponseBody, uuid));
                    }
                    context.Response.Headers.Add("Sign", SignData_Ios428Plus(ServerTime, ResponseBody, ""));
                }

                using (var writer = new StreamWriter(response))
                {
                    await writer.WriteAsync(ResponseBody);
                    await writer.FlushAsync();
                }
            }
        }
        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments("/api"))
            {
                await NormalRequest(context);
            }
            else
            {
                await ApiRequest(context);
            }
        }
        private static bool checkReqSign(HttpContext context, string ReqData, string uuid, int sg_version)
        {
            string Req_sign;
            string Req_ServerTime = context.Request.Headers["Server-Time"].ToString();
            if (string.IsNullOrEmpty(Req_ServerTime))
            {
                return false;
            }
            string TmpSign;
            if (context.Request.Headers["User-Agent"].ToString().IndexOf("Darwin") == -1)
            {
                //Android
                if (sg_version>=138)
                {
                    TmpSign = SignData_Andorid(Req_ServerTime, ReqData, uuid);
                    Req_sign = context.Request.Headers["sign2"].ToString();
                }
                else
                {
                    TmpSign = SignData_Andorid(Req_ServerTime, ReqData ,"");
                    Req_sign = context.Request.Headers["sign"].ToString();
                }
            }
            else
            {
                //ios
                if (sg_version>=455)
                {
                    TmpSign = SignData_Ios428Plus(Req_ServerTime, ReqData, uuid);
                    Req_sign = context.Request.Headers["sign2"].ToString();
                }
                else
                {
                    TmpSign = SignData_Ios428Plus(Req_ServerTime, ReqData, "");
                    Req_sign = context.Request.Headers["sign"].ToString();
                }
            }
            return Req_sign == TmpSign;
        }
        private string checkUserInfo(HttpContext context, JObject data)
        {                        
            F2.user account;
            var dbh = DbContext.Get();
            if (context.Request.Path.Value.EndsWith("register"))
            {
                return "";
            }
            //else if (context.Request.Path.Value.EndsWith("first_login"))
            //{
            //    account = dbh.GetEntityDB<F2.user>().GetSingle(ii => ii.username == data["user_name"].ToString());
            //    if (account == null)
            //    {
            //        return "用户不存在！";
            //    }
            //    if (account.password != data["password"].ToString())
            //    {
            //        return "密码错误";
            //    }
            //}
            else if (context.Request.Path.Value.EndsWith("login"))
            {
                account = dbh.GetEntityDB<F2.user>().GetSingle(ii => ii.username == data["user_name"].ToString());
                if (account == null)
                {
                    return "用户不存在！";
                }
                if (account.password != data["password"].ToString())
                {
                    return "密码错误";
                }
                if (account.isBan)
                {
                    return "你的存档数据已损坏！";
                }
            }
            else
            {
                string uuid = data["uuid"]?.ToString();                
                account = dbh.GetEntityDB<F2.user>().GetSingle(ii => ii.uuid == uuid);
                if (account == null)
                {
                    return "用户不存在！";
                }
                if (account.token != data["token"]?.ToString())
                {
                    return "账号已在其它地方登录";
                }
                if (account.isBan)
                {
                    return "你的存档数据已损坏！";
                }
                if (data["net_id"] != null)
                {
                    int net_id = data["net_id"].ToString().AsInt();
                    if (account.net_id >= net_id)
                    {
                        return "无效的网络请求!";
                    }
                    account.net_id = net_id;
                    dbh.Db.Updateable(account).UpdateColumns(ii => ii.net_id).ExecuteCommand();
                }
            }
            MemoryCacheService.Default.SetCache("account_" + context.TraceIdentifier, account, 1);
            return "";
        }


        public static string MD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "").ToLower();
            }
        }
        private static string SignData_Andorid(string dct, string data, string uuid)
        {
            string k1 = MD5Hash(dct + "^" + dct + "!" + dct);
            string k2 = MD5Hash("zRBcyL2fy[ZsL7XJP$AIDJE*2DFF=#Dxjef2@LDLF");
            k2 = MD5Hash(dct + "#" + k2);
            return MD5Hash(k1 + data.Trim() + k2 + "TeeKB0a1Dmdg8oTovsZOu3E0o2gNhrL2" + uuid);
        }
        private static string SignData_Ios428Plus(string dct, string data, string uuid)
        {
            string k1 = MD5Hash(dct + "^" + dct + "!" + dct);
            string k2 = MD5Hash("zRBcyL2fy[ZsL7XJP$AIDJE*2DFF=#Dxjef2@LDLF");
            k2 = MD5Hash(dct + "#" + k2);
            return MD5Hash(k1 + data.Trim() + k2 + "U8VrXwFkELpEhiMSByrdftZQbnCUP8Vw" + uuid);
        }
    }
}
