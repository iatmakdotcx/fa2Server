using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MakC.Common
{
    public static class JTokenExtensions
    {
        public static long AsLong(this JToken thisValue, long defValue = 0)
        {
            long tmpInt;
            if (thisValue != null && thisValue.Type != JTokenType.Null && long.TryParse(thisValue.ToString(), out tmpInt))
            {
                return tmpInt;
            }
            return defValue;
        }
        public static int AsInt(this JToken thisValue, int defValue = 0)
        {
            int tmpInt;
            if (thisValue != null && thisValue.Type != JTokenType.Null && int.TryParse(thisValue.ToString(), out tmpInt))
            {
                return tmpInt;
            }
            return defValue;
        }
    }
}
