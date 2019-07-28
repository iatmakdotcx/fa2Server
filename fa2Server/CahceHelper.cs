using MakC.Data;
using MakC.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fa2Server
{
    public class CahceHelper
    {
        public static F2.setting GetDBSetting(bool isAndroid)
        {
            var setting = MemoryCacheService.Default.GetCache<F2.setting>("db_f2_setting_" + (isAndroid ? "2" : "1"));
            if (setting==null)
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
    }
}
