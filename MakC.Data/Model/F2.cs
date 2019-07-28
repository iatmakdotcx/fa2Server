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
           
        }
    }




}
