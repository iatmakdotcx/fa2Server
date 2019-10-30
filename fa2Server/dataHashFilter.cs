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

        public async Task ApiRequest_Old(HttpContext context)
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
                        var account = DbContext.Get().GetEntityDB<F2.user>().GetSingle(ii => ii.uuid == uuid);
                        if (account != null)
                            MemoryCacheService.Default.SetCache("account_" + context.TraceIdentifier, account, 1);
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
        public async Task ApiRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            var request = context.Request.Body;
            var response = context.Response.Body;
            string ResponseBody = "";
            string ErrorMessage = "";
            JObject JsonReqdata = new JObject();
            string uuid = "";
            string token = "";
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
                        JsonReqdata = DecryptRequest(context, ref RequestBody, out uuid);
                        if (JsonReqdata == null)
                        {
                            ErrorMessage = "参数错误.";
                            return;
                        }
                        sg_version = JsonReqdata["sg_version"].AsInt();
                        token = context.Request.Headers["Token"];
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
                if (string.IsNullOrEmpty(ResponseBody))
                {
                    JObject ResObj = new JObject();
                    ResObj["code"] = 1;
                    ResObj["type"] = 1;
                    ResObj["message"] = ErrorMessage;
                    ResponseBody = ResObj.ToString(Newtonsoft.Json.Formatting.None);
                }

                MemoryCacheService.Default.RemoveCache("account_" + context.TraceIdentifier);
                long ServerTime = (DateTime.Now.AddHours(8).ToUniversalTime().Ticks - 621355968000000000) / 10000000;
                context.Response.Headers.Add("Server-Time", ServerTime.ToString());
                if (context.Request.Path.Value.Contains("/api/v4/") && !context.Request.Path.Value.Contains("/sects/"))
                {
                    //加盐
                    var xx = (JObject)JsonConvert.DeserializeObject(ResponseBody);
                    xx["userDefR"] = "79K4K7bFvC00eUqp3BM799GoPlV5yP8k0R6SP7k3mrOpIKF9L5vuyOIFh23K8";
                    xx["userHpR"] = "yCfH79K4K7bFvC00eUqp3BM799GoPlV5yP8k0R6SP7k3mrOpIKF9L5vuyOIFh23K8R6X7C77A";
                    xx["userAtkR"] = "iOC582ZAyCfH79K4K7bFvC00eUqp3BM799GoPlV5yP8k0R6SP7k3mrOpIKF9L5vuyOIFh23K8R6X7";
                    ResponseBody = Response_AESEncrypt_475(xx.ToString(Newtonsoft.Json.Formatting.None), uuid, token, ServerTime);
                }
                else if (context.Request.Path.Value.Contains("/api/v4_a/"))
                {
                    //加盐
                    var xx = (JObject)JsonConvert.DeserializeObject(ResponseBody);
                    xx["userDefR"] = "79K4K7bFvC00eUqp3BM799GoPlV5yP8k0R6SP7k3mrOpIKF9L5vuyOIFh23K8";
                    xx["userHpR"] = "yCfH79K4K7bFvC00eUqp3BM799GoPlV5yP8k0R6SP7k3mrOpIKF9L5vuyOIFh23K8R6X7C77A";
                    xx["userAtkR"] = "iOC582ZAyCfH79K4K7bFvC00eUqp3BM799GoPlV5yP8k0R6SP7k3mrOpIKF9L5vuyOIFh23K8R6X7";
                    ResponseBody = Response_AESEncrypt_Android_144(xx.ToString(Newtonsoft.Json.Formatting.None), uuid, token, ServerTime);
                }
                else
                    ResponseBody = Response_AESEncrypt(ResponseBody, uuid, token, ServerTime);

                context.Response.Headers.Add("Sign2", Response_ios460_getSign(ResponseBody, token));

                using (var writer = new StreamWriter(response))
                {
                    await writer.WriteAsync(ResponseBody);
                    await writer.FlushAsync();
                }
            }
        }
        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments("/api")&& context.Request.Path!= "/v1/check_code")
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
            if (context.Request.Path.Value.EndsWith("register"))
            {
                return "";
            }
            else if (context.Request.Path.Value.EndsWith("login"))
            {
                F2.user account = MemoryCacheService.Default.GetCache<F2.user>("account_" + context.TraceIdentifier);
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
            }else if (context.Request.Path.Value.EndsWith("system_user_info"))
            {
                F2.user account = MemoryCacheService.Default.GetCache<F2.user>("account_" + context.TraceIdentifier);
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
                F2.user account = MemoryCacheService.Default.GetCache<F2.user>("account_" + context.TraceIdentifier);
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
#if DEBUG
#else
                    if (account.net_id >= net_id)
                    {
                        return "无效的网络请求!";
                    }
#endif
                    account.net_id = net_id;
                    var dbh = DbContext.Get();
                    dbh.Db.Updateable(account).UpdateColumns(ii => ii.net_id).ExecuteCommand();
                }
            }
            return "";
        }
        private static JObject DecryptRequest(HttpContext context, ref string bodyData, out string uuid)
        {
            uuid = null;
            if (!bodyData.StartsWith("{\"data\":\""))
            {
                return null;
            }
            bodyData = bodyData.Substring(9, bodyData.Length - 11).Replace("\\","");
            string token = context.Request.Headers["Token"];
            string sign2 = ios460_getSign(bodyData, token);
            if (sign2 != context.Request.Headers["Sign2"])
            {
                return null;
            }
            long ServerTime = long.Parse(context.Request.Headers["Server-Time"]);
            F2.user account = null;
            var dbh = DbContext.Get();
            JObject dJo = null;
            if (context.Request.Path.Value.EndsWith("register"))
            {
                return null;
            }else
            if (context.Request.Path.Value.EndsWith("first_login"))
            {
                bodyData = AESDecrypt(bodyData, "", token, ServerTime);
                if (bodyData == null)
                {
                    return null;
                }
                dJo = (JObject)JsonConvert.DeserializeObject(bodyData);
                account = dbh.Db.Queryable<F2.user>()
                    //.IgnoreColumns("cheatMsg", "ClientCheatMsg", "userdata", "player_data", "player_zhong_yao")
                    .First(ii => ii.username == dJo["user_name"].ToString());
            }
            else if (context.Request.Path.Value.Contains("/api/v4/") && !context.Request.Path.Value.Contains("/sects/"))
            {
                account = dbh.Db.Queryable<F2.user>()
                    //.IgnoreColumns("cheatMsg", "ClientCheatMsg", "userdata", "player_data", "player_zhong_yao")
                    .First(ii => ii.token == token);
                if (account == null)
                {
                    return null;
                }
                if (context.Request.Headers["User-Agent"].ToString().IndexOf("Darwin") == -1)
                {
                    //is Android
                    context.Request.Path = new PathString(context.Request.Path.Value.Replace("/api/v4/", "/api/v4_a/"));
                    bodyData = AESDecrypt_Android_144(bodyData, account.uuid, token, ServerTime);
                }
                else
                {
                    bodyData = AESDecrypt_475(bodyData, account.uuid, token, ServerTime);
                }
                if (bodyData == null)
                {
                    return null;
                }
                dJo = (JObject)JsonConvert.DeserializeObject(bodyData);
                uuid = account.uuid;
            }
            else if (!string.IsNullOrEmpty(token))
            {
                account = dbh.Db.Queryable<F2.user>()
                    //.IgnoreColumns("cheatMsg", "ClientCheatMsg", "userdata", "player_data", "player_zhong_yao")
                    .First(ii => ii.token == token);
                if (account == null)
                {
                    return null;
                }
                bodyData = AESDecrypt(bodyData, account.uuid, token, ServerTime);
                if (bodyData == null)
                {
                    return null;
                }
                dJo = (JObject)JsonConvert.DeserializeObject(bodyData);
                uuid = account.uuid;
            }
            if (account == null)
            {
                return null;
            }
            MemoryCacheService.Default.SetCache("account_" + context.TraceIdentifier, account, 1);
            return dJo;
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


       
        public static string AESEncrypt_475(string Data, string uuid, string token, long ServerTime)
        {
            string key;
            string iv;
            string randStr = "UiPL68O700HViOC582ZAyCfH79K4K7bFvC00eUqp3BM799GoPlV5yP8k0R6SP7k3mrOpIKF9L5vuyOIFh23K8R6X7C77ANY6w4n0bTV9dageVA1SD5Anhw2t6ULfyvNY";
            ios475_getAESKey(uuid, token, ServerTime, randStr, out key, out iv);
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
            string resultStr = Convert.ToBase64String(Cryptograph);
            for (int i = 0; i < 128/4; i++)
            {
                string subStr = randStr.Substring(i * 4, 4);
                resultStr = resultStr.Insert(6 + i * 6 + i * 4, subStr);
            }
            return resultStr;
        }
        public static string AESDecrypt_475(string Data, string uuid, string token, long ServerTime)
        {
            string key;
            string iv;
            string randStr = "";
            for (int i = 0; i < 128 / 4; i++)
            {
                randStr += Data.Substring(6 + i * 6, 4);
                Data = Data.Remove(6 + i * 6, 4);
            }
            ios475_getAESKey(uuid, token, ServerTime, randStr, out key, out iv);

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
            catch(Exception e)
            {
                return null;
            }
            return Encoding.UTF8.GetString(original);
        }
        public static void ios475_getAESKey(string uuid, string token, long ServerTime, string randStr, out string key, out string iv)
        {
            string key1 = MD5Hash(uuid + (ServerTime / 29).ToString() + randStr.Substring(0, 0x20));
            string key2 = MD5Hash(((ServerTime % 29) + ServerTime).ToString() + randStr.Substring(0x20, 0x20));
            string key3 = MD5Hash((ServerTime % 29).ToString() + MD5Hash(token).Substring(4, 16) + randStr.Substring(0x40, 0x20));
            string key4 = MD5Hash(key1 + key2 + key3 + randStr.Substring(0x60, 0x20));

            string v20 = key1 + key1;
            string v21 = key2 + key2;
            string v22 = key3 + key3;
            string v23 = key4 + key4;

            int v25 = (int)(ServerTime % 29);
            if ((ServerTime & 1) > 0)
            {
                key = v20.Substring(v25, 3) + 
                      v22.Substring(v25 + 3, 11) + 
                      v21.Substring(v25 + 14, 7) + 
                      v23.Substring(v25 + 21, 11);
            }
            else
            {
                key = v21.Substring(v25, 5) +
                      v23.Substring(v25 + 5, 7) +
                      v20.Substring(v25 + 12, 12) +
                      v22.Substring(v25 + 24, 8);
            }
            string v36 = "IDbOjeDXWJHiJDYClEkArSWWZCHMtxcTnxBfNnoyyPxkdAClolEIRlWSkAIyqSfuwFBWrjZcFYWGUHneMszYaZCzBHhkDamPMKUzkytuiJImLpWeSXWuNcPoliCQsKpB".Substring((int)(ServerTime % 120), 8);
            string v37 = v23.Substring((int)(ServerTime % 24), 8);
            if ((ServerTime & 1) > 0)
            {
                iv = v36 + v37;
            }
            else
            {
                iv = v37 + v36;
            }
        }

        public static string Response_AESDecrypt_475(string Data, string uuid, string token, long ServerTime)
        {
            string key;
            string iv;
            string randStr = "";
            for (int i = 0; i < 128 / 4; i++)
            {
                randStr += Data.Substring(6 + i * 6, 4);
                Data = Data.Remove(6 + i * 6, 4);
            }
            Response_ios475_getAESKey(uuid, token, ServerTime, randStr, out key, out iv);

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
        public static string Response_AESEncrypt_475(string Data, string uuid, string token, long ServerTime)
        {
            string key;
            string iv;
            string randStr = "UiPL68O700HViOC582ZAyCfH79K4K7bFvC00eUqp3BM799GoPlV5yP8k0R6SP7k3mrOpIKF9L5vuyOIFh23K8R6X7C77ANY6w4n0bTV9dageVA1SD5Anhw2t6ULfyvNY";
            Response_ios475_getAESKey(uuid, token, ServerTime, randStr, out key, out iv);

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
            string resultStr = Convert.ToBase64String(Cryptograph);
            for (int i = 0; i < 128 / 4; i++)
            {
                string subStr = randStr.Substring(i * 4, 4);
                resultStr = resultStr.Insert(6 + i * 6 + i * 4, subStr);
            }
            return resultStr;
        }
        public static void Response_ios475_getAESKey(string uuid, string token, long ServerTime, string randStr, out string key, out string iv)
        {
            string key1 = MD5Hash(uuid + (ServerTime / 27).ToString() + randStr.Substring(0, 0x20));
            string key2 = MD5Hash(((ServerTime % 27) + ServerTime).ToString() + randStr.Substring(0x20, 0x20));
            string key3 = MD5Hash((ServerTime % 27).ToString() + MD5Hash(token).Substring(4, 16) + randStr.Substring(0x40, 0x20));
            string key4 = MD5Hash(key1 + key2 + key3 + randStr.Substring(0x60, 0x20));

            string v36 = key1 + key1;
            string v37 = key2 + key2;
            string v38 = key3 + key3;
            string v39 = key4 + key4;
            int v40 = (int)(ServerTime % 27);
            if ((ServerTime & 1) > 0)
            {
                key = v37.Substring(v40, 11) +
                      v39.Substring(v40 + 11, 4) +
                      v36.Substring(v40 + 15, 11) +
                      v38.Substring(v40 + 26, 6);
            }
            else
            {
                key = v36.Substring(v40, 3) +
                      v38.Substring(v40 + 3, 11) +
                      v37.Substring(v40 + 14, 7) +
                      v39.Substring(v40 + 21, 11);
            }
            string v25 = "4Sxw7ir3Ul9inXLtvsWVaHTCZY809sWQmf3pUzQ3WqGrJJnTMFFA4Oz9oQIT8wRii7l00ORvTWU4Oh9Ao6ezjK1LXeOq8FIpL7xSsjYhi2Ks7UoYOGk8TPxIzJAda38b".Substring((int)(ServerTime % 119), 9);            
            string v26 = v39.Substring((int)(ServerTime % 25), 7);
            if ((ServerTime & 1) > 0)
            {
                iv = v25 + v26;
            }
            else
            {
                iv = v26 + v25;
            }
        }


        public static string AESDecrypt(string Data, string uuid, string token, long ServerTime)
        {
            string key;
            string iv;
            ios460_getAESKey(uuid, token, ServerTime, out key, out iv);

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
        public static string AESEncrypt(string Data, string uuid, string token, long ServerTime)
        {
            string key;
            string iv;
            ios460_getAESKey(uuid, token, ServerTime, out key, out iv);

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
        public static void ios460_getAESKey(string uuid, string token, long ServerTime, out string key, out string iv)
        {
            string key1 = MD5Hash(uuid + (ServerTime / 29).ToString());
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
            int v8 = (int)(ServerTime % 29);
            string v19 = v17.Substring(v8, 8);
            string v20 = v18.Substring(v8 + 8, 8);
            string v21 = v14.Substring(v8 + 16, 8);
            string v22 = v15.Substring(v8 + 24, 8);
            key = v19 + v20 + v21 + v22;
            string v25 = "IDbOjeDXWJHiJDYClEkArSWWZCHMtxcTnxBfNnoyyPxkdAClolEIRlWSkAIyqSfuwFBWrjZcFYWGUHneMszYaZCzBHhkDamPMKUzkytuiJImLpWeSXWuNcPoliCQsKpB".Substring((int)(ServerTime % 120), 8);
            int v56 = (int)(ServerTime % 24);
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

        public static string Response_AESDecrypt(string Data, string uuid, string token, long ServerTime)
        {
            string key;
            string iv;
            Response_ios460_getAESKey(uuid, token, ServerTime, out key, out iv);

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
        public static string Response_AESEncrypt(string Data, string uuid, string token, long ServerTime)
        {
            string key;
            string iv;
            Response_ios460_getAESKey(uuid, token, ServerTime, out key, out iv);

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
        public static void Response_ios460_getAESKey(string uuid, string token, long ServerTime, out string key, out string iv)
        {
            string key1 = MD5Hash(uuid + (ServerTime / 27).ToString());
            string key2 = MD5Hash(((ServerTime % 27) + ServerTime).ToString());
            string key3 = MD5Hash((ServerTime % 27).ToString() + MD5Hash(token).Substring(4, 16));
            string key4 = MD5Hash(key1 + key2 + key3);

            string v14 = key1 + key1;
            string v15 = key2 + key2;
            string v16 = key3 + key3;
            string v17 = key4 + key4;
            string v18;
            string v19;
            if ((ServerTime & 1) > 0)
            {
                v18 = v15;
                v19 = v17;
                v15 = v14;
            }
            else
            {
                v18 = v14;
                v19 = v16;
                v16 = v17;
            }
            int v8 = (int)(ServerTime % 27);
            string v20 = v18.Substring(v8, 8);
            string v21 = v19.Substring(v8 + 8, 8);
            string v22 = v15.Substring(v8 + 16, 8);
            string v23 = v16.Substring(v8 + 24, 8);
            key = v20 + v21 + v22 + v23;
            string v25 = "4Sxw7ir3Ul9inXLtvsWVaHTCZY809sWQmf3pUzQ3WqGrJJnTMFFA4Oz9oQIT8wRii7l00ORvTWU4Oh9Ao6ezjK1LXeOq8FIpL7xSsjYhi2Ks7UoYOGk8TPxIzJAda38b".Substring((int)(ServerTime % 119), 9);
            long v56 = ServerTime % 25;
            string v26 = v17.Substring((int)v56, 7);
            if ((ServerTime & 1) > 0)
            {
                iv = v25 + v26;
            }
            else
            {
                iv = v26 + v25;
            }
        }

        public static string ios460_getSign(string data, string token)
        {
            string v61 = MD5Hash(data);
            string v62 = MD5Hash(token);
            var tmpstring =
                v61.Substring(17, 3) +
                v62.Substring(22, 2) +
                v62.Substring(14, 2) +
                v62.Substring(6, 3) +
                v61.Substring(29, 1) +
                v61.Substring(4, 1) +
                v62.Substring(23, 4) +
                v62.Substring(18, 2) +
                v62.Substring(11, 3) +
                v62.Substring(25, 3) +
                v61.Substring(13, 3) +
                v61.Substring(16, 4) +
                v62.Substring(11, 1);
            return MD5Hash(tmpstring);
        }

        public static string Response_ios460_getSign(string data, string token)
        {
            string v13 = MD5Hash(data);
            string v14 = MD5Hash(token);
            var tmpstring = v13.Substring(13, 3) +
            v14.Substring(28, 2) +
            v14.Substring(4, 2) +
            v14.Substring(16, 3) +
            v13.Substring(15, 1) +
            v13.Substring(24, 1) +
            v14.Substring(23, 4) +
            v14.Substring(11, 2) +
            v14.Substring(7, 3) +
            v14.Substring(25, 3) +
            v13.Substring(19, 3) +
            v13.Substring(3, 4) +
            v14.Substring(22, 1);
            return MD5Hash(tmpstring);
        }


        public static string AESDecrypt_Android_144(string Data, string uuid, string token, long ServerTime)
        {
            string key;
            string iv;
            string randStr = "";
            for (int i = 0; i < 128 / 4; i++)
            {
                randStr += Data.Substring(6 + i * 6, 4);
                Data = Data.Remove(6 + i * 6, 4);
            }
            getAESKey_Android_144(uuid, token, ServerTime, randStr, out key, out iv);

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
            catch (Exception e)
            {
                return null;
            }
            return Encoding.UTF8.GetString(original);
        }
        /// <summary>
        /// 安卓星空探索
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="token"></param>
        /// <param name="ServerTime"></param>
        /// <param name="randStr"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        public static void getAESKey_Android_144(string uuid, string token, long ServerTime, string randStr, out string key, out string iv)
        {
            string key1 = MD5Hash(uuid + (ServerTime / 29).ToString() + randStr.Substring(0, 0x20));
            string key2 = MD5Hash(((ServerTime % 29) + ServerTime).ToString() + randStr.Substring(0x20, 0x20));
            string key3 = MD5Hash((ServerTime % 29).ToString() + MD5Hash(token).Substring(4, 16) + randStr.Substring(0x40, 0x20));
            string key4 = MD5Hash(key1 + key2 + key3 + randStr.Substring(0x60, 0x20));

            string key1_v = key1 + key1;
            string key2_v = key2 + key2;
            string key3_v = key3 + key3;
            string key4_v = key4 + key4;

            int v25 = (int)(ServerTime % 29);
            if ((ServerTime & 1) > 0)
            {
                key = key1_v.Substring(v25, 7) +
                      key3_v.Substring(v25 + 7, 9) +
                      key2_v.Substring(v25 + 16, 11) +
                      key4_v.Substring(v25 + 27, 5);
            }
            else
            {
                key = key2_v.Substring(v25, 6) +
                      key4_v.Substring(v25 + 6, 8) +
                      key1_v.Substring(v25 + 14, 12) +
                      key3_v.Substring(v25 + 26, 6);
            }
            string v36 = "IDbOjeDXWJHiJDYClEkArSWWZCHMtxcTnxBfNnoyyPxkdAClolEIRlWSkAIyqSfuwFBWrjZcFYWGUHneMszYaZCzBHhkDamPMKUzkytuiJImLpWeSXWuNcPoliCQsKpB".Substring((int)(ServerTime % 120), 8);
            string v37 = key4_v.Substring((int)(ServerTime % 24), 8);
            if ((ServerTime & 1) > 0)
            {
                iv = v36 + v37;
            }
            else
            {
                iv = v37 + v36;
            }
        }


        public static string Response_AESDecrypt_Android_144(string Data, string uuid, string token, long ServerTime)
        {
            string key;
            string iv;
            string randStr = "";
            for (int i = 0; i < 128 / 4; i++)
            {
                randStr += Data.Substring(6 + i * 6, 4);
                Data = Data.Remove(6 + i * 6, 4);
            }
            Response_getAESKey_Android_144(uuid, token, ServerTime, randStr, out key, out iv);

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
        public static string Response_AESEncrypt_Android_144(string Data, string uuid, string token, long ServerTime)
        {
            string key;
            string iv;
            string randStr = "UiPL68O700HViOC582ZAyCfH79K4K7bFvC00eUqp3BM799GoPlV5yP8k0R6SP7k3mrOpIKF9L5vuyOIFh23K8R6X7C77ANY6w4n0bTV9dageVA1SD5Anhw2t6ULfyvNY";
            Response_getAESKey_Android_144(uuid, token, ServerTime, randStr, out key, out iv);

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
            string resultStr = Convert.ToBase64String(Cryptograph);
            for (int i = 0; i < 128 / 4; i++)
            {
                string subStr = randStr.Substring(i * 4, 4);
                resultStr = resultStr.Insert(6 + i * 6 + i * 4, subStr);
            }
            return resultStr;
        }
        public static void Response_getAESKey_Android_144(string uuid, string token, long ServerTime, string randStr, out string key, out string iv)
        {
            string key1 = MD5Hash(uuid + (ServerTime / 27).ToString() + randStr.Substring(0, 0x20));
            string key2 = MD5Hash(((ServerTime % 27) + ServerTime).ToString() + randStr.Substring(0x20, 0x20));
            string key3 = MD5Hash((ServerTime % 27).ToString() + MD5Hash(token).Substring(4, 16) + randStr.Substring(0x40, 0x20));
            string key4 = MD5Hash(key1 + key2 + key3 + randStr.Substring(0x60, 0x20));

            string key1_v = key1 + key1;
            string key2_v = key2 + key2;
            string key3_v = key3 + key3;
            string key4_v = key4 + key4;

            int v25 = (int)(ServerTime % 27);
            if ((ServerTime & 1) > 0)
            {
                key = key2_v.Substring(v25, 7) +
                      key4_v.Substring(v25 + 7, 4) +
                      key1_v.Substring(v25 + 11, 12) +
                      key3_v.Substring(v25 + 23, 9);
            }
            else
            {
                key = key1_v.Substring(v25, 5) +
                      key3_v.Substring(v25 + 5, 13) +
                      key2_v.Substring(v25 + 18, 8) +
                      key4_v.Substring(v25 + 26, 6);
            }
            string v25x = "4Sxw7ir3Ul9inXLtvsWVaHTCZY809sWQmf3pUzQ3WqGrJJnTMFFA4Oz9oQIT8wRii7l00ORvTWU4Oh9Ao6ezjK1LXeOq8FIpL7xSsjYhi2Ks7UoYOGk8TPxIzJAda38b".Substring((int)(ServerTime % 119), 9);
            string v26 = key4_v.Substring((int)(ServerTime % 25), 7);
            if ((ServerTime & 1) > 0)
            {
                iv = v25x + v26;
            }
            else
            {
                iv = v26 + v25x;
            }
        }

    }
}
