using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MakC.Common
{
    public static class JTokenExtensions
    {
        public static int AsInt(this JToken thisValue,int defValue = 0)
        {
            int tmpInt;
            if(thisValue.Type != JTokenType.Null && int.TryParse(thisValue.ToString(),out tmpInt))
            {
                return tmpInt;
            }
            return defValue;
        }
    }
}
