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
using System.Text;
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
                //if (data["token"] != null && data["token"].ToString() != account.token)
                //{
                //    return "账号已在其它地方登录";
                //}
            }else if (context.Request.Path.Value.EndsWith("system_user_info"))
            {
                string uuid = data["uuid"]?.ToString();
                account = dbh.GetEntityDB<F2.user>().GetSingle(ii => ii.uuid == uuid);
                if (account == null)
                {
                    return "用户不存在！";
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
        public static string AESDecrypt(string Data, string token, int ServerTime)
        {
            string key;
            string iv;
            ios460_getAESKey(token, ServerTime, out key, out iv);

            //使用AES（CBC）解密
            Byte[] original = null;
            Rijndael Aes = Rijndael.Create();
            try
            {
                using (MemoryStream Memory = new MemoryStream(Convert.FromBase64String(Data)))
                {
                    using (CryptoStream Decryptor = new CryptoStream(Memory,
                    Aes.CreateDecryptor(Encoding.ASCII.GetBytes(key), Encoding.ASCII.GetBytes(iv)),
                    CryptoStreamMode.Read))
                    {
                        using (MemoryStream originalMemory = new MemoryStream())
                        {
                            Byte[] Buffer = new Byte[1024];
                            Int32 readBytes = 0;
                            while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                            {
                                originalMemory.Write(Buffer, 0, readBytes);
                            }
                            original = originalMemory.ToArray();
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
            return Encoding.UTF8.GetString(original);
        }

        public static string AESEncrypt(string Data, string token, int ServerTime)
        {
            string key;
            string iv;
            ios460_getAESKey(token, ServerTime, out key, out iv);

            //使用AES（CBC）加密
            Byte[] Cryptograph = null;
            Rijndael Aes = Rijndael.Create();
            try
            {
                using (MemoryStream Memory = new MemoryStream())
                {
                    using (CryptoStream Encryptor = new CryptoStream(Memory,
                    Aes.CreateEncryptor(Encoding.ASCII.GetBytes(key), Encoding.ASCII.GetBytes(iv)),
                    CryptoStreamMode.Write))
                    {
                        Byte[] plainBytes = Encoding.UTF8.GetBytes(Data);
                        Encryptor.Write(plainBytes, 0, plainBytes.Length);
                        Encryptor.FlushFinalBlock();
                        Cryptograph = Memory.ToArray();
                    }
                }
            }
            catch
            {
                return null;
            }
            return Convert.ToBase64String(Cryptograph);
        }
        public static void ios460_getAESKey(string token, int ServerTime, out string key, out string iv)
        {
            string key1 = MD5Hash(token + (ServerTime / 29).ToString());
            string key2 = MD5Hash(((ServerTime % 29) + ServerTime).ToString());
            string key3 = MD5Hash((ServerTime % 29).ToString() + MD5Hash(token).Substring(4, 16));
            string key4 = MD5Hash(key1 + key2 + key3);

            string v13 = key1 + key1;
            string v14 = key2 + key2;
            string v15 = key3 + key3;
            string v16 = key4 + key4;
            string v17;
            string v18;
            if ((ServerTime & 1) > 0)
            {
                v17 = v13;
                v18 = v15;
                v15 = v16;
            }
            else
            {
                v17 = v14;
                v18 = v16;
                v14 = v13;
            }
            int v8 = ServerTime % 29;
            string v19 = v17.Substring(v8, 8);
            string v20 = v18.Substring(v8 + 8, 8);
            string v21 = v14.Substring(v8 + 16, 8);
            string v22 = v15.Substring(v8 + 24, 8);
            key = v19 + v20 + v21 + v22;
            string v25 = "IDbOjeDXWJHiJDYClEkArSWWZCHMtxcTnxBfNnoyyPxkdAClolEIRlWSkAIyqSfuwFBWrjZcFYWGUHneMszYaZCzBHhkDamPMKUzkytuiJImLpWeSXWuNcPoliCQsKpB".Substring(ServerTime % 120, 8);
            int v56 = ServerTime % 24;
            string v26 = v16.Substring(v56, 8);
            if ((ServerTime & 1) > 0)
            {
                iv = v25 + v26;
            }
            else
            {
                iv = v26 + v25;
            }
        }

    }
}
