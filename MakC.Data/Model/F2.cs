using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MakC.Data.Model
{
    public class F2
    {
        [SugarTable("users")]
        public class user
        {
            public user()
            {
            }

            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int id { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public int net_id { get; set; }
            public string uuid { get; set; }
            public string token { get; set; }
            public int lastDCTime { get; set; }
            public int shl { get; set; }
            public int bdshl { get; set; }
            public string userdata { get; set; }
            public string player_data { get; set; }
            public string player_zhong_yao { get; set; }
            public DateTime lastLoginTime { get; set; }
            public bool isAndroid { get; set; }
        }

        [SugarTable("setting")]
        public class setting
        {
            public setting()
            {
            }

            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int id { get; set; }
            public string CHONGZHI { get; set; }
            public string CHONGZHI2 { get; set; }
            public string CURVESION { get; set; }
            public int DAQUNUM { get; set; }
            public int DBLOWLV { get; set; }
            public string GIFT { get; set; }
            public string GONGGAO { get; set; }
            public string HDZX { get; set; }
            public string HUODONG { get; set; }
            public string INREVIEW { get; set; }
            public int LOWLEVEL { get; set; }
            public int LOWLEVELEXP { get; set; }
            public int RATE { get; set; }
            public string SHOUCHONG { get; set; }
            public int TENLOWLV { get; set; }
            public int THLOWLV { get; set; }

            public string base_playerData { get; set; }
            public string base_playerzhongyao { get; set; }
        }
        [SugarTable("shops")]
        public class shop
        {
            public shop()
            {
            }

            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int id { get; set; }
            public string buyer_uuid { get; set; }
            public DateTime created_at { get; set; }
            public string data { get; set; }
            public string item_id { get; set; }
            public string item_name { get; set; }
            public int price { get; set; }
            public DateTime updated_at { get; set; }
            public string uuid { get; set; }
            public int sort { get; set; }
            public string giftcode { get; set; }

        }
        [SugarTable("sects")]
        public class sects
        {
            public sects()
            {
            }

            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int id { get; set; }
            public string uuid { get; set; }
            public string name { get; set; }
            public string leader_name { get; set; }
            public string leader_uuid { get; set; }
            public int creator_id { get; set; }
            public DateTime created_at { get; set; }
            public int beast_level { get; set; }
            public int boss_level { get; set; }
            public int capital { get; set; }
            public int dange_level { get; set; }
            public int danqi { get; set; }
            public int level { get; set; }
            public int library_level { get; set; }
            public int max_count { get; set; }
            public int member_count { get; set; }
            public int smelt_level { get; set; }
            public DateTime updated_at { get; set; }
            public int donate { get; set; }
            public int contribution { get; set; }
            public long boss_HP { get; set; }
        }
        [SugarTable("sect_member")]
        public class sect_member
        {
            public sect_member()
            {
            }

            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int id { get; set; }
            public int sectId { get; set; }
            public int playerId { get; set; }
            public string playerUuid { get; set; }
            public string playerName { get; set; }
            public int playerlv { get; set; }
            public int HYJF { get; set; }
            public DateTime last_login_time { get; set; }
            public int smelt_count { get; set; }
            public int sect_coin { get; set; }
            public DateTime join_time { get; set; }
            public int new_message_id { get; set; }
            public int position_level { get; set; }
            public string message_board_config { get; set; }
            public int contribution { get; set; }
            public int danqi { get; set; }
            public int awardCnt { get; set; }
            public int donateCnt { get; set; }
            public int AttackBossCnt { get; set; }
            public int CanAttackBossCnt { get; set; }
        }
        [SugarTable("sect_joinRequest")]
        public class sect_joinRequest
        {
            public sect_joinRequest()
            {
            }

            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int id { get; set; }
            public int sectId { get; set; }
            public int playerId { get; set; }
            public string playerUuid { get; set; }
            public string playerName { get; set; }
            public int playerlv { get; set; }
            public int HYJF { get; set; }
            public DateTime create_at { get; set; }
        }
        [SugarTable("sectMessage")]
        public class sectMessage
        {
            public sectMessage()
            {
            }

            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int id { get; set; }
            public DateTime create_at { get; set; }
            public int sectId { get; set; }            
            public string playerName { get; set; }
            public string playerUuid { get; set; }
            public string content { get; set; }
        }
        [SugarTable("sectBossAward")]
        public class sectBossAward
        {
            public sectBossAward()
            {
            }

            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int id { get; set; }
            public int bossLevel { get; set; }
            public int playerId { get; set; }
        }
        [SugarTable("sectBossDamage")]
        public class sectBossDamage
        {
            public sectBossDamage()
            {
            }

            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int id { get; set; }
            public int sectid { get; set; }
            public long damage { get; set; }
            public string playerName { get; set; }
            public int position_level { get; set; }
            public string playerUuid { get; set; }
            public int playerId { get; set; }
        }
        [SugarTable("giftCode")]
        public class giftCode
        {
            public giftCode()
            {
            }

            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int id { get; set; }
            public int code { get; set; }
            public string uuid { get; set; }
            public string itemData { get; set; }
            public DateTime create_at { get; set; }
        }
    }

}
