using MakC.Data;
using MakC.Data.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fa2Server
{
    public class CacheHelper
    {
        public static void clearCache()
        {
            MemoryCacheService.Default.RemoveCache("db_f2_setting_1");
            MemoryCacheService.Default.RemoveCache("db_f2_setting_2");
            MemoryCacheService.Default.RemoveCache("mi_jing_reward");
            MemoryCacheService.Default.RemoveCache("mi_jing_reward_info");
            MemoryCacheService.Default.RemoveCache("sect_shop_cfg");
            MemoryCacheService.Default.RemoveCache(DateTime.Now.ToString("yyyyMMdd") + "_1");
            MemoryCacheService.Default.RemoveCache(DateTime.Now.ToString("yyyyMMdd") + "_2");
            Setsect_shop(1);
        }

        public static F2.setting GetDBSetting(bool isAndroid)
        {
            var setting = MemoryCacheService.Default.GetCache<F2.setting>("db_f2_setting_" + (isAndroid ? "2" : "1"));
            if (setting == null)
            {
                lock (MemoryCacheService.Default)
                {
                    setting = MemoryCacheService.Default.GetCache<F2.setting>("db_f2_setting_" + (isAndroid ? "2" : "1"));
                    if (setting == null)
                    {
                        setting = DbContext.Get().GetEntityDB<F2.setting>().GetSingle(ii => ii.id == (isAndroid ? 2 : 1));
                        if (setting != null)
                        {
                            MemoryCacheService.Default.SetCache("db_f2_setting_" + (isAndroid ? "2" : "1"), setting, 5);
                        }
                        else
                            setting = new F2.setting();
                    }
                }
            }
            return setting;
        }

        public static List<F2.mi_jing_rewards> Getmi_jing_reward()
        {
            List<F2.mi_jing_rewards> tmpData = MemoryCacheService.Default.GetCache<List<F2.mi_jing_rewards>>("mi_jing_reward");
            if (tmpData == null)
            {
                lock (MemoryCacheService.Default)
                {
                    tmpData = MemoryCacheService.Default.GetCache<List<F2.mi_jing_rewards>>("mi_jing_reward");
                    if (tmpData == null)
                    {
                        tmpData = DbContext.Get().GetEntityDB<F2.mi_jing_rewards>().GetList();
                        if (tmpData != null)
                        {
                            MemoryCacheService.Default.SetCache("mi_jing_reward", tmpData, 5);
                        }
                    }
                }
            }
            return tmpData;
        }
        public static JObject Get_Mi_jing_reward_info()
        {
            JObject tmpData = MemoryCacheService.Default.GetCache<JObject>("mi_jing_reward_info");
            if (tmpData == null)
            {
                lock (MemoryCacheService.Default)
                {
                    tmpData = MemoryCacheService.Default.GetCache<JObject>("mi_jing_reward_info");
                    if (tmpData == null)
                    {
                        var vjvj = Getmi_jing_reward();
                        if (vjvj != null && vjvj.Count > 0)
                        {
                            JArray personal_rewards = new JArray();
                            JArray sect_rewards = new JArray();
                            tmpData = new JObject();
                            tmpData["personal_rewards"] = personal_rewards;
                            tmpData["sect_rewards"] = sect_rewards;
                            foreach (var item in vjvj)
                            {
                                if (item.isSect)
                                {
                                    sect_rewards.Add(new JObject(
                                        new JProperty("end_ranking", item.end_ranking),
                                        new JProperty("ranking", item.ranking),
                                        new JProperty("sect_coin", item.sect_coin)
                                    ));
                                }
                                else
                                {
                                    personal_rewards.Add(new JObject(
                                        new JProperty("id", item.id),
                                        new JProperty("content", item.content),
                                        new JProperty("end_ranking", item.end_ranking),
                                        new JProperty("ranking", item.ranking),
                                        new JProperty("sect_coin", item.sect_coin)
                                    ));
                                }
                            }
                            MemoryCacheService.Default.SetCache("mi_jing_reward_info", tmpData, 5);
                        }
                    }
                }
            }
            return tmpData;
        }
        private static Dictionary<int, int> skillPrice = new Dictionary<int, int>() {
            { 320,300 },
            { 323,500 },
            { 324,500 },
            { 325,500 },
            { 326,500 },
            { 327,500 },
            { 282,500 },
            { 317,500 },
            { 276,500 },
            { 285,500 },
            { 334,500 },
        };
        private static List<int> resvSkill = new List<int>()
        {
            291,346,294,71
        };
        public static JObject GetExchangeData(bool isAndroid)
        {
            var tmpisa = (isAndroid ? 2 : 1);
            string key = DateTime.Now.ToString("yyyyMMdd")+"_"+ tmpisa;

            JObject tmpData = MemoryCacheService.Default.GetCache<JObject>(key);
            if (tmpData == null)
            {
                lock (MemoryCacheService.Default)
                {
                    tmpData = MemoryCacheService.Default.GetCache<JObject>(key);
                    if (tmpData == null)
                    {
                        var w = "w" + ((int)(DateTime.Now.DayOfWeek)).ToString();
                        var m = "m" + DateTime.Now.Day.ToString();

                        var datas = DbContext.Get().Db.Queryable<F2.setting_hdzx>()
                            .Where(ii=> 
                                (ii.day.Contains("-") || ii.day.Contains(w) || ii.day.Contains(m)) &&
                                (ii.platform==0 || ii.platform== tmpisa)
                             ).ToList();
                        JArray DHARR = new JArray();
                        foreach (var item in datas)
                        {
                            DHARR.Add(new JObject(
                                new JProperty("USE", new JObject(
                                    new JProperty("itemType", item.f_itemType),
                                    new JProperty("childType", item.f_childType),
                                    new JProperty("num", item.f_count)
                                    )),
                                new JProperty("GET", new JObject(
                                    new JProperty("itemType", item.t_itemType),
                                    new JProperty("childType", item.t_childType),
                                    new JProperty("num", item.t_count)
                                    ))
                             ));
                        }
                        if (w == "w6")
                        {
                            //周六随机30个技能
                            Dictionary<int,int> randSkills = new Dictionary<int, int>();
                            var r = new Random();
                            while (randSkills.Count <= 30)
                            {
                                int ui = r.Next(1, 357);
                                if (!randSkills.ContainsKey(ui) && !resvSkill.Contains(ui))
                                {
                                    int price = skillPrice.GetValueOrDefault(ui, 100);
                                    randSkills.Add(ui,price);
                                }
                            }
                            foreach (var item in randSkills)
                            {
                                DHARR.Add(new JObject(
                                new JProperty("USE", new JObject(
                                    new JProperty("itemType", "8"),
                                    new JProperty("childType", "96"),
                                    new JProperty("num", item.Value)
                                    )),
                                new JProperty("GET", new JObject(
                                    new JProperty("itemType", "9"),
                                    new JProperty("childType", item.Key),
                                    new JProperty("num", 1)
                                    ))
                             ));

                            }



                        }
                        tmpData = new JObject(new JProperty("TEXT", "兑换活动"), new JProperty("DHARR", DHARR));
                        MemoryCacheService.Default.SetCache(key, tmpData, 24*60);
                    }
                }
            }
            return tmpData;
        }
        public static List<F2.sect_shop_cfg> Getsect_shop_cfg()
        {
            List<F2.sect_shop_cfg> tmpData = MemoryCacheService.Default.GetCache<List<F2.sect_shop_cfg>>("sect_shop_cfg");
            if (tmpData == null)
            {
                lock (MemoryCacheService.Default)
                {
                    tmpData = MemoryCacheService.Default.GetCache<List<F2.sect_shop_cfg>>("sect_shop_cfg");
                    if (tmpData == null)
                    {
                        tmpData = DbContext.Get().GetEntityDB<F2.sect_shop_cfg>().GetList(ii => ii.threshold >= 0);
                        if (tmpData != null)
                        {
                            foreach (var item in tmpData)
                            {
                                if (item.type == 1)
                                {
                                    List<int> li = new List<int>();
                                    foreach (var cccItem in item.ccc.ToString().Split(","))
                                    {
                                        li.Add(int.Parse(cccItem, 0));
                                    }
                                    item.ccc = li.ToArray();
                                }
                                else if (item.type == 2 || item.type == 3)
                                {
                                    List<F2.sect_shop_cfg.cccItem> cccs = new List<F2.sect_shop_cfg.cccItem>();
                                    foreach (var cccItem in item.ccc.ToString().Split(","))
                                    {
                                        var ss = cccItem.Split(":");
                                        cccs.Add(new F2.sect_shop_cfg.cccItem(int.Parse(ss[0], 0), int.Parse(ss[1], 0)));
                                    }
                                    item.ccc = cccs;
                                }
                            }
                            MemoryCacheService.Default.SetCache("sect_shop_cfg", tmpData, 24*60);
                        }
                    }
                }
            }
            return tmpData;
        }
        public static List<F2.sect_shop> Getsect_shop(int sect_id)
        {
            List<F2.sect_shop> tmpData = MemoryCacheService.Default.GetCache<List<F2.sect_shop>>("sect_shop_"+sect_id);
            return tmpData;
        }
        public static void Setsect_shop(int sect_id)
        {
            MemoryCacheService.Default.RemoveCache("sect_shop_" + sect_id);
            List<F2.sect_shop_cfg> shopcfg = Getsect_shop_cfg();
            List<F2.sect_shop> tmpData = new List<F2.sect_shop>();
            if (shopcfg != null && shopcfg.Count > 0)
            {
                var r = new Random();
                while (true)
                {
                    var acfg = shopcfg[r.Next(0, shopcfg.Count)];
                    if (r.Next(0, 101) <= acfg.threshold)
                    {
                        var si = new F2.sect_shop(acfg);
                        if (acfg.type == 3)
                        {
                            //辣鸡装备
                            si.RandItem = new F2.sect_shop_cfg();
                            var lccc = (List<F2.sect_shop_cfg.cccItem>)acfg.ccc;
                            var triri = lccc[r.Next(0, lccc.Count)];
                            si.RandItem.itemType = triri.itemType;
                            si.RandItem.childType = triri.childType;
                            si.RandItem.num = acfg.num;
                            si.RandItem.price = acfg.price;
                        }
                        tmpData.Add(si);
                        if (tmpData.Count >= 12)
                        {
                            MemoryCacheService.Default.SetCache("sect_shop_" + sect_id, tmpData, 24 * 60);
                            break;
                        }
                    }
                }
            }
        }

        public static void SetSect_BossCd(int sect_id,int timeout = 5)
        {
            MemoryCacheService.Default.SetCache("sect_bosscd_" + sect_id, DateTime.Now.AddMinutes(timeout).ToString(), timeout);
        }
        public static int GetSect_BossCd(int sect_id)
        {
            string ts = MemoryCacheService.Default.GetCache<string>("sect_bosscd_" + sect_id);
            DateTime dts;
            int its = 0;
            if (!string.IsNullOrEmpty(ts) && DateTime.TryParse(ts, out dts) && (its = (int)((dts - DateTime.Now).TotalSeconds)) > 0)
            {
                return its;
            }
            return 0;            
        }

    }
}
