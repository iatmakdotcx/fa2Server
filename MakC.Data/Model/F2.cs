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
            public int jiaTa { get; set; }
            public int ShenMu { get; set; }
            public string cheatMsg { get; set; }
            public bool isBan { get; set; }
            public string ClientCheatMsg { get; set; }
            public int shlUsed { get; set; }
            public DateTime lastGetShlTime { get; set; }
            public int fastAck { get; set; }
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
            /// <summary>
            /// 秘境状态，0未开启。1报名中，2开战中，3结算中
            /// </summary>
            public int mi_jing_state { get; set; }

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
            public long remain_dimension_boss_hp { get; set; }
            public int remain_dimension_boss_Level { get; set; }
            public string remain_dimension_boss_skill { get; set; }
            public int mi_jing_point { get; set; }
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
            public int AckDimBossCnt { get; set; }
            public int CanAckDimBossCnt { get; set; }
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

        [SugarTable("sectdimensiondamage")]
        public class sectDimensionDamage
        {
            public sectDimensionDamage()
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
        [SugarTable("sectdimensionbossaward")]
        public class sectDimensionBossAward
        {
            public sectDimensionBossAward()
            {
            }

            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int id { get; set; }
            public int bossLevel { get; set; }
            public int playerId { get; set; }
            public int sect_coin { get; set; }
        }
        [SugarTable("mi_jing")]
        public class mi_jing
        {
            public mi_jing()
            {
            }

            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int id { get; set; }
            public int playerId { get; set; }
            public bool isbm { get; set; }
            public int leftnum { get; set; }
            public int point { get; set; }
            public string JJCRoles { get; set; }
            public string zfDict { get; set; }
            public int FirstRoleID { get; set; }            
            public string enemyData { get; set; }
            public bool isRobot { get; set; }
            public string playerName { get; set; }
            public int playerLv { get; set; }
            public int playerHYJF { get; set; }
            public string playerUuid { get; set; }
            public string Logs { get; set; }
            public int sect_id { get; set; }
            public bool reward_sect { get; set; }
            public bool reward_person { get; set; }
            public bool isAndroid { get; set; }
        }
        [SugarTable("mi_jing_rewards")]
        public class mi_jing_rewards
        {
            public mi_jing_rewards()
            {
            }

            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int id { get; set; }
            public int ranking { get; set; }
            public int end_ranking { get; set; }
            public int sect_coin { get; set; }
            public int shl { get; set; }
            public string content { get; set; }
            public bool isSect { get; set; }
        }
    }

}
