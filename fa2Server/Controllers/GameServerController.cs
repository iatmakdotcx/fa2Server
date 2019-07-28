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

        private F2.user getUserFromCache()
        {
            return MemoryCacheService.Default.GetCache<F2.user>("account_" + HttpContext.TraceIdentifier) ?? throw new Exception("用户信息已过期？？？？？？？？");
        }
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

            if (string.IsNullOrEmpty(account.uuid))
            {
                account.uuid = Guid.NewGuid().ToString();
                account.userdata = "{}";
                account.isAndroid = HttpContext.Request.Headers["User-Agent"].ToString().IndexOf("Darwin") == -1;
                var setting = CahceHelper.GetDBSetting(account.isAndroid);
                account.player_data = setting.base_playerData;
                account.player_zhong_yao = setting.base_playerzhongyao;                
            }

            JObject player_data = (JObject)JsonConvert.DeserializeObject(account.player_data);
            player_data["playerDict"]["uuid"] = account.uuid;
            player_data["playerDict"]["token"] = account.token;
            player_data["playerDict"]["password"] = account.password;
            player_data["playerDict"]["user_name"] = account.username;
            if (player_data["playerDict"]["playerId"] == null || !player_data["playerDict"]["playerId"].ToString().StartsWith("G"))
            {
                player_data["playerDict"]["playerId"] = "G:00000000003";
            }
            account.lastLoginTime = DateTime.Now;
            account.player_data = player_data.ToString(Formatting.None);
            if (dbh.Db.Updateable(account).ExecuteCommand() == 0)
            {
                ResObj["message"] = "稍后重试！";
                return ResObj;
            }

            ResObj["data"] = new JObject();
            ResObj["data"]["login_time"] = account.lastLoginTime.AsTimestamp();
            ResObj["data"]["player_data"] = account.player_data;
            ResObj["data"]["player_zhong_yao"] = account.player_zhong_yao;
            ResObj["data"]["userdata"] = account.userdata;
            ResObj["data"]["uuid"] = account.uuid;
            ResObj["code"] = 0;
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
            F2.setting setting = CahceHelper.GetDBSetting(account.isAndroid);
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
            account.net_id = net_id;
            dbh.Db.Updateable(account).ExecuteCommand();

            ResObj["data"] = new JObject();
            ResObj["data"]["ling"] = account.shl;
            ResObj["data"]["binding_ling"] = account.bdshl;
            var list = new JArray();
            List<F2.shop> shops = dbh.Db.Queryable<F2.shop>().Take(100).Where(ii => ii.buyer_uuid == null).ToList();
            foreach (var item in shops)
            {
                var a = new JObject();
                a["buyer_uuid"] = item.buyer_uuid??"";
                a["created_at"] = item.created_at.AsTimestamp();
                a["data"] = item.data;
                a["id"] = item.id;
                a["item_id"] = item.item_id;
                a["item_name"] = item.item_name??"";
                a["price"] = item.price;
                a["updated_at"] = item.updated_at.AsTimestamp();
                a["uuid"] = item.uuid;
                list.Add(a);
            }
            ResObj["data"]["list"] = list;
            ResObj["code"] = 0;
            ResObj["type"] = 18;
            return ResObj;
        }
        [HttpPost("api/v2/shops/search")]
        public JObject shops_search([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
           

            ResObj["data"] = new JObject();

            ResObj["code"] = 0;
            ResObj["type"] = 18;
            return ResObj;
        }
        [HttpPost("api/v2/shops/owner_shop")]
        public JObject shops_owner_shop([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
           

            ResObj["data"] = new JObject();

            ResObj["code"] = 0;
            ResObj["type"] = 18;
            return ResObj;
        }
        [HttpPost("api/v2/shops/owner_sell_logs")]
        public JObject shops_sell_logs([FromBody] JObject value)
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
            account.net_id = net_id;
            dbh.Db.Updateable(account).ExecuteCommand();

            var list = new JArray();
            List<F2.shop> shops = dbh.Db.Queryable<F2.shop>().Take(100).Where(ii => ii.uuid == uuid).ToList();
            foreach (var item in shops)
            {
                var a = new JObject();
                a["buyer_uuid"] = item.buyer_uuid ?? "";
                a["created_at"] = item.created_at.AsTimestamp();
                a["data"] = item.data;
                a["id"] = item.id;
                a["item_id"] = item.item_id;
                a["item_name"] = item.item_name ?? "";
                a["price"] = item.price;
                a["updated_at"] = item.updated_at.AsTimestamp();
                a["uuid"] = item.uuid;
                list.Add(a);
            }
            ResObj["data"] = list;

            ResObj["code"] = 0;
            ResObj["type"] = 18;
            return ResObj;
        }
        [HttpPost("api/v2/shops/sell")]
        public JObject shops_sell([FromBody] JObject value)
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
            account.net_id = net_id;
            account.player_data = value["player_data"].ToString();
            account.player_zhong_yao = value["player_zhong_yao"].ToString();
            dbh.Db.Updateable(account).ExecuteCommand();


            var list = new JArray();
            List<F2.shop> shops = dbh.Db.Queryable<F2.shop>().Take(100).Where(ii => ii.uuid == uuid).ToList();
            //if (shops.Count > 10)
            //{
            //    ResObj["message"] = "寄售物品数量已到上限";
            //    return ResObj;
            //}
 

            F2.shop shopItem = new F2.shop();
            shopItem.created_at = DateTime.Now;
            shopItem.updated_at = DateTime.Now;
            shopItem.data = value["data"].ToString();
            shopItem.price = value["price"].ToString().AsInt();
            shopItem.item_name = value["item_name"].ToString();
            shopItem.item_id = ((JObject)JsonConvert.DeserializeObject(shopItem.data))["itemType"].ToString();
            shopItem.uuid = uuid;
            dbh.Db.Insertable(shopItem).ExecuteCommand();

            shops.Add(shopItem);
            foreach (var item in shops)
            {
                var a = new JObject();
                a["buyer_uuid"] = item.buyer_uuid ?? "";
                a["created_at"] = item.created_at.AsTimestamp();
                a["data"] = item.data;
                a["id"] = item.id;
                a["item_id"] = item.item_id;
                a["item_name"] = item.item_name ?? "";
                a["price"] = item.price;
                a["updated_at"] = item.updated_at.AsTimestamp();
                a["uuid"] = item.uuid;
                list.Add(a);
            }            
            ResObj["data"] = list;

            ResObj["code"] = 0;
            ResObj["type"] = 17;
            return ResObj;
        }
    }
}