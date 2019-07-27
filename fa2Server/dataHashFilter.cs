using log4net;
using Microsoft.AspNetCore.Http;
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
        private bool isAndroid = false;
        public dataHashFilter(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke2(HttpContext context)
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
            finally
            {
                string ServerTime = ((DateTime.Now.AddHours(8).ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
                string sign = "";

                using (StreamReader sr = new StreamReader(context.Response.Body))
                {
                    if (isAndroid)
                    {
                        sign = SignData_132Plus(ServerTime,sr.ReadToEnd());
                    }
                    else
                      sign = SignData_Ios428Plus(ServerTime, sr.ReadToEnd());
                }
                context.Response.Headers.Add("Server-Time", ServerTime);
                context.Response.Headers.Add("Sign", sign);
            }
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
            string ResponseBody="";
            try
            {
                using (var newRequest = new MemoryStream())
                {
                    string RequestBody = "";
                    using (var reader = new StreamReader(request))
                    {
                        RequestBody = await reader.ReadToEndAsync();
                    }
                   
                    //todo:这里校验签名
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
            catch (Exception exex)
            {
                log.Error(exex.Message);
                log.Error(exex.StackTrace);
                ResponseBody = "";
            }
            finally
            {
                if (string.IsNullOrEmpty(ResponseBody))
                {
                    JObject ResObj = new JObject();
                    ResObj["code"] = 1;
                    ResObj["type"] = 0;
                    ResObj["message"] = "系统错误";
                    ResponseBody = ResObj.ToString(Newtonsoft.Json.Formatting.None);
                }
                string ServerTime = ((DateTime.Now.AddHours(8).ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
                string sign = "";
                if (isAndroid)
                {
                    sign = SignData_132Plus(ServerTime, ResponseBody);
                }
                else
                    sign = SignData_Ios428Plus(ServerTime, ResponseBody);
                context.Response.Headers.Add("Server-Time", ServerTime);
                context.Response.Headers.Add("Sign", sign);
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


        private static string MD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "").ToLower();
            }
        }
        private static string SignData_132Plus(string dct, string data)
        {
            string k1 = MD5Hash(dct + "^" + dct + "!" + dct);
            string k2 = MD5Hash("zRBcyL2fy[ZsL7XJP$AIDJE*2DFF=#Dxjef2@LDLF");
            k2 = MD5Hash(dct + "#" + k2);
            return MD5Hash(k1 + data.Trim() + k2 + "TeeKB0a1Dmdg8oTovsZOu3E0o2gNhrL2");
        }
        private static string SignData_Ios428Plus(string dct, string data)
        {
            string k1 = MD5Hash(dct + "^" + dct + "!" + dct);
            string k2 = MD5Hash("zRBcyL2fy[ZsL7XJP$AIDJE*2DFF=#Dxjef2@LDLF");
            k2 = MD5Hash(dct + "#" + k2);
            return MD5Hash(k1 + data.Trim() + k2 + "U8VrXwFkELpEhiMSByrdftZQbnCUP8Vd");
        }
    }
}
