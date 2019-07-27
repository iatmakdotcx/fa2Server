using System;
using System.Collections.Generic;
using System.Text;

namespace MakC.Common
{
    public static class DateTimeExtensions
    {
        public static long AsTimestamp(this DateTime thisValue)
        {
            return (thisValue.AddHours(8).ToUniversalTime().Ticks - 621355968000000000) / 10000000 ;
        }
    }
}
