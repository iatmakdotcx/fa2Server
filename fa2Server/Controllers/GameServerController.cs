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

        private F2.sect_member updateSectInfo(F2.user account)
        {
            var dbh = DbContext.Get();
            F2.sect_member sectMember = dbh.GetEntityDB<F2.sect_member>().GetSingle(ii => ii.playerId == account.id);
            if (sectMember == null)
            {
                sectMember = new F2.sect_member();
            }
            sectMember.last_login_time = DateTime.Now;
            sectMember.playerName = ((JObject)JsonConvert.DeserializeObject(account.player_data))["playerDict"]?["playName"]?.ToString() ?? account.username;
            sectMember.playerId = account.id;
            sectMember.playerUuid = account.uuid;
            sectMember.HYJF = 0;//会员等级

            if (sectMember.id == 0)
            {
                dbh.Db.Insertable(sectMember).ExecuteReturnIdentity();
            }
            else
            {
                dbh.Db.Updateable(sectMember).ExecuteCommand();
            }
            return sectMember;
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
            var dbh = DbContext.Get();
            F2.user account = getUserFromCache();
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
            F2.user account = getUserFromCache();
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
            ResObj["data"]["login_time"] = DateTime.Now.AsTimestamp();
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
            var dbh = DbContext.Get();
            F2.user account = getUserFromCache();
            account.player_data = value["player_data"].ToString();
            account.player_zhong_yao = value["player_zhong_yao"].ToString();
            account.userdata = value["userdata"].ToString();
            JObject zhong_yao = (JObject)JsonConvert.DeserializeObject(account.player_zhong_yao);
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
            var dbh = DbContext.Get();
            F2.user account = getUserFromCache();

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
            string item_name = value["item_name"].ToString();

            var dbh = DbContext.Get();
            var list = new JArray();
            List<F2.shop> shops = dbh.Db.Queryable<F2.shop>().Take(100).Where(ii => ii.item_name.Contains(item_name)).ToList();
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
            ResObj["type"] = 19;
            return ResObj;
        }
        [HttpPost("api/v2/shops/owner_shop")]
        public JObject shops_owner_shop([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;

            var dbh = DbContext.Get();
            var list = new JArray();
            List<F2.shop> shops = dbh.Db.Queryable<F2.shop>().Take(100).Where(ii => ii.uuid == value["uuid"].ToString() && ii.buyer_uuid == null).ToList();
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
            ResObj["type"] = 22;
            return ResObj;
        }
        [HttpPost("api/v2/shops/sell_logs")]
        public JObject shops_sell_logs([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            var dbh = DbContext.Get();
            var list = new JArray();
            List<F2.shop> shops = dbh.Db.Queryable<F2.shop>().Take(100).Where(ii => ii.uuid == value["uuid"].ToString() && ii.buyer_uuid != null).ToList();
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
            ResObj["type"] = 25;
            return ResObj;
        }
        [HttpPost("api/v2/shops/sell")]
        public JObject shops_sell([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            var dbh = DbContext.Get();
            F2.user account = getUserFromCache();
            account.player_data = value["player_data"].ToString();
            account.player_zhong_yao = value["player_zhong_yao"].ToString();
            dbh.Db.Updateable(account).ExecuteCommand();


            var list = new JArray();
            List<F2.shop> shops = dbh.Db.Queryable<F2.shop>().Take(100).Where(ii => ii.uuid == account.uuid).ToList();
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
            shopItem.uuid = account.uuid;
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
        [HttpPost("api/v2/shops/buy")]
        public JObject shops_buy([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            int id = value["id"].ToString().AsInt();

            var dbh = DbContext.Get();
            F2.user account = getUserFromCache();

            F2.shop shopItem = dbh.GetEntityDB<F2.shop>().GetById(id);
            if (shopItem == null)
            {
                ResObj["messgae"] = "物品不存在！";
                return ResObj;
            }
            if (account.shl < shopItem.price)
            {
                ResObj["messgae"] = "商会令不足！";
                return ResObj;
            }
            shopItem.buyer_uuid = account.uuid;
            dbh.Db.BeginTran();
            int optCnt = dbh.Db.Updateable(shopItem).UpdateColumns(ii=>ii.buyer_uuid).Where(ii => ii.id == shopItem.id && ii.buyer_uuid == null).ExecuteCommand();
            if (optCnt != 1)
            {
                dbh.Db.RollbackTran();
                ResObj["messgae"] = "购买失败！";
                return ResObj;
            }
            optCnt = dbh.Db.Updateable<F2.user>().ReSetValue(ii => ii.shl == (ii.shl - shopItem.price)).UpdateColumns(ii => ii.shl).Where(ii => ii.id == account.id && ii.shl >= shopItem.price).ExecuteCommand();
            if (optCnt != 1)
            {
                dbh.Db.RollbackTran();
                ResObj["messgae"] = "购买失败！";
                return ResObj;
            }
            dbh.Db.CommitTran();
            account.shl -= shopItem.price;

            ResObj["data"] = new JObject();
            var a = new JObject();
            a["buyer_uuid"] = shopItem.buyer_uuid ?? "";
            a["created_at"] = shopItem.created_at.AsTimestamp();
            a["data"] = shopItem.data;
            a["id"] = shopItem.id;
            a["item_id"] = shopItem.item_id;
            a["item_name"] = shopItem.item_name ?? "";
            a["price"] = shopItem.price;
            a["updated_at"] = shopItem.updated_at.AsTimestamp();
            a["uuid"] = shopItem.uuid;

            ResObj["data"]["item"] = a;
            ResObj["data"]["ling"] = account.shl;

            ResObj["code"] = 0;
            ResObj["type"] = 21;
            return ResObj;
        }
        [HttpPost("api/v2/shops/buy_native_shop")]
        public JObject shops_buy_native_shop([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;

            int num = value["num"].ToString().AsInt();
            F2.user account = getUserFromCache();
            if (account.bdshl < num)
            {
                ResObj["messgae"] = "商会令不足！";
                return ResObj;
            }
            var dbh = DbContext.Get();
            dbh.Db.BeginTran();
            int optCnt = dbh.Db.Updateable<F2.user>().ReSetValue(ii => ii.bdshl == (ii.bdshl - num)).UpdateColumns(ii => ii.bdshl).Where(ii => ii.id == account.id && ii.shl >= num).ExecuteCommand();
            if (optCnt != 1)
            {
                dbh.Db.RollbackTran();
                ResObj["messgae"] = "购买失败！";
                return ResObj;
            }
            dbh.Db.CommitTran();
            ResObj["data"] = new JObject();
            ResObj["data"]["ling"] = account.shl;
            ResObj["data"]["binding_ling"] = account.bdshl;
            ResObj["code"] = 0;
            ResObj["type"] = 24;
            return ResObj;
        }
        [HttpPost("api/v2/shops/recaption_item")]
        public JObject sects_recaption_item([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;

            ResObj["data"] = new JArray();

            //ResObj["code"] = 0;
            //ResObj["type"] = 41;
            return ResObj;
        }
        [HttpPost("api/v2/sects/info")]
        public JObject sects_info([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            var dbh = DbContext.Get();
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.sectId == 0)
            {
                //查列表
                ResObj = sects_list(value);
                ResObj["type"] = 41;
                return ResObj;
            }
            else
            {
                F2.sects sect = dbh.GetEntityDB<F2.sects>().GetById(sectMember.id);
                if (sect == null)
                {
                    sectMember.sectId = 0;
                    dbh.Db.Updateable(sectMember).UpdateColumns(ii=>ii.sectId).ExecuteCommand();

                    //查列表
                    ResObj = sects_list(value);
                    ResObj["type"] = 41;
                    return ResObj;
                }
                //宗门信息
                var sectInfo = new JObject();
                sectInfo["advanced_smelt_count"] = sectMember.smelt_count;
                sectInfo["boss_level"] = sect.boss_level;
                sectInfo["contribution"] = sect.contribution;
                sectInfo["donate"] = sect.donate;
                sectInfo["is_creator"] = sect.creator_id == account.id ? 1 : 0;
                sectInfo["join_time"] = sectMember.join_time.AsTimestamp();
                sectInfo["leader_name"] = sect.leader_name;
                sectInfo["max_count"] = sect.max_count;
                sectInfo["member_count"] = sect.member_count;
                sectInfo["new_message_id"] = sectMember.new_message_id;
                sectInfo["position_level"] = sectMember.position_level;
                sectInfo["sect_coin"] = sectMember.sect_coin;
                sectInfo["smelt_count"] = sectMember.smelt_count;
                sectInfo["today_boss_killed"] = sect.boss_HP <= 0;
                sectInfo["sect_info"] = new JObject(
                    new JProperty("HYJF", sectMember.HYJF),
                    new JProperty("playerlv", sectMember.playerlv),
                    new JProperty("playerName", sectMember.playerName),
                    new JProperty("uuid", sectMember.playerUuid));
                sectInfo["sect"] = new JObject(
                   new JProperty("beast_level", sect.beast_level),
                   new JProperty("boss_level", sect.boss_level),
                   new JProperty("capital", sect.capital),
                   new JProperty("created_at", sect.created_at),
                   new JProperty("creator_id", sect.creator_id),
                   new JProperty("dange_level", sect.dange_level),
                   new JProperty("danqi", sect.danqi),
                   new JProperty("id", sect.id),
                   new JProperty("last_login_time", sectMember.last_login_time),
                   new JProperty("level", sect.level),
                   new JProperty("library_level", sect.library_level),
                   new JProperty("message_board_config", sectMember.message_board_config),
                   new JProperty("name", sect.name),
                   new JProperty("smelt_level", sect.smelt_level),
                   new JProperty("updated_at", sect.updated_at),
                   new JProperty("uuid", sect.uuid)
                   );
            }
            ResObj["code"] = 0;
            ResObj["type"] = 41;
            return ResObj;
        }
        [HttpPost("api/v2/sects/list")]
        public JObject sects_list([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            int page = value["page"].AsInt();
            int per = value["per"].AsInt(30);
            if (page < 1)
            {
                page = 1;
            }
            var dbh = DbContext.Get();
            List<F2.sects> sects = dbh.Db.Queryable<F2.sects>().OrderBy(ii=>ii.id, SqlSugar.OrderByType.Desc).ToPageList(page, per);
            var list = new JArray();            
            foreach (var item in sects)
            {
                var a = new JObject();
                a["beast_level"] = item.beast_level;
                a["boss_level"] = item.boss_level;
                a["capital"] = item.capital;
                a["dange_level"] = item.dange_level;
                a["danqi"] = item.danqi;
                a["id"] = item.id;
                a["leader_name"] = item.leader_name;
                a["level"] = item.level;
                a["library_level"] = item.library_level;
                a["max_count"] = item.max_count;
                a["member_count"] = item.member_count;
                a["name"] = item.name;
                a["uuid"] = item.uuid;
                list.Add(a);
            }
            ResObj["data"] = list;
            ResObj["code"] = 0;
            ResObj["type"] = 29;
            return ResObj;
        }
        [HttpPost("api/v2/sects")]
        public JObject sects_create([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            string name = value["name"].ToString().Trim();
            var dbh = DbContext.Get();
            if (dbh.GetEntityDB<F2.sects>().Count(ii => ii.name == name) > 0)
            {
                ResObj["messgae"] = "宗门名字已经存在!";
                return ResObj;
            }
            F2.user account = getUserFromCache();

            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.sectId > 0)
            {
                ResObj["messgae"] = "玩家已加入其他门派";
                return ResObj;
            }
            F2.sects sect = new F2.sects();
            sect.uuid = Guid.NewGuid().ToString();
            sect.name = name;
            sect.creator_id = account.id;
            sect.leader_uuid = account.uuid;
            sect.name = account.username;//((JObject)JsonConvert.DeserializeObject(account.player_data))["playerDict"]?["playName"]?.ToString()??account.username;
            sect.max_count = 31;
            sect.member_count = 1;
            sect.boss_level = 1;
            dbh.Db.Insertable(sect).ExecuteReturnIdentity();

            sectMember.join_time = DateTime.Now;
            sectMember.last_login_time = DateTime.Now;
            sectMember.position_level = 0;
            sectMember.sectId = sect.id;
            dbh.Db.Updateable(sectMember).ExecuteCommand();

            ResObj["data"] = new JObject();
            ResObj["data"]["sect"] = new JObject(
                new JProperty("level", 0), 
                new JProperty("name", name),
                new JProperty("uuid", sect.uuid)
            );
            ResObj["data"]["user_sect"] = new JObject(
                new JProperty("position_level", 0), 
                new JProperty("sect_id", sect.id), 
                new JProperty("sect_uuid", sect.uuid),
                new JProperty("user_id", account.id),
                new JProperty("user_uuid", account.uuid)
            );
            ResObj["code"] = 0;
            ResObj["type"] = 28;
            return ResObj;
        }
    }
}