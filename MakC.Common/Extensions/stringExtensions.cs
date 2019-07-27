using System;
using System.Collections.Generic;
using System.Text;

namespace MakC.Common
{
    public static class stringExtensions
    {
        public static string GetQueryStringValue(this string thisValue,string key)
        {
            int eqIdx = thisValue.IndexOf(key);
            if (eqIdx >= 0)
            {
                eqIdx += key.Length + 1;
                int andIdx = thisValue.IndexOf("&", eqIdx);
                if (andIdx >= 0)
                {
                    return thisValue.Substring(eqIdx, andIdx - eqIdx);
                }
                else
                    return thisValue.Substring(eqIdx);
            }
            return "";
        }
        public static int AsInt(this string thisValue, int defval = 0)
        {
            int tmpint;
            if (int.TryParse(thisValue,out tmpint))
            {
                return tmpint;
            }
            return defval;
        }
        private static HashSet<string> booltrueTable = new HashSet<string>() {
            "1","true","yes","on"
        };
        public static bool Asbool(this string thisValue)
        {
            return !string.IsNullOrEmpty(thisValue) && booltrueTable.Contains(thisValue.ToLower());
        }
    }
}
