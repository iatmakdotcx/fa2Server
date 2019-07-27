using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MakC.Common;
using MakC.Data;
using MakC.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace fa2Server.Controllers
{
    
    [ApiController]
    public class GameServerController : ControllerBase
    {
        [HttpPost("api/v2/users/register")]
        public JObject register([FromBody] JObject value)
        {
            return new JObject(new JProperty("code", "1"), new JProperty("type", "4"), new JProperty("message", "注册已关闭"));
        }
        [HttpPost("api/v1/users/first_login")]
        public JObject first_login([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 8;
            string username = value["user_name"].ToString();
            string password = value["password"].ToString();

            var dbh = DbContext.Get();
            F2.user account = dbh.GetEntityDB<F2.user>().GetSingle(ii => ii.username == username);
            if (account == null)
            {
                ResObj["message"] = "用户不存在！";
                return ResObj;
            }            
            if (account.password != password)
            {
                ResObj["message"] = "密码错误";
                return ResObj;
            }

            account.token = Guid.NewGuid().ToString();

            JObject player_data = (JObject)JsonConvert.DeserializeObject(account.player_data);
            player_data["playerDict"]["uuid"] = account.uuid;
            player_data["playerDict"]["token"] = account.token;
            player_data["playerDict"]["password"] = account.password;
            player_data["playerDict"]["user_name"] = account.username;
            if (player_data["playerDict"]["playerId"] == null || !player_data["playerDict"]["playerId"].ToString().StartsWith("G"))
            {
                player_data["playerDict"]["playerId"] = "G:00000000003";
            }
            account.player_data = player_data.ToString(Formatting.None);

            ResObj["data"] = new JObject();
            ResObj["data"]["login_time"] = account.lastLoginTime.AsTimestamp();
            ResObj["data"]["player_data"] = account.player_data;
            ResObj["data"]["player_zhong_yao"] = account.player_zhong_yao;
            ResObj["data"]["userdata"] = account.userdata;
            ResObj["data"]["uuid"] = account.uuid;
            ResObj["code"] = 0;

            account.lastLoginTime = DateTime.Now;
            dbh.Db.Updateable(account).ExecuteCommand();

            return ResObj;
        }
        [HttpPost("api/v2/users/login")]
        public JObject login([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 3;
            string username = value["user_name"].ToString();
            string password = value["password"].ToString();            
            string uuid = value["uuid"].ToString();

            var dbh = DbContext.Get();
            F2.user account = dbh.GetEntityDB<F2.user>().GetSingle(ii => ii.username == username);
            if (account == null)
            {
                ResObj["message"] = "用户不存在！";
                return ResObj;
            }
            if (account.password != password)
            {
                ResObj["message"] = "密码错误";
                return ResObj;
            }
            F2.setting setting = dbh.GetEntityDB<F2.setting>().GetSingle(ii => ii.id == (account.isAndroid ? 2 : 1));
            var fscq = new JObject();
            fscq["id"] = setting.id;
            fscq["CHONGZHI"] = (JToken)JsonConvert.DeserializeObject(setting.CHONGZHI);
            fscq["CHONGZHI2"] = (JToken)JsonConvert.DeserializeObject(setting.CHONGZHI2);
            fscq["CURVESION"] = setting.CURVESION;
            fscq["DAQUNUM"] = setting.DAQUNUM;
            fscq["DBLOWLV"] = setting.DBLOWLV;
            fscq["GIFT"] = (JToken)JsonConvert.DeserializeObject(setting.GIFT);
            fscq["GONGGAO"] = (JToken)JsonConvert.DeserializeObject(setting.GONGGAO);
            fscq["HDZX"] = (JToken)JsonConvert.DeserializeObject(setting.HDZX);
            fscq["HUODONG"] = (JToken)JsonConvert.DeserializeObject(setting.HUODONG);
            fscq["INREVIEW"] = setting.INREVIEW;
            fscq["LOWLEVEL"] = setting.LOWLEVEL;
            fscq["LOWLEVELEXP"] = setting.LOWLEVELEXP;
            fscq["RATE"] = setting.RATE;
            fscq["SHOUCHONG"] = (JToken)JsonConvert.DeserializeObject(setting.SHOUCHONG);
            fscq["TENLOWLV"] = setting.TENLOWLV;
            fscq["THLOWLV"] = setting.THLOWLV;

            ResObj["data"] = new JObject();
            ResObj["data"]["fscq"] = fscq.ToString(Formatting.None);
            ResObj["data"]["isDLSave"] =0;
            ResObj["data"]["lastDCTime"] = account.lastDCTime;
            ResObj["data"]["login_time"] = account.lastLoginTime.AsTimestamp();
            ResObj["data"]["net_id"] = account.net_id;
            ResObj["data"]["token"] = account.token;
            ResObj["data"]["userdata"] = account.userdata;
            ResObj["data"]["uuid"] = account.uuid;
            ResObj["code"] = 0;
            return ResObj;
        }

        [HttpPost("api/v2/users/save_user")]
        public JObject save_user([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            string uuid = value["uuid"].ToString();
            string token = value["token"].ToString();
            int net_id = value["net_id"].ToString().AsInt();

            var dbh = DbContext.Get();
            F2.user account = dbh.GetEntityDB<F2.user>().GetSingle(ii => ii.uuid == uuid);
            if (account == null)
            {
                ResObj["message"] = "用户不存在！";
                return ResObj;
            }
            if (account.token != token)
            {
                ResObj["message"] = "账号已在其它地方登录";
                return ResObj;
            }
            if (account.net_id >= net_id)
            {
                ResObj["message"] = "无效的网络请求!";
                return ResObj;
            }
            account.player_data = value["player_data"].ToString();
            account.player_zhong_yao = value["player_zhong_yao"].ToString();
            account.userdata = value["userdata"].ToString();

            JObject zhong_yao = (JObject)JsonConvert.DeserializeObject(account.player_zhong_yao);

            account.net_id = net_id;
            account.lastDCTime = zhong_yao["lastDCTime"].ToString().AsInt();
            dbh.Db.Updateable(account).ExecuteCommand();

            ResObj["data"] = new JObject();           
            ResObj["data"]["player_zhong_yao"] = account.player_zhong_yao;
            ResObj["data"]["read_time"] = 28800;
            ResObj["data"]["uuid"] = account.uuid;
            ResObj["data"]["write_time"] = DateTime.Now.AsTimestamp();

            ResObj["code"] = 0;
            ResObj["type"] = 2;
            return ResObj;
        }

        [HttpPost("api/v2/shops/list")]
        public JObject shops_list([FromBody] JObject value)
        {

            //TODO://weiwanc
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            string uuid = value["uuid"].ToString();
            string token = value["token"].ToString();
            int net_id = value["net_id"].ToString().AsInt();

            var dbh = DbContext.Get();
            F2.user account = dbh.GetEntityDB<F2.user>().GetSingle(ii => ii.uuid == uuid);
            if (account == null)
            {
                ResObj["message"] = "用户不存在！";
                return ResObj;
            }
            if (account.token != token)
            {
                ResObj["message"] = "账号已在其它地方登录";
                return ResObj;
            }
            if (account.net_id >= net_id)
            {
                ResObj["message"] = "无效的网络请求!";
                return ResObj;
            }
            account.player_data = value["player_data"].ToString();
            account.player_zhong_yao = value["player_zhong_yao"].ToString();
            account.userdata = value["userdata"].ToString();

            JObject zhong_yao = (JObject)JsonConvert.DeserializeObject(account.player_zhong_yao);

            account.net_id = net_id;
            account.lastDCTime = zhong_yao["lastDCTime"].ToString().AsInt();
            dbh.Db.Updateable(account).ExecuteCommand();

            ResObj["data"] = new JObject();
            ResObj["data"]["player_zhong_yao"] = account.player_zhong_yao;
            ResObj["data"]["read_time"] = 28800;
            ResObj["data"]["uuid"] = account.uuid;
            ResObj["data"]["write_time"] = DateTime.Now.AsTimestamp();

            ResObj["code"] = 0;
            ResObj["type"] = 2;
            return ResObj;
        }
    }
}