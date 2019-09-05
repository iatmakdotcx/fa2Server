using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fa2Server.Models
{
    public class allUserLst
    {
        public List<allUserLstItem> list;        
    }
    public class allUserLstItem
    {
        public int id;
        public string uuid;
        public string username;
        public string playerName;
        public bool isAndroid;
        public bool isBan;
    }
}
