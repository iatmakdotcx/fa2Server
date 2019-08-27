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
            MemoryCacheService.Default.RemoveCache(DateTime.Now.ToString("yyyyMMdd") + "_1");
            MemoryCacheService.Default.RemoveCache(DateTime.Now.ToString("yyyyMMdd") + "_2");
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
                        tmpData = new JObject(new JProperty("TEXT", "兑换活动"), new JProperty("DHARR", DHARR));
                        MemoryCacheService.Default.SetCache(key, tmpData, 24*60);
                    }
                }
            }
            return tmpData;
        }
    }
}
