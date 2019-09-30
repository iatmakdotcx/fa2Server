using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using MakC.Common;
using MakC.Data;
using MakC.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace fa2Server.Controllers
{
    
    [ApiController]
    public class GameServerController : ControllerBase
    {
        private ILog log = LogManager.GetLogger(Startup.Repository.Name, "GameServerController");
        long[] EXPLEVEL = new long[] { 0, 100, 300, 600, 1000, 1700, 3000, 5000, 8000, 12000, 17000, 25000, 35000, 50000, 70000, 100000, 150000, 220000, 320000, 450000, 600000, 800000, 1010000, 1220000, 1470000, 1730000, 2000000, 2280000, 2570000, 2870000, 3180000, 3500000, 4000000, 4600000, 5300000, 6100000, 7000000, 8000000, 9000000, 10000000, 12000000, 14000000, 16000000, 18000000, 20000000, 22000000, 24000000, 26000000, 28000000, 30000000, 31400000, 32800000, 34200000, 35600000, 37000000, 38400000, 39800000, 41200000, 42600000, 44000000, 45400000, 46800000, 48200000, 49600000, 51000000, 52400000, 53800000, 55200000, 56600000, 58000000, 59400000, 60800000, 62200000, 63600000, 65000000, 66400000, 67800000, 69200000, 70600000, 72000000, 73400000, 74800000, 76200000, 77600000, 79000000, 80400000, 81800000, 83200000, 84600000, 86000000, 87400000, 88800000, 90200000, 91600000, 93000000, 94400000, 95800000, 97200000, 98600000, 100000000, 124081000, 148162000, 172243000, 196324000, 220405000, 244486000, 268567000, 292648000, 316729000, 340810000, 364891000, 388972000, 413053000, 437134000, 461215000, 485296000, 509377000, 533458000, 557539000, 581620000, 605701000, 629782000, 653863000, 677944000, 702025000, 726106000, 750187000, 774268000, 798349000, 822430000, 846511000, 870592000, 894673000, 918754000, 942835000, 966916000, 990997000, 1015078000, 1039159000, 1063240000, 1087321000, 1111402000, 1135483000, 1159564000, 1183645000, 1207726000, 1231807000, 1255888000, 1279969000, 1304050000, 1328131000, 1352212000, 1376293000, 1400374000, 1424455000, 1448536000, 1472617000, 1496698000, 1520779000, 1544860000, 1568941000, 1593022000, 1617103000, 1641184000, 1665265000, 1689346000, 1713427000, 1737508000, 1761589000, 1785670000, 1809751000, 1833832000, 1857913000, 1881994000, 1906075000, 1930156000, 1954237000, 1978318000, 2002399000, 2026480000, 2050561000, 2074642000, 2098723000, 2122804000, 2146885000, 2170966000, 2195047000, 2219128000, 2243209000, 2267290000, 2291371000, 2315452000, 2339533000, 2363614000, 2387695000, 2411776000, 2435857000, 2459938000, 2484019000, 2508100000, 2541900000, 2575700000, 2609500000, 2643300000, 2677100000, 2710900000, 2744700000, 2778500000, 2812300000, 2846100000, 2879900000, 2913700000, 2947500000, 2981300000, 3015100000, 3048900000, 3082700000, 3116500000, 3150300000, 3184100000, 3217900000, 3251700000, 3285500000, 3319300000, 3353100000, 3386900000, 3420700000, 3454500000, 3488300000, 3522100000, 3555900000, 3589700000, 3623500000, 3657300000, 3691100000, 3724900000, 3758700000, 3792500000, 3826300000, 3860100000, 3893900000, 3927700000, 3961500000, 3995300000, 4029100000, 4062900000, 4096700000, 4130500000, 4164300000, 4198100000, 4231900000, 4265700000, 4299500000, 4333300000, 4367100000, 4400900000, 4434700000, 4468500000, 4502300000, 4536100000, 4569900000, 4603700000, 4637500000, 4671300000, 4705100000, 4738900000, 4772700000, 4806500000, 4840300000, 4874100000, 4907900000, 4941700000, 4975500000, 5009300000, 5043100000, 5076900000, 5110700000, 5144500000, 5178300000, 5212100000, 5245900000, 5279700000, 5313500000, 5347300000, 5381100000, 5414900000, 5448700000, 5482500000, 5516300000, 5550100000, 5583900000, 5617700000, 5651500000, 5685300000, 5719100000, 5752900000, 5786700000, 5820500000, 5854300000, 5888100000, 5942700000, 5997300000, 6051900000, 6106500000, 6161100000, 6215700000, 6270300000, 6324900000, 6379500000, 6434100000, 6488700000, 6543300000, 6597900000, 6652500000, 6707100000, 6761700000, 6816300000, 6870900000, 6925500000, 6980100000, 7034700000, 7089300000, 7143900000, 7198500000, 7253100000, 7307700000, 7362300000, 7416900000, 7471500000, 7526100000, 7580700000, 7635300000, 7689900000, 7744500000, 7799100000, 7853700000, 7908300000, 7962900000, 8017500000, 8072100000, 8126700000, 8181300000, 8235900000, 8290500000, 8345100000, 8399700000, 8454300000, 8508900000, 8563500000, 8618100000, 8672700000, 8727300000, 8781900000, 8836500000, 8891100000, 8945700000, 9000300000, 9054900000, 9109500000, 9164100000, 9218700000, 9273300000, 9327900000, 9382500000, 9437100000, 9491700000, 9546300000, 9600900000, 9655500000, 9710100000, 9764700000, 9819300000, 9873900000, 9928500000, 9983100000, 10037700000, 10092300000, 10146900000, 10201500000, 10256100000, 10310700000, 10365300000, 10419900000, 10474500000, 10529100000, 10583700000, 10638300000, 10692900000, 10747500000, 10802100000, 10856700000, 10911300000, 10965900000, 11020500000, 11075100000, 11129700000, 11184300000, 11238900000, 11293500000, 11348100000, 11426100000, 11504100000, 11582100000, 11660100000, 11738100000, 11816100000, 11894100000, 11972100000, 12050100000, 12128100000, 12206100000, 12284100000, 12362100000, 12440100000, 12518100000, 12596100000, 12674100000, 12752100000, 12830100000, 12908100000, 12986100000, 13064100000, 13142100000, 13220100000, 13298100000, 13376100000, 13454100000, 13532100000, 13610100000, 13688100000, 13766100000, 13844100000, 13922100000, 14000100000, 14078100000, 14156100000, 14234100000, 14312100000, 14390100000, 14468100000, 14546100000, 14624100000, 14702100000, 14780100000, 14858100000, 14936100000, 15014100000, 15092100000, 15170100000, 15248100000, 15326100000, 15404100000, 15482100000, 15560100000, 15638100000, 15716100000, 15794100000, 15872100000, 15950100000, 16028100000, 16106100000, 16184100000, 16262100000, 16340100000, 16418100000, 16496100000, 16574100000, 16652100000, 16730100000, 16808100000, 16886100000, 16964100000, 17042100000, 17120100000, 17198100000, 17276100000, 17354100000, 17432100000, 17510100000, 17588100000, 17666100000, 17744100000, 17822100000, 17900100000, 17978100000, 18056100000, 18134100000, 18212100000, 18290100000, 18368100000, 18446100000, 18524100000, 18602100000, 18680100000, 18758100000, 18836100000, 18914100000, 18992100000, 19070100000, 19148100000, 19252100000, 19356100000, 19460100000, 19564100000, 19668100000, 19772100000, 19876100000, 19980100000, 20084100000, 20188100000, 20292100000, 20396100000, 20500100000, 20604100000, 20708100000, 20812100000, 20916100000, 21020100000, 21124100000, 21228100000, 21332100000, 21436100000, 21540100000, 21644100000, 21748100000, 21852100000, 21956100000, 22060100000, 22164100000, 22268100000, 22372100000, 22476100000, 22580100000, 22684100000, 22788100000, 22892100000, 22996100000, 23100100000, 23204100000, 23308100000, 23412100000, 23516100000, 23620100000, 23724100000, 23828100000, 23932100000, 24036100000, 24140100000, 24244100000, 24348100000, 24452100000, 24556100000, 24660100000, 24764100000, 24868100000, 24972100000, 25076100000, 25180100000, 25284100000, 25388100000, 25492100000, 25596100000, 25700100000, 25804100000, 25908100000, 26012100000, 26116100000, 26220100000, 26324100000, 26428100000, 26532100000, 26636100000, 26740100000, 26844100000, 26948100000, 27052100000, 27156100000, 27260100000, 27364100000, 27468100000, 27572100000, 27676100000, 27780100000, 27884100000, 27988100000, 28092100000, 28196100000, 28300100000, 28404100000, 28508100000, 28612100000, 28716100000, 28820100000, 28924100000, 29028100000, 29132100000, 29236100000, 29340100000, 29444100000, 29548100000, 29680700000, 29813300000, 29945900000, 30078500000, 30211100000, 30343700000, 30476300000, 30608900000, 30741500000, 30874100000, 31006700000, 31139300000, 31271900000, 31404500000, 31537100000, 31669700000, 31802300000, 31934900000, 32067500000, 32200100000, 32332700000, 32465300000, 32597900000, 32730500000, 32863100000, 32995700000, 33128300000, 33260900000, 33393500000, 33526100000, 33658700000, 33791300000, 33923900000, 34056500000, 34189100000, 34321700000, 34454300000, 34586900000, 34719500000, 34852100000, 34984700000, 35117300000, 35249900000, 35382500000, 35515100000, 35647700000, 35780300000, 35912900000, 36045500000, 36178100000, 36310700000, 36443300000, 36575900000, 36708500000, 36841100000, 36973700000, 37106300000, 37238900000, 37371500000, 37504100000, 37636700000, 37769300000, 37901900000, 38034500000, 38167100000, 38299700000, 38432300000, 38564900000, 38697500000, 38830100000, 38962700000, 39095300000, 39227900000, 39360500000, 39493100000, 39625700000, 39758300000, 39890900000, 40023500000, 40156100000, 40288700000, 40421300000, 40553900000, 40686500000, 40819100000, 40951700000, 41084300000, 41216900000, 41349500000, 41482100000, 41614700000, 41747300000, 41879900000, 42012500000, 42145100000, 42277700000, 42410300000, 42542900000, 42675500000, 42808100000, 42971900000, 43135700000, 43299500000, 43463300000, 43627100000, 43790900000, 43954700000, 44118500000, 44282300000, 44446100000, 44609900000, 44773700000, 44937500000, 45101300000, 45265100000, 45428900000, 45592700000, 45756500000, 45920300000, 46084100000, 46247900000, 46411700000, 46575500000, 46739300000, 46903100000, 47066900000, 47230700000, 47394500000, 47558300000, 47722100000, 47885900000, 48049700000, 48213500000, 48377300000, 48541100000, 48704900000, 48868700000, 49032500000, 49196300000, 49360100000, 49523900000, 49687700000, 49851500000, 50015300000, 50179100000, 50342900000, 50506700000, 50670500000, 50834300000, 50998100000, 51161900000, 51325700000, 51489500000, 51653300000, 51817100000, 51980900000, 52144700000, 52308500000, 52472300000, 52636100000, 52799900000, 52963700000, 53127500000, 53291300000, 53455100000, 53618900000, 53782700000, 53946500000, 54110300000, 54274100000, 54437900000, 54601700000, 54765500000, 54929300000, 55093100000, 55256900000, 55420700000, 55584500000, 55748300000, 55912100000, 56075900000, 56239700000, 56403500000, 56567300000, 56731100000, 56894900000, 57058700000, 57222500000, 57386300000, 57550100000, 57713900000, 57877700000, 58041500000, 58205300000, 58369100000, 58532900000, 58696700000, 58860500000, 59024300000, 59188100000, 59385700000, 59583300000, 59780900000, 59978500000, 60176100000, 60373700000, 60571300000, 60768900000, 60966500000, 61164100000, 61361700000, 61559300000, 61756900000, 61954500000, 62152100000, 62349700000, 62547300000, 62744900000, 62942500000, 63140100000, 63337700000, 63535300000, 63732900000, 63930500000, 64128100000, 64325700000, 64523300000, 64720900000, 64918500000, 65116100000, 65313700000, 65511300000, 65708900000, 65906500000, 66104100000, 66301700000, 66499300000, 66696900000, 66894500000, 67092100000, 67289700000, 67487300000, 67684900000, 67882500000, 68080100000, 68277700000, 68475300000, 68672900000, 68870500000, 69068100000, 69265700000, 69463300000, 69660900000, 69858500000, 70056100000, 70253700000, 70451300000, 70648900000, 70846500000, 71044100000, 71241700000, 71439300000, 71636900000, 71834500000, 72032100000, 72229700000, 72427300000, 72624900000, 72822500000, 73020100000, 73217700000, 73415300000, 73612900000, 73810500000, 74008100000, 74205700000, 74403300000, 74600900000, 74798500000, 74996100000, 75193700000, 75391300000, 75588900000, 75786500000, 75984100000, 76181700000, 76379300000, 76576900000, 76774500000, 76972100000, 77169700000, 77367300000, 77564900000, 77762500000, 77960100000, 78157700000, 78355300000, 78552900000, 78750500000, 78948100000, 79182100000, 79416100000, 79650100000, 79884100000, 80118100000, 80352100000, 80586100000, 80820100000, 81054100000, 81288100000, 81522100000, 81756100000, 81990100000, 82224100000, 82458100000, 82692100000, 82926100000, 83160100000, 83394100000, 83628100000, 83862100000, 84096100000, 84330100000, 84564100000, 84798100000, 85032100000, 85266100000, 85500100000, 85734100000, 85968100000, 86202100000, 86436100000, 86670100000, 86904100000, 87138100000, 87372100000, 87606100000, 87840100000, 88074100000, 88308100000, 88542100000, 88776100000, 89010100000, 89244100000, 89478100000, 89712100000, 89946100000, 90180100000, 90414100000, 90648100000, 90882100000, 91116100000, 91350100000, 91584100000, 91818100000, 92052100000, 92286100000, 92520100000, 92754100000, 92988100000, 93222100000, 93456100000, 93690100000, 93924100000, 94158100000, 94392100000, 94626100000, 94860100000, 95094100000, 95328100000, 95562100000, 95796100000, 96030100000, 96264100000, 96498100000, 96732100000, 96966100000, 97200100000, 97434100000, 97668100000, 97902100000, 98136100000, 98370100000, 98604100000, 98838100000, 99072100000, 99306100000, 99540100000, 99774100000, 100008100000, 100242100000, 100476100000, 100710100000, 100944100000, 101178100000, 101412100000, 101646100000, 101880100000, 102114100000, 102348100000 };
        long[] VIP_EXPLEVEL = new long[] { 0, 1500, 5000, 15000, 30000, 50000, 100000, 150000, 300000, 450000, 750000, 1200000, 1800000, 2800000, 4000000, 6500000, 10000000 };
        private static object locker = new object();
        private F2.user getUserFromCache()
        {
            return MemoryCacheService.Default.GetCache<F2.user>("account_" + HttpContext.TraceIdentifier) ?? throw new Exception("用户信息已过期？？？？？？？？");
        }
        private int GetExpLevel(long exp)
        {
            for (int i = EXPLEVEL.Length-1; i >= 0 ; i--)
            {
                if (exp > EXPLEVEL[i])
                {
                    return i + 1;
                }
            }
            return 0;
        }
        private int GetVIPLevel(int exp)
        {
            for (int i = VIP_EXPLEVEL.Length-1; i >= 0; i--)
            {
                if (exp > VIP_EXPLEVEL[i])
                {
                    return i + 1;
                }
            }
            return 0;
        }
        private F2.sect_member updateSectInfo(F2.user account)
        {
            var dbh = DbContext.Get();
            F2.sect_member sectMember = dbh.GetEntityDB<F2.sect_member>().GetSingle(ii => ii.playerId == account.id);
            if (sectMember == null)
            {
                sectMember = new F2.sect_member();
                sectMember.message_board_config = "{\"limit_count\":20,\"max_count\":30,\"min_count\":10,\"max_word_size\":30,\"min_word_size\":10,\"position_level\":7}";
            }

            var playerDict = ((JObject)JsonConvert.DeserializeObject(account.player_data))["playerDict"];
            
            sectMember.last_login_time = DateTime.Now;
            sectMember.playerName = playerDict["playName"]?.ToString() ?? account.username;
            sectMember.playerId = account.id;
            sectMember.playerUuid = account.uuid;
            sectMember.playerlv = GetExpLevel(playerDict["juntuanExp"].AsLong());
            sectMember.HYJF = GetVIPLevel(playerDict["hyJiFen"].AsInt());//会员等级
            sectMember.join_time = DateTime.Now;
            if (sectMember.id == 0)
            {
                sectMember.CanAttackBossCnt = 10;
                sectMember.CanAckDimBossCnt = 50;
                dbh.Db.Insertable(sectMember).ExecuteReturnIdentity();
            }
            else
            {
                dbh.Db.Updateable(sectMember).ExecuteCommand();
            }
            dbh.Db.Updateable<F2.sects>(new F2.sects() { leader_name = sectMember.playerName }).UpdateColumns(ii => ii.leader_name).Where(ii => ii.leader_uuid == sectMember.playerUuid).ExecuteCommand();
            return sectMember;
        }
        private JArray GenMi_jing_enemy(F2.mi_jing mi_Jing,int rankidx)
        {
            JArray ResObj = new JArray();
            var r = new Random();

            ResObj.Add(new JObject(
                new JProperty("level", 20),  //拔草固定20分
                new JProperty("playerDict", new JObject()),
                new JProperty("type", "0")
            ));
            ResObj.Add(new JObject(
                new JProperty("level", r.Next(100, 301)),
                new JProperty("playerDict", new JObject()),
                new JProperty("type", "1")
            ));
            F2.mi_jing enemyMi = null;
            if (rankingsData.Count == 0) ctl_reloadrankingsData();

            if (rankingsData.Count>0)
            {
                if (mi_Jing.point == 0)
                {
                    do
                    {
                        enemyMi = rankingsData[r.Next(0, rankingsData.Count)];
                    } while (enemyMi.isAndroid != mi_Jing.isAndroid); //只能匹配同平台的
                }
                else
                {
                    //匹配方式，过滤出所有相同平台的人，最近的6个随机
                    var oyl = rankingsData.Where(ii => ii.point > 0 && ii.isAndroid == mi_Jing.isAndroid).OrderByDescending(ii => ii.point).ToList();
                    List<F2.mi_jing> ml = new List<F2.mi_jing>();
                    for (int i = 0; i < oyl.Count(); i++)
                    {
                        var item = oyl[i];
                        if (item.id==mi_Jing.id)
                        {
                            var tmpix = i - 3;
                            if (tmpix < 0) tmpix = 0;
                            for (int j = tmpix; j < Math.Min(oyl.Count(),7+i); j++)
                            {
                                ml.Add(oyl[j]);
                            }
                            break;
                        }
                    }
                    if (ml.Count > 0)
                    {
                        enemyMi = ml[r.Next(0, ml.Count)];
                    }else enemyMi = mi_Jing;
                }
            }
            else
            {
                enemyMi = mi_Jing;
            }
            ResObj.Add(new JObject(
                new JProperty("level", r.Next(3000, 7001)),
                new JProperty("playerDict", new JObject(
                    new JProperty("FirstRoleID", enemyMi.FirstRoleID),
                    new JProperty("JJCRoles", enemyMi.JJCRoles),
                    new JProperty("playerlv", enemyMi.playerLv),
                    new JProperty("playerName", enemyMi.playerName),
                    new JProperty("id", enemyMi.id),
                    new JProperty("zfDict", enemyMi.zfDict)
                    )),
                new JProperty("jjcJF", mi_Jing.point),
                new JProperty("type", "2")
            ));
            mi_Jing.enemyData = ResObj.ToString(Formatting.None);
            DbContext.Get().Db.Updateable(mi_Jing).UpdateColumns(ii => ii.enemyData).ExecuteCommand();                
            return ResObj;
        }
        private int getsjCount(JObject playerData)
        {
            int sjsl = 0;
            foreach (var item in (JArray)playerData["playerDict"]["packageArr"])
            {

            }



             return 0;
        }
        private bool compareJsonObj(JToken data1, JToken data2)
        {
            var a = data1.ToString(Formatting.None).Replace("\"", "").ToCharArray();
            Array.Sort(a);
            var b = data2.ToString(Formatting.None).Replace("\"", "").ToCharArray();
            Array.Sort(b);
            return new string(a) == new string(b);
        }
        private void checkPackageItemNotEqual(JObject oldData, JObject newData,ref StringBuilder zbMsg)
        {
            List<GameItem> OldItem = new List<GameItem>();
            foreach (var tmpitem in (JArray)oldData["playerDict"]["packageArr"])
            {
                GameItem oi = new GameItem();
                oi.id = tmpitem["itemID"].AsInt();
                oi.itemType = tmpitem["itemType"].AsInt();
                oi.childType = tmpitem["childType"].AsInt();
                oi.num = tmpitem["num"].AsInt(1);
                OldItem.Add(oi);
            }
            foreach (var item in (JArray)newData["playerDict"]["packageArr"])
            {
                int id = item["itemID"].AsInt();
                int itemType = item["itemType"].AsInt();
                int childType = item["childType"].AsInt();
                int num = item["num"].AsInt(1);
                GameItem Oi = OldItem.Find(i => i.id == id);
                if (Oi == null)
                {
                    //增加的物品
                    if (childType > 40)
                    {
                        zbMsg.Append($"增加物品:{itemType},{childType},{num};");
                    }
                }else if (Oi.itemType != itemType || Oi.childType!=childType)
                {
                    //物品改变
                    if (childType > 40)
                    {
                        zbMsg.Append($"改变物品:{Oi.itemType},{Oi.childType},{Oi.num}->{itemType},{childType},{num};");
                    }
                }
                else if (Oi.num!= num)
                {
                    if (itemType == 8)
                    {
                        switch (childType)
                        {
                            case 91:
                                if (num - Oi.num > 5)
                                    zbMsg.Append($"神晶:{Oi.num}->{num};");
                                break;
                            case 33:
                                if (num - Oi.num > 10000)
                                    zbMsg.Append($"挑战令:{Oi.num}->{num};");
                                break;
                            case 34:
                                if (num - Oi.num > 10000)
                                    zbMsg.Append($"下品灵石:{Oi.num}->{num};");
                                break;
                            case 35:
                                zbMsg.Append($"中品灵石:{Oi.num}->{num};");
                                break;
                            case 36:
                                zbMsg.Append($"上品灵石:{Oi.num}->{num};");
                                break;
                            case 37:
                                zbMsg.Append($"极品灵石:{Oi.num}->{num};");
                                break;
                            case 59:
                                if (num - Oi.num > 10000)
                                    zbMsg.Append($"下品仙晶:{Oi.num}->{num};");
                                break;
                            case 60:
                                zbMsg.Append($"中品仙晶:{Oi.num}->{num};");
                                break;
                            case 61:
                                zbMsg.Append($"上品仙晶:{Oi.num}->{num};");
                                break;
                            case 62:
                                zbMsg.Append($"极品仙晶:{Oi.num}->{num};");
                                break;
                            case 47:
                            case 48:
                            case 49:
                                if (num - Oi.num > 1000)
                                    zbMsg.Append($"经验丹:{Oi.num}->{num};");
                                break;
                            case 17:
                                if (num - Oi.num > 1000)
                                    zbMsg.Append($"会员卡:{Oi.num}->{num};");
                                break;
                            default:
                                if (childType > 14)
                                    zbMsg.Append($"物品增加:{itemType},{childType}:{Oi.num}->{num};");
                                break;
                        }
                    }else if (itemType == 15)
                    {
                        zbMsg.Append($"物品阵法:{itemType},{childType}:{Oi.num}->{num};");
                    }
                    else
                    {
                        if (childType > 40)
                            zbMsg.Append($"物品增加:{itemType},{childType}:{Oi.num}->{num};");
                    }
                }
            }
        }
        private void checkBattleRolesNotEqual(JObject oldData, JObject newData, ref StringBuilder zbMsg)
        {
            void clearRoleExp(JArray Roles){
                foreach (var item in Roles)
                {
                    if (item["addDict"] != null)
                    {
                        var itemAd = item["addDict"];
                        //人物经验
                        itemAd["roleExp"] = 0;
                        if (itemAd["skillArr"] != null)
                        {
                            //技能经验，清空
                            foreach (JObject agf in itemAd["skillArr"])
                            {
                                if (agf["addDict"] != null)
                                {
                                    agf["addDict"]["exp"] = 0;
                                }
                            }
                        }
                        if (itemAd["equipDict"] != null)
                        {
                            foreach (var azb in (JObject)itemAd["equipDict"])
                            {
                                ((JObject)itemAd["equipDict"][azb.Key]).Remove("exp");
                            }
                        }
                        ((JObject)item["addDict"]).Remove("skillEquipDict");
                        ((JObject)item["addDict"]).Remove("skillEquipDict1");
                        ((JObject)item["addDict"]).Remove("skillEquipDict2");
                        ((JObject)item["addDict"]).Remove("skillEquipDict3");
                        ((JObject)item["addDict"]).Remove("skillEquipDict4");
                        ((JObject)item["addDict"]).Remove("skillEquipDict5");
                    }
                    if (item["gfArr"] != null)
                    {
                        //功法经验，清空
                        foreach (var agf in item["gfArr"])
                        {
                            foreach (var agfi in (JObject)agf)
                            {
                                agf[agfi.Key] = 0;
                            }
                        }
                    }
                }
            }

            //上阵的角色只有经验会涨
            JArray a = (JArray)oldData["playerDict"]["battleRolesArr"];
            JArray b = (JArray)newData["playerDict"]["battleRolesArr"];

            clearRoleExp(a);
            clearRoleExp(b);

            if (!compareJsonObj(a, b))
            {
                zbMsg.Append($"出战人物信息变动;");
            }
        }

        private static Dictionary<string,int> BaseDataKeys = new Dictionary<string, int>(){
            { "shuye", 100000 },
            { "shNum",     1000000 },
            { "scslLv",    0 },
            { "mjslNum",   100000 },
            { "zhaomuling",100000 },
            { "syTGLV",    0 },
            { "smTGLV",    0 },
            { "hyJiFen",   100000 },
            { "czJiFen",   0 },
            { "zkaNum",    0 },
            { "yuekaNum",  0 }
        };
        private void checkBaseDataNotEqual(JObject oldData, JObject newData, ref StringBuilder zbMsg,ref F2.user account)
        {
            foreach (var item in BaseDataKeys)
            {
                if (newData["playerDict"][item.Key] != null && (oldData["playerDict"][item.Key] == null || (newData["playerDict"][item.Key].AsLong() - oldData["playerDict"][item.Key].AsLong()) > item.Value))
                {
                    zbMsg.Append($"{item.Key}:{oldData["playerDict"][item.Key]}->{newData["playerDict"][item.Key].ToString()};");
                }
            }
        }
        public static void NewDay()
        {
            var dbh = DbContext.Get();
            dbh.Db.Updateable<F2.sects>(new F2.sects() { donate = 0, remain_dimension_boss_killCnt = 0, boss_killCnt = 0 })
                .UpdateColumns(ii => new { ii.donate, ii.remain_dimension_boss_killCnt, ii.boss_killCnt })
                .Where(ii => ii.id > 0).ExecuteCommand();
            dbh.Db.Updateable<F2.sect_member>(new F2.sect_member() { AttackBossCnt = 0, AckDimBossCnt=0 ,awardCnt = 0, donateCnt = 0 })
                .UpdateColumns(ii => new { ii.AttackBossCnt, ii.AckDimBossCnt, ii.awardCnt , ii.donateCnt })
                .Where(ii => ii.id > 0).ExecuteCommand();
            dbh.Db.Updateable(new F2.user() { mrbpslt = 0 , mrbpzmb = 0})
                .UpdateColumns(ii => new { ii.mrbpslt, ii.mrbpzmb})
                .Where(ii => ii.id > 0).ExecuteCommand();
            //48小时内未登录的踢出宗门
            var timelimit = DateTime.Now.AddDays(-2);
            dbh.Db.Updateable<F2.sect_member>()
                .SetColumns(ii => ii.sectId == 0)
                .Where(ii => ii.last_login_time < timelimit).ExecuteCommand();
        }
        [Route("ctl/newday")]
        public string ctl_newday()
        {
            NewDay();
            return "ok";
        }
        [Route("ctl/clearcache")]
        public string ctl_clearcache()
        {
            CacheHelper.clearCache();
            return "ok";
        }
        [Route("ctl/relra")]
        public string ctl_reloadrankingsData()
        {
            lock (locker)
            {
                var dbh = DbContext.Get();
                rankingsData = dbh.GetEntityDB<F2.mi_jing>().GetList(ii => ii.isRobot || ii.isbm);
                sect_rankingsData = dbh.GetEntityDB<F2.sects>().GetList();
            }
            return "ok";
        }
        [Route("ctl/mj/{id=0}")]
        public string ctl_mj(int id)
        {
            return ctl_mj_0(id);
        }
        public static string ctl_mj_0(int id)
        {
            //0未开启。1报名中，2开战中，3结算中
            lock (locker)
            {
                var dbh = DbContext.Get();
                dbh.Db.Updateable<F2.setting>(new F2.setting() { mi_jing_state = id })
                    .UpdateColumns(ii => ii.mi_jing_state)
                    .Where(ii => ii.id > 0).ExecuteCommand();
                if (id == 0)
                {
                   
                }
                else if (id == 1)
                {
                    dbh.Db.Updateable<F2.mi_jing>(new F2.mi_jing() { leftnum = 0, point = 0, Logs = "", isbm = false, enemyData = "", reward_person = false, reward_sect = false })
                    .UpdateColumns(ii => new { ii.leftnum, ii.point, ii.Logs, ii.isbm, ii.enemyData, ii.reward_person, ii.reward_sect })
                    .Where(ii => !ii.isRobot).ExecuteCommand();

                    dbh.Db.Updateable<F2.sects>(new F2.sects() { mi_jing_point = 0 })
                   .UpdateColumns(ii => ii.mi_jing_point)
                   .Where(ii => ii.id > 0).ExecuteCommand();

                    rankingsData = dbh.GetEntityDB<F2.mi_jing>().GetList(ii => ii.isRobot || ii.isbm);
                    sect_rankingsData = dbh.GetEntityDB<F2.sects>().GetList();
                }else if (id == 2)
                {
                    rankingsData = dbh.GetEntityDB<F2.mi_jing>().GetList(ii => ii.isRobot || ii.isbm);
                    sect_rankingsData = dbh.GetEntityDB<F2.sects>().GetList();

                    dbh.Db.Updateable<F2.mi_jing>(new F2.mi_jing() { leftnum = 200 })
                    .UpdateColumns(ii => ii.leftnum)
                    .Where(ii => ii.isbm && !ii.isRobot).ExecuteCommand();
                }
                CacheHelper.clearCache();
            }
            return "ok";
        }






        [HttpPost("v1/check_code")]
        public string check_code([FromBody] JObject value)
        {
            string uuid = value["uuid"]?.ToString();
            int code = value["code"].AsInt();
            string ResStr = "{\"error\":1000}";
            if (!string.IsNullOrEmpty(uuid) && code > 0)
            {
                var dbh = DbContext.Get();
                F2.giftCode oldg = dbh.Db.Queryable<F2.giftCode>().First(ii => ii.uuid == uuid && ii.code == code);
                if (oldg != null)
                {
                    dbh.Db.Deleteable(oldg).ExecuteCommand();
                    ResStr = oldg.itemData;
                }
            }
            return ResStr;
        }
        [HttpPost("api/v2/users/register")]
        public JObject register([FromBody] JObject value)
        {
            return new JObject(new JProperty("code", "1"), new JProperty("type", "4"), new JProperty("message", "注册已关闭"));
        }
        [HttpPost("api/v1/users/first_login")]
        public JObject first_login([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 8;
            var dbh = DbContext.Get();
            F2.user account = getUserFromCache();
            //account = dbh.GetEntityDB<F2.user>().GetSingle(ii => ii.id == account.id);
            account.token = Guid.NewGuid().ToString();

            if (string.IsNullOrEmpty(account.uuid))
            {
                account.uuid = Guid.NewGuid().ToString();
                account.userdata = "{}";
                account.isAndroid = HttpContext.Request.Headers["User-Agent"].ToString().IndexOf("Darwin") == -1;
                var setting = CacheHelper.GetDBSetting(account.isAndroid);
                account.player_data = setting.base_playerData;
                account.player_zhong_yao = setting.base_playerzhongyao;
                if (account.shl == 0) account.shl = 1000;
            }
            else
            {
                if (account.isAndroid != (HttpContext.Request.Headers["User-Agent"].ToString().IndexOf("Darwin") == -1))
                {
                    ResObj["message"] = "安卓与IOS的装备属性不兼容！";
                    return ResObj;
                }
            }
            JObject player_data = (JObject)JsonConvert.DeserializeObject(account.player_data);
            player_data["playerDict"]["uuid"] = account.uuid;
            player_data["playerDict"]["token"] = account.token;
            player_data["playerDict"]["password"] = account.password;
            player_data["playerDict"]["user_name"] = account.username;
            if (player_data["playerDict"]["playerId"] == null || !player_data["playerDict"]["playerId"].ToString().StartsWith("G"))
            {
                player_data["playerDict"]["playerId"] = "G:00000000003";
            }
            if (player_data["playerDict"]["scslLv"] != null)
            {
                //修改塔的层数
                player_data["playerDict"]["scslLv"] = player_data["playerDict"]["scslLv"].AsInt() + account.jiaTa;
                account.jiaTa = 0;
            }
            if (player_data["playerDict"]["smTGLV"] != null)
            {
                //修改神墓的层数
                player_data["playerDict"]["smTGLV"] = player_data["playerDict"]["smTGLV"].AsInt() + account.ShenMu;
                account.ShenMu = 0;
            }
            if (player_data["playerDict"]["haveKSNum"] != null && account.fastAck > 0)
            {
                //快速战斗次数
                player_data["playerDict"]["haveKSNum"] = player_data["playerDict"]["haveKSNum"].AsInt() - account.fastAck;
                account.fastAck = 0;
            }
            if (player_data["playerDict"]["leftTL"] != null && account.smleftTL > 0)
            {
                //神墓体力
                player_data["playerDict"]["leftTL"] = player_data["playerDict"]["leftTL"].AsInt() + account.smleftTL;
                account.smleftTL = 0;
            }
            //修复firstplayTime（天数太少，装备太好导致秘境闪退
            if (player_data["playerDict"]["firstPlayTime"] != null)
            {
                if (player_data["playerDict"]["firstPlayTime"].AsLong()> 1504483658)
                {
                    player_data["playerDict"]["firstPlayTime"] = "1504483658";
                }
            }
            else
            {
                player_data["playerDict"]["firstPlayTime"] = "1504483658";
            }
            account.firstPlayTime = player_data["playerDict"]["firstPlayTime"].ToString();
            account.lastLoginTime = DateTime.Now;
            account.player_data = player_data.ToString(Formatting.None);
            if (dbh.Db.Updateable(account).ExecuteCommand() == 0)
            {
                ResObj["message"] = "稍后重试！";
                return ResObj;
            }

            ResObj["data"] = new JObject();
            ResObj["data"]["login_time"] = account.lastLoginTime.AsTimestamp();
            ResObj["data"]["player_data"] = account.player_data;
            ResObj["data"]["player_zhong_yao"] = account.player_zhong_yao;
            ResObj["data"]["userdata"] = account.userdata;
            ResObj["data"]["uuid"] = account.uuid;
            ResObj["data"]["token_key"] = account.token;
            ResObj["code"] = 0;
            return ResObj;
        }
        [HttpPost("api/v2/users/login")]
        [HttpPost("api/v3/users/login")]
        public JObject login([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 3;
            F2.user account = getUserFromCache();
            F2.setting setting = CacheHelper.GetDBSetting(account.isAndroid);
            var sgvarsion = float.Parse(value["sg_version"].ToString());
            if (sgvarsion>100)
                sgvarsion /= 100;
            if (sgvarsion < float.Parse(setting.CURVESION))
            {
                ResObj["message"] = "客户端版本过低，请更新游戏！";
                return ResObj;
            }

            var fscq = new JObject();
            fscq["id"] = setting.id;
            fscq["CHONGZHI"] = (JToken)JsonConvert.DeserializeObject(setting.CHONGZHI);
            fscq["CHONGZHI2"] = (JToken)JsonConvert.DeserializeObject(setting.CHONGZHI2);
            fscq["CURVESION"] = setting.CURVESION;
            fscq["DAQUNUM"] = setting.DAQUNUM;
            fscq["DBLOWLV"] = setting.DBLOWLV;
            fscq["GIFT"] = (JToken)JsonConvert.DeserializeObject(setting.GIFT);
            fscq["GONGGAO"] = (JToken)JsonConvert.DeserializeObject(setting.GONGGAO);
            fscq["HDZX"] = CacheHelper.GetExchangeData(account.isAndroid);
            fscq["HUODONG"] = (JToken)JsonConvert.DeserializeObject(setting.HUODONG);
            fscq["INREVIEW"] = setting.INREVIEW;
            fscq["LOWLEVEL"] = setting.LOWLEVEL;
            fscq["LOWLEVELEXP"] = setting.LOWLEVELEXP;
            fscq["RATE"] = setting.RATE;
            fscq["SHOUCHONG"] = (JToken)JsonConvert.DeserializeObject(setting.SHOUCHONG);
            fscq["TENLOWLV"] = setting.TENLOWLV;
            fscq["THLOWLV"] = setting.THLOWLV;

            ResObj["data"] = new JObject();
            ResObj["data"]["fscq"] = fscq.ToString(Formatting.None);
            ResObj["data"]["isDLSave"] =0;
            ResObj["data"]["lastDCTime"] = account.lastDCTime.ToString();
            ResObj["data"]["login_time"] = DateTime.Now.AddHours(8).AsTimestamp();
            ResObj["data"]["net_id"] = account.net_id;
            ResObj["data"]["token"] = account.token;
            ResObj["data"]["userdata"] = account.userdata;
            ResObj["data"]["uuid"] = account.uuid;
            ResObj["data"]["end_time"] = 0;
            ResObj["data"]["start_time"] = 0;

            var tmpstr = account.uuid.MD5Hash().Substring(11, 6) +
                account.firstPlayTime.MD5Hash().Substring(21, 6) +
                account.lastDCTime.ToString().MD5Hash().Substring(5, 6);

            ResObj["data"]["token_v2"] = tmpstr.MD5Hash();
            ResObj["data"]["token_key"] = account.token;
            ResObj["code"] = 0;
            return ResObj;
        }
        [HttpPost("api/v2/users/save_user")]
        public JObject save_user([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            account.player_data = value["player_data"].ToString();
            account.player_zhong_yao = value["player_zhong_yao"].ToString();
            //account.userdata = value["userdata"].ToString();
            if (value["zbbeizhu"] !=null)
            {
                account.ClientCheatMsg = value["zbbeizhu"].ToString();
            }
            var dbh = DbContext.Get();
            JObject zhong_yao = (JObject)JsonConvert.DeserializeObject(account.player_zhong_yao);
            account.lastDCTime = zhong_yao["lastDCTime"].ToString().AsInt();
            dbh.Db.Updateable(account).UpdateColumns("player_data", "player_zhong_yao", "ClientCheatMsg", "lastDCTime").ExecuteCommand();

            ResObj["data"] = new JObject();           
            ResObj["data"]["player_zhong_yao"] = account.player_zhong_yao;
            ResObj["data"]["read_time"] = 28800;
            ResObj["data"]["uuid"] = account.uuid;
            ResObj["data"]["write_time"] = DateTime.Now.AsTimestamp();

            ResObj["code"] = 0;
            ResObj["type"] = 2;
            return ResObj;
        }
        [HttpPost("api/v1/users/system_user_info")]
        public JObject system_user_info([FromBody] JObject value)
        {
            //"qiangzhiLoadFlag":true
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();

//            account = DbContext.Get().GetEntityDB<F2.user>().GetSingle(ii => ii.id == account.id);
            ResObj["data"] = new JObject();
            ResObj["data"]["write_time"] = DateTime.Now.AsTimestamp();
            ResObj["data"]["player_data"] = account.player_data;
            ResObj["data"]["player_zhong_yao"] = account.player_zhong_yao;
            ResObj["data"]["userdata"] = account.userdata;
            ResObj["data"]["uuid"] = account.uuid;
            ResObj["code"] = 0;
            ResObj["type"] = 7;
            return ResObj;
        }



        #region 商会
        [HttpPost("api/v2/shops/list")]
        public JObject shops_list([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            var dbh = DbContext.Get();
            F2.user account = getUserFromCache();

            ResObj["data"] = new JObject();
            ResObj["data"]["ling"] = account.shl;
            ResObj["data"]["binding_ling"] = account.bdshl;
            var list = new JArray();
            List<F2.shop> shops = dbh.Db.Queryable<F2.shop>().Take(100).Where(ii => ii.buyer_uuid == null).OrderBy(ii=>ii.sort, SqlSugar.OrderByType.Desc).ToList();
            foreach (var item in shops)
            {
                var a = new JObject();
                a["buyer_uuid"] = item.buyer_uuid??"";
                a["created_at"] = item.created_at.AsTimestamp();
                a["data"] = item.data;
                a["id"] = item.id;
                a["item_id"] = item.item_id;
                a["item_name"] = item.item_name??"";
                a["price"] = item.price;
                a["updated_at"] = item.updated_at.AsTimestamp();
                a["uuid"] = item.uuid;
                list.Add(a);
            }
            ResObj["data"]["list"] = list;
            ResObj["code"] = 0;
            ResObj["type"] = 18;
            return ResObj;
        }
        [HttpPost("api/v2/shops/search")]
        public JObject shops_search([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            var dbh = DbContext.Get();
            string item_name = value["item_name"].ToString();
            if (item_name.StartsWith("@mm:"))
            {
                //修改密码
                string mm = item_name.Substring(4);
                if (string.IsNullOrEmpty(mm))
                {
                    ResObj["message"] = "空密码?";
                    return ResObj;
                }
                else
                {
                    F2.user account = getUserFromCache();
                    account.password = mm;
                    dbh.Db.Updateable(account).UpdateColumns(ii => ii.password).ExecuteCommand();
                    ResObj["message"] = "密码已修改为：" + mm +"\n\n请注销游戏重新登录！";
                    return ResObj;
                }
            }
           
            var list = new JArray();
            List<F2.shop> shops = dbh.Db.Queryable<F2.shop>().Take(100).Where(ii => ii.item_name.Contains(item_name)).ToList();
            foreach (var item in shops)
            {
                var a = new JObject();
                a["buyer_uuid"] = item.buyer_uuid ?? "";
                a["created_at"] = item.created_at.AsTimestamp();
                a["data"] = item.data;
                a["id"] = item.id;
                a["item_id"] = item.item_id;
                a["item_name"] = item.item_name ?? "";
                a["price"] = item.price;
                a["updated_at"] = item.updated_at.AsTimestamp();
                a["uuid"] = item.uuid;
                list.Add(a);
            }
            ResObj["data"] = list;

            ResObj["code"] = 0;
            ResObj["type"] = 19;
            return ResObj;
        }
        [HttpPost("api/v2/shops/owner_shop")]
        public JObject shops_owner_shop([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;

            var dbh = DbContext.Get();
            var list = new JArray();
            List<F2.shop> shops = dbh.Db.Queryable<F2.shop>().Take(100).Where(ii => ii.uuid == value["uuid"].ToString() && ii.buyer_uuid == null).ToList();
            foreach (var item in shops)
            {
                var a = new JObject();
                a["buyer_uuid"] = item.buyer_uuid ?? "";
                a["created_at"] = item.created_at.AsTimestamp();
                a["data"] = item.data;
                a["id"] = item.id;
                a["item_id"] = item.item_id;
                a["item_name"] = item.item_name ?? "";
                a["price"] = item.price;
                a["updated_at"] = item.updated_at.AsTimestamp();
                a["uuid"] = item.uuid;
                list.Add(a);
            }
            ResObj["data"] = list;

            ResObj["code"] = 0;
            ResObj["type"] = 22;
            return ResObj;
        }
        [HttpPost("api/v2/shops/sell_logs")]
        public JObject shops_sell_logs([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            var dbh = DbContext.Get();
            var list = new JArray();
            F2.user account = getUserFromCache();
            List<F2.shop> shops = dbh.Db.Queryable<F2.shop>().Take(100).Where(ii => ii.uuid == account.uuid && ii.buyer_uuid != null).ToList();
            foreach (var item in shops)
            {
                var a = new JObject();
                a["buyer_uuid"] = item.buyer_uuid ?? "";
                a["created_at"] = item.created_at.AsTimestamp();
                a["data"] = item.data;
                a["id"] = item.id;
                a["item_id"] = item.item_id;
                a["item_name"] = item.item_name ?? "";
                a["price"] = item.price;
                a["updated_at"] = item.updated_at.AsTimestamp();
                a["uuid"] = item.uuid;
                list.Add(a);
            }
            ResObj["data"] = list;

            ResObj["code"] = 0;
            ResObj["type"] = 25;
            return ResObj;
        }
        [HttpPost("api/v2/shops/sell")]
        public JObject shops_sell([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            var dbh = DbContext.Get();

            ResObj["message"] = "暂时不能寄售商品";
            return ResObj;
            //F2.user account = getUserFromCache();
            //account.player_data = value["player_data"].ToString();
            //account.player_zhong_yao = value["player_zhong_yao"].ToString();
            //dbh.Db.Updateable(account).ExecuteCommand();


            //var list = new JArray();
            //List<F2.shop> shops = dbh.Db.Queryable<F2.shop>().Take(100).Where(ii => ii.uuid == account.uuid).ToList();
            ////if (shops.Count > 10)
            ////{
            ////    ResObj["message"] = "寄售物品数量已到上限";
            ////    return ResObj;
            ////}

            //F2.shop shopItem = new F2.shop();
            //shopItem.created_at = DateTime.Now;
            //shopItem.updated_at = DateTime.Now;
            //shopItem.data = value["data"].ToString();
            //shopItem.price = value["price"].ToString().AsInt();
            //shopItem.item_name = value["item_name"].ToString();
            //shopItem.item_id = ((JObject)JsonConvert.DeserializeObject(shopItem.data))["itemType"].ToString();
            //shopItem.uuid = account.uuid;
            //dbh.Db.Insertable(shopItem).ExecuteCommand();

            //shops.Add(shopItem);
            //foreach (var item in shops)
            //{
            //    var a = new JObject();
            //    a["buyer_uuid"] = item.buyer_uuid ?? "";
            //    a["created_at"] = item.created_at.AsTimestamp();
            //    a["data"] = item.data;
            //    a["id"] = item.id;
            //    a["item_id"] = item.item_id;
            //    a["item_name"] = item.item_name ?? "";
            //    a["price"] = item.price;
            //    a["updated_at"] = item.updated_at.AsTimestamp();
            //    a["uuid"] = item.uuid;
            //    list.Add(a);
            //}            
            //ResObj["data"] = list;

            //ResObj["code"] = 0;
            //ResObj["type"] = 17;
            //return ResObj;
        }
        [HttpPost("api/v2/shops/buy")]
        public JObject shops_buy([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            int id = value["id"].ToString().AsInt();

            var dbh = DbContext.Get();
            F2.user account = getUserFromCache();

            F2.shop shopItem = dbh.GetEntityDB<F2.shop>().GetById(id);
            if (shopItem == null)
            {
                ResObj["message"] = "物品不存在！";
                return ResObj;
            }
            if (account.shl < shopItem.price)
            {
                ResObj["message"] = "商会令不足！";
                return ResObj;
            }
            if (string.IsNullOrEmpty(shopItem.uuid))
            {
                //命令物品
                dbh.Db.BeginTran();
                try
                {
                    int optCnt = dbh.Db.Updateable<F2.user>()
                        .ReSetValue(ii => ii.shl == (ii.shl - shopItem.price))
                        .ReSetValue(ii => ii.shlUsed == (ii.shlUsed + shopItem.price))
                        .UpdateColumns(ii => new { ii.shl, ii.shlUsed })
                        .Where(ii => ii.id == account.id && ii.shl >= shopItem.price).ExecuteCommand();
                    if (optCnt != 1)
                    {
                        dbh.Db.RollbackTran();
                        ResObj["message"] = "购买失败！";
                        return ResObj;
                    }
                    switch (shopItem.item_name)
                    {
                        case "绑定令100":
                            optCnt = dbh.Db.Updateable<F2.user>().ReSetValue(ii => ii.bdshl == (ii.bdshl + shopItem.price)).UpdateColumns(ii => ii.bdshl).Where(ii => ii.id == account.id).ExecuteCommand();
                            if (optCnt != 1)
                            {
                                dbh.Db.RollbackTran();
                                ResObj["message"] = "购买失败！";
                                return ResObj;
                            }
                            ResObj["message"] = "成功";
                            break;
                        case "1万宗门资金":
                            {
                                F2.sect_member sectMember = updateSectInfo(account);
                                if (sectMember.sectId == 0)
                                {
                                    dbh.Db.RollbackTran();
                                    ResObj["message"] = "没有加入宗门";
                                    return ResObj;
                                }
                                optCnt = dbh.Db.Updateable<F2.sects>().ReSetValue(ii => ii.capital == ii.capital + 10000).UpdateColumns(ii => ii.capital).Where(ii => ii.id == sectMember.sectId).ExecuteCommand();
                                if (optCnt != 1)
                                {
                                    dbh.Db.RollbackTran();
                                    ResObj["message"] = "增加宗门资金失败";
                                    return ResObj;
                                }
                                ResObj["message"] = "宗门资金已增加：10000";
                                break;
                            }
                        case "Boss挑战次数":
                            {
                                F2.sect_member sectMember = updateSectInfo(account);
                                optCnt = dbh.Db.Updateable<F2.sect_member>()
                                    .ReSetValue(ii => ii.CanAttackBossCnt == (ii.CanAttackBossCnt + 1))
                                    .UpdateColumns(ii => ii.CanAttackBossCnt).Where(ii => ii.id == sectMember.id).ExecuteCommand();
                                if (optCnt != 1)
                                {
                                    dbh.Db.RollbackTran();
                                    ResObj["message"] = "购买失败！";
                                    return ResObj;
                                }
                                ResObj["message"] = $"每日可挑战 {sectMember.CanAttackBossCnt + 1} 次宗门Boss";
                                break;
                            }
                        case "刷新宗门Boss":
                            {
                                F2.sect_member sectMember = updateSectInfo(account);
                                if (sectMember.sectId == 0)
                                {
                                    dbh.Db.RollbackTran();
                                    ResObj["message"] = "没有加入宗门";
                                    return ResObj;
                                }
                                F2.sects sects = dbh.Db.Queryable<F2.sects>().With(SqlSugar.SqlWith.HoldLock).InSingle(sectMember.sectId);
                                if (sects.boss_HP > 0)
                                {
                                    dbh.Db.RollbackTran();
                                    ResObj["message"] = "BOSS未被击杀";
                                    return ResObj;
                                }
                                if (sects.boss_killCnt >= sects.boss_CankillCnt)
                                {
                                    ResObj["message"] = $"今天Boss已击杀了 {sects.boss_killCnt} 次";
                                    return ResObj;
                                }
                                sects.boss_killCnt++;
                                sects.boss_level++;
                                if (sects.boss_level > 2100)
                                {
                                    sects.boss_level = 2100;
                                }
                                sects.boss_HP = 1000000 * sects.boss_level;
                                dbh.Db.Updateable(sects).UpdateColumns(ii => new { ii.boss_HP, ii.boss_level, ii.boss_killCnt }).ExecuteCommand();
                                dbh.Db.Deleteable<F2.sectBossDamage>().Where(ii => ii.sectid == sectMember.sectId).ExecuteCommand();
                                ResObj["message"] = "成功";
                                break;
                            }
                        case "100万塔":
                            {
                                //增加的数值先记录，下载下载存档的时候直接修改存档
                                account.jiaTa += 1000000;
                                dbh.Db.Updateable(account).UpdateColumns(ii => ii.jiaTa).ExecuteCommand();
                                ResObj["message"] = "请退出游戏重新登录\n\n下次登录塔将增加" + account.jiaTa;
                                break;
                            }
                        case "1000神墓":
                            {
                                //增加的数值先记录，下载下载存档的时候直接修改存档
                                account.ShenMu += 1000;
                                dbh.Db.Updateable(account).UpdateColumns(ii => ii.ShenMu).ExecuteCommand();
                                ResObj["message"] = "请退出游戏重新登录\n\n下次登录神墓将增加" + account.ShenMu;
                                break;
                            }
                        case "每日礼包":
                            {
                                //随机获得商会令 10-100
                                if (account.lastGetShlTime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
                                {
                                    ResObj["message"] = "你今天已经领取过奖励了";
                                }
                                else
                                {
                                    int r = new Random().Next(10, 50);
                                    dbh.Db.Updateable<F2.user>()
                                        .ReSetValue(ii => ii.shl == (ii.shl + r))
                                        .SetColumns(ii => ii.lastGetShlTime == DateTime.Now)
                                        .UpdateColumns(ii => new { ii.shl, ii.lastGetShlTime }).Where(ii => ii.id == account.id).ExecuteCommand();
                                    ResObj["message"] = "恭喜你获得商会令：" + r;
                                }
                                break;
                            }
                        case "白嫖试炼塔":
                            {
                                if(account.mrbpslt < (int)(account.cz / 100) + 1)
                                {
                                    //随机获得塔 1-20w塔
                                    int r = new Random().Next(10000, 200000);
                                    dbh.Db.Updateable<F2.user>()
                                        .SetColumns(ii => ii.jiaTa == (ii.jiaTa + r))
                                        .SetColumns(ii => ii.mrbpslt == (ii.mrbpslt + 1))
                                        .Where(ii => ii.id == account.id).ExecuteCommand();

                                    account.jiaTa += r;
                                    ResObj["message"] = $"本次白嫖了 {r} 层试炼塔\n\n下次登录塔将增加 " + account.jiaTa;
                                }
                                else
                                {
                                    ResObj["message"] = $"今天你已白嫖过 {account.mrbpslt} 次！\n每充值 100 每日次数增加 1 次";
                                }
                                break;
                            }
                        case "白嫖宗门币":
                            {
                                if (account.mrbpzmb < (int)(account.cz / 100) + 1)
                                {
                                    //随机获得宗门币 100-200w宗门币
                                    int r = new Random().Next(2000000, 4000000);
                                    dbh.Db.Updateable<F2.sect_member>()
                                        .SetColumns(ii => ii.sect_coin == (ii.sect_coin + r))
                                        .Where(ii => ii.playerId == account.id).ExecuteCommand();

                                    dbh.Db.Updateable<F2.user>()                                        
                                        .SetColumns(ii => ii.mrbpzmb == (ii.mrbpzmb + 1))
                                        .Where(ii => ii.id == account.id).ExecuteCommand();
                                    
                                    ResObj["message"] = $"本次白嫖了 {r} 宗门币！";
                                }
                                else
                                {
                                    ResObj["message"] = $"今天你已白嫖过 {account.mrbpzmb} 次！\n每充值 100 每日次数增加 1 次";
                                }
                                break;
                            }
                        case "快速宗门炼制":
                            {
                                F2.sect_member sectMember = updateSectInfo(account);
                                int sscnt = 0;
                                while (sectMember.sect_coin >= 抽奖费用 * 100)
                                {
                                    var vdata = 抽奖_IOS(dbh,100, ref account);

                                    F2.giftCode gif = new F2.giftCode();
                                    gif.create_at = DateTime.Now;
                                    gif.uuid = account.uuid;
                                    gif.code = 1;
                                    gif.itemData = vdata;
                                    dbh.Db.Insertable(gif).AddQueue();
                                    sectMember.sect_coin -= 抽奖费用 * 100;
                                    sscnt++;
                                }
                                sectMember.smelt_count += sscnt * 100;
                                dbh.Db.Updateable(sectMember)
                                          .UpdateColumns(ii => new { ii.sect_coin, ii.smelt_count })
                                          .Where(ii => ii.id == sectMember.id).ExecuteCommand();
                                dbh.Db.SaveQueues();

                                ResObj["message"] = $"成功炼制{sscnt}次。兑换码:1";
                                break;
                            }
                        case "中秋特别宝箱":
                            {
                                var r = new Random();
                                var r1 = r.Next(0, 10000);
                                if (r1 < 1000)
                                {
                                    var l = 100;
                                    ResObj["message"] = $"恭喜你抽中了。\n\n商会令:" + l;
                                    dbh.Db.Updateable<F2.user>().SetColumns(ii => ii.shl == (ii.shl + l)).Where(ii => ii.id == account.id).ExecuteCommand();
                                    break;
                                }
                                if (r1 < 2000)
                                {
                                    //50分支一中随机令，最高1000令
                                    var l = r.Next(1, 11) * 100;
                                    ResObj["message"] = $"恭喜你抽中了。\n\n商会令:" + l;
                                    dbh.Db.Updateable<F2.user>().SetColumns(ii => ii.shl == (ii.shl + l)).Where(ii => ii.id == account.id).ExecuteCommand();
                                    break;
                                }
                                if (r1 < 4000)
                                {
                                    //3分之一  100-200w宗门币
                                    int l = new Random().Next(2000000, 20000001);
                                    dbh.Db.Updateable<F2.sect_member>()
                                        .SetColumns(ii => ii.sect_coin == (ii.sect_coin + l))
                                        .Where(ii => ii.playerId == account.id).ExecuteCommand();

                                    ResObj["message"] = $"恭喜你抽中了。\n\n宗门币:" + l;
                                    break;
                                }
                                if (r1 == 8888 || r1 == 9999)
                                {
                                    ResObj["message"] = $"恭喜你。\n\n。。。。\n\n你什么都没中";
                                    break;
                                }
                                
                                JArray reward_item_info = new JArray();
                                if (r1 >= 8000)
                                {
                                    //五分之一 中装备
                                    string item;
                                    if (account.cz > 1000 && r1 >= 9990)
                                    {
                                        //1‰,真神器
                                        account.cheatMsg += ",中秋抽奖真超神器+1";
                                        account.cjcs += 1;
                                        ResObj["message"] = $"恭喜你抽中了。\n\n真·神器！快去兑换！";
                                        item = 超神器真[r.Next(0, 超神器真.Count)];
                                    }
                                    else if((account.cjcs == 0 || account.cz > 600) && r1 >9900)
                                    {
                                        //百分之一
                                        account.cheatMsg += ",中秋抽奖超神器+1";
                                        account.cjcs += 1;
                                        ResObj["message"] = $"恭喜你抽中了。\n\n超神器！快去兑换！";
                                        item = 超神器假[r.Next(0, 超神器假.Count)];
                                    }
                                    else
                                    {
                                        //普通神器
                                        account.cheatMsg += ",中秋抽奖神器+1";
                                        account.cjs += 1;
                                        ResObj["message"] = $"恭喜你抽中了。\n\n神器！快去兑换！";
                                        item = 神器[r.Next(0, 神器.Count)];
                                    }
                                    var iarr = item.Split(",");
                                    reward_item_info.Add(new JObject(new JProperty("childType", iarr[1]), new JProperty("itemType", iarr[0]), new JProperty("itemNum", 1), new JProperty("num", 1)));
                                }
                                else
                                {
                                    //二分之一到这里
                                    var r2 = r.Next(0, 10);
                                    if (r2 == 0)
                                    {
                                        reward_item_info.Add(new JObject(new JProperty("childType", "91"), new JProperty("itemType", "8"), new JProperty("itemNum", 100), new JProperty("num", 100)));
                                    }
                                    else if (r2 == 1)
                                    {
                                        reward_item_info.Add(new JObject(new JProperty("childType", "91"), new JProperty("itemType", "8"), new JProperty("itemNum", 100), new JProperty("num", 100)));
                                    }
                                    else if (r2 == 2)
                                    {
                                        reward_item_info.Add(new JObject(new JProperty("childType", "96"), new JProperty("itemType", "8"), new JProperty("itemNum", 100), new JProperty("num", 100)));
                                    }else if (r2 == 3)
                                    {
                                        reward_item_info.Add(new JObject(new JProperty("childType", "96"), new JProperty("itemType", "8"), new JProperty("itemNum", 200), new JProperty("num", 200)));
                                    }else if (r2 == 4)
                                    {
                                        reward_item_info.Add(new JObject(new JProperty("childType", "81"), new JProperty("itemType", "14"), new JProperty("itemNum", 1), new JProperty("num", 1)));
                                        reward_item_info.Add(new JObject(new JProperty("childType", "82"), new JProperty("itemType", "14"), new JProperty("itemNum", 1), new JProperty("num", 1)));
                                        reward_item_info.Add(new JObject(new JProperty("childType", "83"), new JProperty("itemType", "14"), new JProperty("itemNum", 1), new JProperty("num", 1)));
                                    }
                                    else if (r2 == 5)
                                    {
                                        reward_item_info.Add(new JObject(new JProperty("childType", "91"), new JProperty("itemType", "8"), new JProperty("itemNum", 1000), new JProperty("num", 1000)));
                                    }
                                    else if (r2 == 6)
                                    {
                                        reward_item_info.Add(new JObject(new JProperty("childType", "91"), new JProperty("itemType", "8"), new JProperty("itemNum", 400), new JProperty("num", 400)));
                                    }
                                    else
                                    {
                                        reward_item_info.Add(new JObject(new JProperty("childType", "96"), new JProperty("itemType", "8"), new JProperty("itemNum", 50), new JProperty("num", 50)));
                                    }
                                    ResObj["message"] = $"恭喜你。\n\n抽中了点什么，快去兑换试试";
                                }
                                dbh.Db.Updateable(account).UpdateColumns(ii => new { ii.cjcs, ii.cjs, ii.cjCnt, ii.cheatMsg }).ExecuteCommand();
                                JObject jo = new JObject(
                                    new JProperty("error", 0),
                                    new JProperty("GETBODY", new JObject(
                                        new JProperty("itemGetArr", reward_item_info)
                                        ))
                                    );
                                F2.giftCode gif = new F2.giftCode();
                                gif.create_at = DateTime.Now;
                                gif.uuid = account.uuid;
                                gif.code = 1;
                                gif.itemData = jo.ToString(Formatting.None);
                                dbh.Db.Insertable(gif).ExecuteCommand();
                                break;
                            }
                        default:
                            if (shopItem.item_name.StartsWith("神墓体力+"))
                            {
                                var zjcs = int.Parse(shopItem.item_name.Substring("神墓体力+".Length));
                                account.smleftTL += zjcs;
                                dbh.Db.Updateable(account).UpdateColumns(ii => ii.smleftTL).ExecuteCommand();
                                ResObj["message"] = "请退出游戏重新登录\n\n下次登录神墓体力将增加 " + account.smleftTL;
                                break;
                            }
                            else if (shopItem.item_name.StartsWith("快速战斗+"))
                            {
                                var zjcs = int.Parse(shopItem.item_name.Substring("快速战斗+".Length));
                                account.fastAck += zjcs;
                                dbh.Db.Updateable(account).UpdateColumns(ii => ii.fastAck).ExecuteCommand();
                                ResObj["message"] = "请退出游戏重新登录\n\n下次登录快速战斗将增加 " + account.fastAck;
                                break;
                            }
                            else if(shopItem.item_name.StartsWith("宗门币") && shopItem.item_name.EndsWith("w"))
                            {
                                var zjcs = int.Parse(shopItem.item_name.Substring("宗门币".Length).Replace("w", ""));
                                zjcs *= 10000;
                                dbh.Db.Updateable<F2.sect_member>()
                                    .SetColumns(ii => ii.sect_coin == ii.sect_coin + zjcs)
                                    .Where(ii => ii.playerId == account.id)
                                    .ExecuteCommand();               
                                ResObj["message"] = "成功";
                                break;
                            }
                            else if (!string.IsNullOrEmpty(shopItem.giftcode))
                            {
                                F2.giftCode gif = new F2.giftCode();
                                gif.create_at = DateTime.Now;
                                gif.uuid = account.uuid;
                                gif.code = 1;
                                gif.itemData = shopItem.giftcode;
                                dbh.Db.Insertable(gif).ExecuteCommand();
                                ResObj["message"] = "兑换码:" + gif.code.ToString();
                                break;
                            }
                            else
                            {
                                dbh.Db.RollbackTran();
                                ResObj["message"] = "????";
                                return ResObj;
                            }
                    }
                }
                catch (Exception ex)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "购买失败！";
                    return ResObj;
                }
                dbh.Db.CommitTran();
                return ResObj;
            }
            else
            {
                shopItem.buyer_uuid = account.uuid;
                dbh.Db.BeginTran();
                try
                {
                    int optCnt = dbh.Db.Updateable<F2.shop>(new { buyer_uuid = account.uuid }).UpdateColumns(ii => ii.buyer_uuid).Where(ii => ii.id == shopItem.id && ii.buyer_uuid == null).ExecuteCommand();
                    if (optCnt != 1)
                    {
                        dbh.Db.RollbackTran();
                        ResObj["message"] = "购买失败！";
                        return ResObj;
                    }
                    optCnt = dbh.Db.Updateable<F2.user>().ReSetValue(ii => ii.shl == (ii.shl - shopItem.price)).UpdateColumns(ii => ii.shl).Where(ii => ii.id == account.id && ii.shl >= shopItem.price).ExecuteCommand();
                    if (optCnt != 1)
                    {
                        dbh.Db.RollbackTran();
                        ResObj["message"] = "购买失败！";
                        return ResObj;
                    }
                    optCnt = dbh.Db.Updateable<F2.user>().ReSetValue(ii => ii.bdshl == (ii.bdshl + shopItem.price)).UpdateColumns(ii => ii.bdshl).Where(ii => ii.uuid == shopItem.uuid).ExecuteCommand();
                    if (optCnt != 1)
                    {
                        dbh.Db.RollbackTran();
                        ResObj["message"] = "购买失败！";
                        return ResObj;
                    }
                    dbh.Db.CommitTran();
                }
                catch (Exception)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "购买失败！";
                    return ResObj;
                }

                account.shl -= shopItem.price;

                ResObj["data"] = new JObject();
                var a = new JObject();
                a["buyer_uuid"] = shopItem.buyer_uuid ?? "";
                a["created_at"] = shopItem.created_at.AsTimestamp();
                a["data"] = shopItem.data;
                a["id"] = shopItem.id;
                a["item_id"] = shopItem.item_id;
                a["item_name"] = shopItem.item_name ?? "";
                a["price"] = shopItem.price;
                a["updated_at"] = shopItem.updated_at.AsTimestamp();
                a["uuid"] = shopItem.uuid;

                ResObj["data"]["item"] = a;
                ResObj["data"]["ling"] = account.shl;

                ResObj["code"] = 0;
                ResObj["type"] = 21;
                return ResObj;
            }
        }
        [HttpPost("api/v2/shops/buy_native_shop")]
        public JObject shops_buy_native_shop([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;

            int num = value["num"].ToString().AsInt();
            F2.user account = getUserFromCache();
            if (account.bdshl < num)
            {
                ResObj["message"] = "商会令不足！";
                return ResObj;
            }
            var dbh = DbContext.Get();
            int optCnt = dbh.Db.Updateable<F2.user>().ReSetValue(ii => ii.bdshl == (ii.bdshl - num)).UpdateColumns(ii => ii.bdshl).Where(ii => ii.id == account.id && ii.bdshl >= num).ExecuteCommand();
            if (optCnt != 1)
            {
                ResObj["message"] = "购买失败！";
                return ResObj;
            }
            ResObj["data"] = new JObject();
            ResObj["data"]["ling"] = account.shl;
            ResObj["data"]["binding_ling"] = account.bdshl;
            ResObj["code"] = 0;
            ResObj["type"] = 24;
            return ResObj;
        }
        [HttpPost("api/v2/shops/recaption_item")]
        public JObject sects_recaption_item([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;

            ResObj["data"] = new JArray();

            //ResObj["code"] = 0;
            //ResObj["type"] = 41;
            return ResObj;
        }
        [HttpPost("api/v3/shops/buy_quicken_item")]
        public JObject shops_buy_quicken_item([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            int num = value["num"].ToString().AsInt();
            F2.user account = getUserFromCache();
            if (account.bdshl < num)
            {
                ResObj["message"] = "商会令不足！";
                return ResObj;
            }
            var dbh = DbContext.Get();
            int optCnt = dbh.Db.Updateable<F2.user>()
                .SetColumns(ii => ii.bdshl == (ii.bdshl - num))
                .Where(ii => ii.id == account.id && ii.bdshl >= num).ExecuteCommand();
            if (optCnt != 1)
            {
                ResObj["message"] = "购买失败！";
                return ResObj;
            }

            ResObj["data"] = new JObject(
                new JProperty("start_time", DateTime.Now.AddHours(8).AsTimestamp()),
                new JProperty("end_time", DateTime.Now.AddDays(3).AddHours(8).AsTimestamp())
            );

            ResObj["code"] = 0;
            ResObj["type"] = 99;
            return ResObj;
        }
        #endregion 商会

        #region 宗门

        [HttpPost("api/v2/sects/info")]
        public JObject sects_info([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            var dbh = DbContext.Get();
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.sectId == 0)
            {
                //查列表
                ResObj = sects_list(value);
                ResObj["type"] = 41;
                return ResObj;
            }
            else
            {
                F2.sects sect = dbh.GetEntityDB<F2.sects>().GetById(sectMember.sectId);
                if (sect == null)
                {
                    sectMember.sectId = 0;
                    dbh.Db.Updateable(sectMember).UpdateColumns(ii=>ii.sectId).ExecuteCommand();

                    //查列表
                    ResObj = sects_list(value);
                    ResObj["type"] = 41;
                    return ResObj;
                }
                //宗门信息
                var sectInfo = new JObject();
                sectInfo["advanced_smelt_count"] = sectMember.smelt_count;
                sectInfo["boss_level"] = sect.boss_level;
                sectInfo["contribution"] = sectMember.contribution;
                sectInfo["donate"] = sectMember.donateCnt > 0 ? 1 : 0;
                sectInfo["is_creator"] = sect.creator_id == account.id ? 1 : 0;
                sectInfo["join_time"] = sectMember.join_time.AsTimestamp();
                sectInfo["leader_name"] = sect.leader_name;
                sectInfo["max_count"] = sect.max_count;
                sectInfo["member_count"] = sect.member_count;
                sectInfo["new_message_id"] = sectMember.new_message_id;
                sectInfo["position_level"] = sectMember.position_level;
                sectInfo["sect_coin"] = sectMember.sect_coin;
                sectInfo["smelt_count"] = sectMember.smelt_count;
                sectInfo["today_boss_killed"] = sect.boss_HP <= 0;
                sectInfo["tomorrow_position_level"] = sectMember.position_level;
                sectInfo["sect_info"] = new JObject(
                    new JProperty("HYJF", sectMember.HYJF),
                    new JProperty("playerlv", sectMember.playerlv),
                    new JProperty("playerName", sectMember.playerName),
                    new JProperty("uuid", sectMember.playerUuid));
                sectInfo["sect"] = new JObject(
                   new JProperty("beast_level", sect.beast_level),
                   new JProperty("boss_level", sect.boss_level),
                   new JProperty("capital", sect.capital),
                   new JProperty("created_at", sect.created_at),
                   new JProperty("creator_id", sect.creator_id),
                   new JProperty("dange_level", sect.dange_level),
                   new JProperty("danqi", sect.danqi),
                   new JProperty("dimension_door", 1),
                   new JProperty("id", sect.id),
                   new JProperty("last_login_time", sectMember.last_login_time.AddDays(2).AsTimestamp()),
                   new JProperty("level", sect.level),
                   new JProperty("library_level", sect.library_level),
                   new JProperty("message_board_config", sectMember.message_board_config?? "{\"limit_count\":20,\"max_count\":30,\"min_count\":10,\"max_word_size\":30,\"min_word_size\":10,\"position_level\":7}"),
                   new JProperty("name", sect.name),
                   new JProperty("smelt_level", sect.smelt_level),
                   new JProperty("updated_at", sect.updated_at),
                   new JProperty("uuid", sect.uuid)
                   );
                if (!string.IsNullOrEmpty(sect.remain_dimension_boss_skill))
                {
                    sectInfo["dimension_boss_skill"] = (JArray)JsonConvert.DeserializeObject(sect.remain_dimension_boss_skill);
                }
                else
                {
                    sectInfo["dimension_boss_skill"] = new JArray();
                    sect.remain_dimension_boss_skill = "[]";
                    dbh.Db.Updateable(sect).UpdateColumns(ii => ii.remain_dimension_boss_skill).ExecuteCommand();
                }
                sectInfo["today_dimension_boss_killed"] = sect.remain_dimension_boss_hp <= 0;
                sectInfo["today_dimension_call"] = sect.remain_dimension_boss_hp > 0;
                ResObj["data"] = sectInfo;
            }
            ResObj["code"] = 0;
            ResObj["message"] = 1;
            ResObj["type"] = 41;
            return ResObj;
        }
        [HttpPost("api/v2/sects/list")]
        public JObject sects_list([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            int page = value["page"].AsInt();
            int per = value["per"].AsInt(30);
            if (page < 1)
            {
                page = 1;
            }
            var dbh = DbContext.Get();
            List<F2.sects> sects = dbh.Db.Queryable<F2.sects>().OrderBy(ii=>ii.id, SqlSugar.OrderByType.Desc).ToPageList(page, per);
            var list = new JArray();            
            foreach (var item in sects)
            {
                var a = new JObject();
                a["beast_level"] = item.beast_level;
                a["boss_level"] = item.boss_level;
                a["capital"] = item.capital;
                a["dange_level"] = item.dange_level;
                a["danqi"] = item.danqi;
                a["id"] = item.id;
                a["leader_name"] = item.leader_name;
                a["level"] = item.level;
                a["library_level"] = item.library_level;
                a["max_count"] = item.max_count;
                a["member_count"] = item.member_count;
                a["name"] = item.name;
                a["uuid"] = item.uuid;
                list.Add(a);
            }
            ResObj["data"] = list;
            ResObj["code"] = 0;
            ResObj["type"] = 29;
            return ResObj;
        }
        [HttpPost("api/v2/sects/join")]
        public JObject sects_join([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            var dbh = DbContext.Get();
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            int sect_id = value["sect_id"].AsInt();
            if (sect_id < 5)
            {

                sectMember.sectId = sect_id;
                sectMember.position_level = 7;
                dbh.Db.Updateable(sectMember).UpdateColumns(ii => new { ii.sectId, ii.position_level }).ExecuteCommand();
                ResObj["message"] = "你已成功加入宗门！";
                return ResObj;
            }
            else
            if (dbh.Db.Queryable<F2.sect_joinRequest>().Count(ii => ii.sectId == 0 && ii.playerId == sectMember.playerId) == 0)
            {
                //没有有宗门则提交申请
                F2.sects sect = dbh.GetEntityDB<F2.sects>().GetById(sect_id);
                if (sect.creator_id == account.id)
                {
                    //自己创建的宗门，也直接进
                    sectMember.sectId = sect_id;
                    sectMember.position_level = 0;
                    dbh.Db.Updateable(sectMember).UpdateColumns(ii => new { ii.sectId, ii.position_level }).ExecuteCommand();
                    ResObj["message"] = "你已成功加入宗门！";
                    return ResObj;
                }
                F2.sect_joinRequest request = new F2.sect_joinRequest();
                request.create_at = DateTime.Now;
                request.HYJF = sectMember.HYJF;
                request.playerId = sectMember.playerId;
                request.playerName = sectMember.playerName;
                request.playerUuid = sectMember.playerUuid;
                request.playerlv = sectMember.playerlv;
                request.sectId = sect_id;
                dbh.Db.Insertable(request).ExecuteCommand();
            }
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 37;
            return ResObj;
        }
        [HttpPost("api/v2/sects")]
        public JObject sects_create([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            string name = value["name"].ToString().Trim();
            var dbh = DbContext.Get();
            if (dbh.GetEntityDB<F2.sects>().Count(ii => ii.name == name) > 0)
            {
                ResObj["message"] = "宗门名字已经存在!";
                return ResObj;
            }
            F2.user account = getUserFromCache();

            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.sectId > 0)
            {
                ResObj["message"] = "玩家已加入其他门派";
                return ResObj;
            }
            F2.sects sect = new F2.sects();
            sect.uuid = Guid.NewGuid().ToString();
            sect.name = name;
            sect.creator_id = account.id;
            sect.leader_uuid = account.uuid;
            sect.leader_name = account.username;//((JObject)JsonConvert.DeserializeObject(account.player_data))["playerDict"]?["playName"]?.ToString()??account.username;
            sect.max_count = 31;
            sect.member_count = 1;
            sect.boss_level = 1;
            sect.created_at = DateTime.Now;
            sect.updated_at = DateTime.Now;
            sect.id = dbh.Db.Insertable(sect).ExecuteReturnIdentity();
            sect.boss_HP = 1000000;
            sect.remain_dimension_boss_CankillCnt = 50;
            sect.boss_CankillCnt = 50;

            sectMember.join_time = DateTime.Now;
            sectMember.last_login_time = DateTime.Now;
            sectMember.position_level = 0;
            sectMember.sectId = sect.id;
            dbh.Db.Updateable(sectMember).ExecuteCommand();

            ResObj["data"] = new JObject();
            ResObj["data"]["sect"] = new JObject(
                new JProperty("level", 0), 
                new JProperty("name", name),
                new JProperty("uuid", sect.uuid)
            );
            ResObj["data"]["user_sect"] = new JObject(
                new JProperty("position_level", 0), 
                new JProperty("sect_id", sect.id), 
                new JProperty("sect_uuid", sect.uuid),
                new JProperty("user_id", account.id),
                new JProperty("user_uuid", account.uuid)
            );
            ResObj["code"] = 0;
            ResObj["type"] = 28;
            return ResObj;
        }
        [HttpPost("api/v2/sects/member_list")]
        public JObject sects_member_list([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            var dbh = DbContext.Get();
            List<F2.sect_member> members = dbh.Db.Queryable<F2.sect_member>().Where(ii => ii.sectId == sectMember.sectId).ToList();
            var list = new JArray();
            foreach (var item in members)
            {
                list.Add(
                     new JObject(
                         new JProperty("contribution", item.contribution),
                         new JProperty("last_login_time", item.last_login_time.AsTimestamp()),
                         new JProperty("position_level", item.position_level),
                         new JProperty("sect_info", new JObject(
                             new JProperty("HYJF", item.HYJF),
                             new JProperty("playerlv", item.playerlv),
                             new JProperty("playerName", item.playerName),
                             new JProperty("uuid", item.playerUuid)
                             )
                         )
                     )
                 );
            }
            ResObj["data"] = list;
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 32;
            return ResObj;
        }
        [HttpPost("api/v2/sects/join_list")]
        public JObject sects_join_list([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            var dbh = DbContext.Get();
            List<F2.sect_joinRequest> members = dbh.Db.Queryable<F2.sect_joinRequest>().Where(ii => ii.sectId == sectMember.sectId).ToList();
            var list = new JArray();
            foreach (var item in members)
            {
                list.Add(new JObject(
                    new JProperty("HYJF", item.HYJF),
                    new JProperty("playerlv", item.playerlv),
                    new JProperty("playerName", item.playerName),
                    new JProperty("uuid", item.playerUuid)
                    )
                );
            }
            ResObj["data"] = list;
            ResObj["message"] = "success";
            ResObj["code"] = 0;
            ResObj["type"] = 42;
            return ResObj;
        }
        [HttpPost("api/v2/sects/agreed_join")]
        public JObject sects_agreed_join([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            string uuid = value["player_uuid"].ToString();
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.position_level != 0 && sectMember.position_level != 1)
            {
                ResObj["message"] = "权限不足";
                return ResObj;
            }
            var dbh = DbContext.Get();
            F2.sect_joinRequest members = dbh.Db.Queryable<F2.sect_joinRequest>().Where(ii => ii.sectId == sectMember.sectId && ii.playerUuid == uuid).First();
            if (members == null )
            {
                ResObj["message"] = "玩家已加入其他门派";
                return ResObj;
            }
            dbh.Db.Deleteable(members).ExecuteCommand();
            dbh.Db.BeginTran();
            try
            {
                int optCnt = dbh.Db.Updateable<F2.sect_member>(
                    new F2.sect_member { sectId = sectMember.sectId, position_level = 7, join_time = DateTime.Now, danqi = 0, contribution = 0 })
                    .UpdateColumns(ii => new { ii.sectId, ii.join_time, ii.position_level, ii.danqi, ii.contribution })
                    .Where(ii => ii.playerUuid == uuid && ii.sectId == 0).ExecuteCommand();
                if (optCnt!=1)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "玩家已加入其他门派";
                    return ResObj;
                }
                dbh.Db.CommitTran();
            }
            catch (Exception)
            {
                dbh.Db.RollbackTran();
                throw;
            }
            ResObj["message"] = "success";
            ResObj["code"] = 0;
            ResObj["type"] = 38;
            return ResObj;
        }
        [HttpPost("api/v2/sects/reject_join")]
        public JObject sects_reject_join([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            string uuid = value["player_uuid"].ToString();
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.position_level != 0 && sectMember.position_level != 1)
            {
                ResObj["message"] = "权限不足";
                return ResObj;
            }
            var dbh = DbContext.Get();
            dbh.Db.Deleteable<F2.sect_joinRequest>().Where(ii => ii.sectId == sectMember.sectId && ii.playerUuid == uuid).ExecuteCommand();

            ResObj["message"] = "success";
            ResObj["code"] = 0;
            ResObj["type"] = 39;
            return ResObj;
        }
        [HttpPost("api/v2/sects/clear_join_list")]
        public JObject sects_clear_join_list([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.position_level != 0 && sectMember.position_level != 1)
            {
                ResObj["message"] = "权限不足";
                return ResObj;
            }
            var dbh = DbContext.Get();
            dbh.Db.Deleteable<F2.sect_joinRequest>().Where(ii => ii.sectId == sectMember.sectId).ExecuteCommand();

            ResObj["message"] = "success";
            ResObj["code"] = 0;
            ResObj["type"] = 40;
            return ResObj;
        }
        [HttpPost("api/v3/sects/promotion")]
        public JObject sects_promotion([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;

            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.position_level != 0 && sectMember.position_level != 1)
            {
                ResObj["message"] = "权限不足";
                return ResObj;
            }
            var dbh = DbContext.Get();
            string uuid = value["player_uuid"].ToString();
            if (uuid == account.uuid)
            {
                ResObj["message"] = "无法调整自己的职位";
                return ResObj;
            }
            int level = value["level"].AsInt();
            F2.sect_member member = dbh.GetEntityDB<F2.sect_member>().GetSingle(ii => ii.playerUuid == uuid && ii.sectId == sectMember.sectId);
            if (member!=null)
            {
                member.position_level = level;
                dbh.Db.Updateable(member).UpdateColumns(ii => ii.position_level).ExecuteCommand();

                ResObj["data"] = new JObject(
                    new JProperty("contribution", member.contribution),                    
                    new JProperty("danqi", member.danqi),
                    new JProperty("id", member.id),
                    new JProperty("position_level", member.position_level),
                    new JProperty("sect_id", member.sectId),
                    new JProperty("user_id", member.playerId),
                    new JProperty("user_uuid", member.playerUuid)
               );
            }

            ResObj["message"] = "success";
            ResObj["code"] = 0;
            ResObj["type"] = 49;
            return ResObj;
        }
        [HttpPost("api/v3/sects/demotion")]
        public JObject sects_demotion([FromBody] JObject value)
        {
            JObject ResObj = sects_promotion(value);
            ResObj["type"] = 50;
            return ResObj;
        }
        [HttpPost("api/v2/sects/kickout")]
        public JObject sects_kickout([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;

            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.position_level != 0 && sectMember.position_level != 1)
            {
                ResObj["message"] = "权限不足";
                return ResObj;
            }
            var dbh = DbContext.Get();
            string uuid = value["user_uuid"].ToString();

            dbh.Db.Updateable<F2.sect_member>(
                   new F2.sect_member { sectId = 0})
                   .UpdateColumns(ii => new { ii.sectId})
                   .Where(ii => ii.playerUuid == uuid && ii.sectId == sectMember.sectId).ExecuteCommand();

            ResObj["message"] = "success";
            ResObj["code"] = 0;
            ResObj["type"] = 31;
            return ResObj;
        }
        [HttpPost("api/v2/sects/quit")]
        public JObject sects_quit([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;

            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.position_level == 0)
            {
                ResObj["message"] = "掌门无法退出！";
                return ResObj;
            }
            var dbh = DbContext.Get();
            dbh.Db.Updateable<F2.sect_member>(
                   new F2.sect_member { sectId = 0 })
                   .UpdateColumns(ii => new { ii.sectId })
                   .Where(ii => ii.playerUuid == account.uuid).ExecuteCommand();

            ResObj["message"] = "success";
            ResObj["code"] = 0;
            ResObj["type"] = 30;
            return ResObj;
        }
        [HttpPost("api/v3/sects/send_message")]
        public JObject sects_send_message([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);

            F2.sectMessage message = new F2.sectMessage();
            message.content = value["content"].ToString();
            message.create_at = DateTime.Now;
            message.sectId = sectMember.sectId;            
            message.playerName = sectMember.playerName;
            message.playerUuid = sectMember.playerUuid;

            var dbh = DbContext.Get();
            dbh.Db.Insertable(message).ExecuteCommand();

            var list = new JArray();
            list.Add(new JObject(
                         new JProperty("content", message.content),
                         new JProperty("created_at", message.create_at),
                         new JProperty("id", message.id),
                         new JProperty("player_name", message.playerName),
                         new JProperty("sect_id", message.sectId),
                         new JProperty("updated_at", message.create_at),
                         new JProperty("uuid", message.playerUuid)
                     )
            );
            ResObj["data"]= list;
            ResObj["message"] = "success";
            ResObj["code"] = 0;
            ResObj["type"] = 58;
            return ResObj;
        }

        [HttpPost("api/v3/sects/get_message")]
        public JObject sects_get_message([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;            
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            var message_board_config = (JObject)JsonConvert.DeserializeObject(sectMember.message_board_config);
            var dbh = DbContext.Get();
            List<F2.sectMessage> members = dbh.Db.Queryable<F2.sectMessage>()
                .Take(message_board_config["limit_count"].AsInt(20))
                .Where(ii => ii.sectId == sectMember.sectId)
                .OrderBy(ii=>ii.id, SqlSugar.OrderByType.Desc).ToList();
            var list = new JArray();
            foreach (var item in members)
            {
                list.Add(new JObject(
                    new JProperty("content", item.content),
                    new JProperty("created_at", item.create_at),
                    new JProperty("id", item.id),
                    new JProperty("player_name", item.playerName),
                    new JProperty("sect_id", item.sectId),
                    new JProperty("updated_at", item.create_at),
                    new JProperty("uuid", item.playerUuid)
                    )
                );
            }
            ResObj["data"] = new JObject(new JProperty("message_config", message_board_config), new JProperty("messages", list));            
            ResObj["message"] = "success";
            ResObj["code"] = 0;
            ResObj["type"] = 59;
            return ResObj;
        }

        [HttpPost("api/v3/sects/dange_info")]
        public JObject sects_dange_info([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            var dbh = DbContext.Get();
            F2.sects sect = dbh.GetEntityDB<F2.sects>().GetById(sectMember.sectId);
            ResObj["data"] = new JObject(new JProperty("dange_level", sect.dange_level), new JProperty("danqi", sect.danqi));
            ResObj["code"] = 0;
            ResObj["type"] = 48;
            return ResObj;
        }
        [HttpPost("api/v2/sects/upgrade")]
        public JObject sects_upgrade([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.position_level != 0 && sectMember.position_level != 1)
            {
                ResObj["message"] = "权限不足";
                return ResObj;
            }
            var dbh = DbContext.Get();
            dbh.Db.BeginTran();
            F2.sects sect = null;
            try
            {
                sect = dbh.GetEntityDB<F2.sects>().AsQueryable().With(SqlSugar.SqlWith.HoldLock).Single(ii => ii.id == sectMember.sectId);
                if (sect.level < 29)
                {
                    sect.level++;
                }
                sect.max_count++;
                int needCapital = sect.level * 300000;
                if (sect.capital < needCapital)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "资金不足";
                    return ResObj;
                }
                sect.capital -= needCapital;
                
                dbh.Db.Updateable<F2.sects>(sect).ExecuteCommand();
                dbh.Db.CommitTran();
            }
            catch (Exception)
            {
                dbh.Db.RollbackTran();
                ResObj["message"] = "更新出错！";
                return ResObj;
            }
            ResObj["data"] = new JObject(
                        new JProperty("beast_level", sect.beast_level),
                        new JProperty("boss_level", sect.boss_level),
                        new JProperty("capital", sect.capital),
                        new JProperty("dange_level", sect.dange_level),
                        new JProperty("danqi", sect.danqi),
                        new JProperty("dimension_door", 1),
                        new JProperty("id", sect.id),
                        new JProperty("join_time", sect.created_at.AsTimestamp()),
                        new JProperty("leader_name", sect.leader_name),
                        new JProperty("level", sect.level),
                        new JProperty("library_level", sect.library_level),
                        new JProperty("max_count", sect.max_count),
                        new JProperty("member_count", sect.member_count),
                        new JProperty("name", sect.name),
                        new JProperty("uuid", sect.uuid)
                    );
            ResObj["message"] = "success";
            ResObj["code"] = 0;
            ResObj["type"] = 36;
            return ResObj;
        }
        [HttpPost("api/v3/sects/upgrade_smelt")]
        public JObject sects_upgrade_smelt([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.position_level != 0 && sectMember.position_level != 1)
            {
                ResObj["message"] = "权限不足";
                return ResObj;
            }
            var dbh = DbContext.Get();
            dbh.Db.BeginTran();
            F2.sects sect = null;
            try
            {
                sect = dbh.GetEntityDB<F2.sects>().AsQueryable().With(SqlSugar.SqlWith.HoldLock).Single(ii => ii.id == sectMember.sectId);
                if (sect.smelt_level >= 29)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "已到满级";
                    return ResObj;
                }
                sect.smelt_level++;
                int needCapital = sect.smelt_level * 100000;
                if (sect.capital< needCapital)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "资金不足";
                    return ResObj;
                }
                sect.capital -= needCapital;
                dbh.Db.Updateable<F2.sects>(sect).ExecuteCommand();
                dbh.Db.CommitTran();
            }
            catch (Exception)
            {
                dbh.Db.RollbackTran();
                ResObj["message"] = "更新出错！";
                return ResObj;
            }
            ResObj["data"] = new JObject(
                new JProperty("beast_level", sect.beast_level), 
                new JProperty("boss_level", sect.boss_level),
                new JProperty("capital", sect.capital),
                new JProperty("dange_level", sect.dange_level),
                new JProperty("danqi", sect.danqi),
                new JProperty("dimension_door", 1),
                new JProperty("id", sect.id),
                new JProperty("join_time", sect.created_at.AsTimestamp()),
                new JProperty("leader_name", sect.leader_name),
                new JProperty("level", sect.level),
                new JProperty("library_level", sect.library_level),
                new JProperty("max_count", sect.max_count),
                new JProperty("member_count", sect.member_count),
                new JProperty("name", sect.name),
                new JProperty("uuid", sect.uuid),
                new JProperty("user", new JObject(new JProperty("sect_coin", sectMember.sect_coin)))
            );
            ResObj["message"] = "success";
            ResObj["code"] = 0;
            ResObj["type"] = 64;
            return ResObj;
        }
        [HttpPost("api/v3/sects/upgrade_dange")]
        public JObject sects_upgrade_dange([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.position_level != 0 && sectMember.position_level != 1)
            {
                ResObj["message"] = "权限不足";
                return ResObj;
            }
            var dbh = DbContext.Get();
            dbh.Db.BeginTran();
            F2.sects sect = null;
            try
            {
                sect = dbh.GetEntityDB<F2.sects>().AsQueryable().With(SqlSugar.SqlWith.HoldLock).Single(ii => ii.id == sectMember.sectId);
                if (sect.dange_level >= 29)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "已到满级";
                    return ResObj;
                }
                sect.dange_level++;
                int needCapital = sect.dange_level * 100000;
                if (sect.capital < needCapital)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "资金不足";
                    return ResObj;
                }
                if (sect.danqi < needCapital)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "丹气不足";
                    return ResObj;
                }
                sect.capital -= needCapital;
                sect.danqi -= needCapital;
                dbh.Db.Updateable<F2.sects>(sect).ExecuteCommand();
                dbh.Db.CommitTran();
            }
            catch (Exception)
            {
                dbh.Db.RollbackTran();
                ResObj["message"] = "更新出错！";
                return ResObj;
            }
            ResObj["data"] = new JObject(
                new JProperty("beast_level", sect.beast_level),
                new JProperty("boss_level", sect.boss_level),
                new JProperty("capital", sect.capital),
                new JProperty("dange_level", sect.dange_level),
                new JProperty("danqi", sect.danqi),
                new JProperty("dimension_door", 1),
                new JProperty("id", sect.id),
                new JProperty("join_time", sect.created_at.AsTimestamp()),
                new JProperty("leader_name", sect.leader_name),
                new JProperty("level", sect.level),
                new JProperty("library_level", sect.library_level),
                new JProperty("max_count", sect.max_count),
                new JProperty("member_count", sect.member_count),
                new JProperty("name", sect.name),
                new JProperty("uuid", sect.uuid),
                new JProperty("user", new JObject(new JProperty("sect_coin", sectMember.sect_coin)))
            );
            ResObj["message"] = "success";
            ResObj["code"] = 0;
            ResObj["type"] = 47;
            return ResObj;
        }
        [HttpPost("api/v3/sects/update_library")]
        public JObject sects_update_library([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 90;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.position_level != 0 && sectMember.position_level != 1)
            {
                ResObj["message"] = "权限不足";
                return ResObj;
            }
            var dbh = DbContext.Get();
            dbh.Db.BeginTran();
            F2.sects sect = null;
            try
            {
                sect = dbh.GetEntityDB<F2.sects>().AsQueryable().With(SqlSugar.SqlWith.HoldLock).Single(ii => ii.id == sectMember.sectId);
                if (sect.library_level >= 29)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "已到满级";
                    return ResObj;
                }
                sect.library_level++;
                int needCapital = sect.library_level * 300000;
                if (sect.capital < needCapital)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "资金不足";
                    return ResObj;
                }
                sect.capital -= needCapital;             
                dbh.Db.Updateable<F2.sects>(sect).ExecuteCommand();
                dbh.Db.CommitTran();
            }
            catch (Exception)
            {
                dbh.Db.RollbackTran();
                ResObj["message"] = "更新出错！";
                return ResObj;
            }
            ResObj["data"] = new JObject(
                new JProperty("beast_level", sect.beast_level),
                new JProperty("boss_level", sect.boss_level),
                new JProperty("capital", sect.capital),
                new JProperty("dange_level", sect.dange_level),
                new JProperty("danqi", sect.danqi),
                new JProperty("dimension_door", 1),
                new JProperty("id", sect.id),
                new JProperty("join_time", sect.created_at.AsTimestamp()),
                new JProperty("leader_name", sect.leader_name),
                new JProperty("level", sect.level),
                new JProperty("library_level", sect.library_level),
                new JProperty("max_count", sect.max_count),
                new JProperty("member_count", sect.member_count),
                new JProperty("name", sect.name),
                new JProperty("uuid", sect.uuid)                
            );
            ResObj["message"] = "success";
            ResObj["code"] = 0;
            ResObj["type"] = 90;
            return ResObj;
        }
        [HttpPost("api/v3/sects/update_beast")]
        public JObject sects_update_beast([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 91;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.position_level != 0 && sectMember.position_level != 1)
            {
                ResObj["message"] = "权限不足";
                return ResObj;
            }
            var dbh = DbContext.Get();
            dbh.Db.BeginTran();
            F2.sects sect = null;
            try
            {
                sect = dbh.GetEntityDB<F2.sects>().AsQueryable().With(SqlSugar.SqlWith.HoldLock).Single(ii => ii.id == sectMember.sectId);
                if (sect.library_level >= 29)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "已到满级";
                    return ResObj;
                }
                sect.beast_level++;
                int needCapital = sect.beast_level * 500000;
                if (sect.capital < needCapital)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "资金不足";
                    return ResObj;
                }
                sect.capital -= needCapital;
                dbh.Db.Updateable<F2.sects>(sect).ExecuteCommand();
                dbh.Db.CommitTran();
            }
            catch (Exception)
            {
                dbh.Db.RollbackTran();
                ResObj["message"] = "更新出错！";
                return ResObj;
            }
            ResObj["data"] = new JObject(
                new JProperty("beast_level", sect.beast_level),
                new JProperty("boss_level", sect.boss_level),
                new JProperty("capital", sect.capital),
                new JProperty("dange_level", sect.dange_level),
                new JProperty("danqi", sect.danqi),
                new JProperty("dimension_door", 1),
                new JProperty("id", sect.id),
                new JProperty("join_time", sect.created_at.AsTimestamp()),
                new JProperty("leader_name", sect.leader_name),
                new JProperty("level", sect.level),
                new JProperty("library_level", sect.library_level),
                new JProperty("max_count", sect.max_count),
                new JProperty("member_count", sect.member_count),
                new JProperty("name", sect.name),
                new JProperty("uuid", sect.uuid)              
            );
            ResObj["message"] = "success";
            ResObj["code"] = 0;
            ResObj["type"] = 91;
            return ResObj;
        }


        [HttpPost("api/v2/sects/award")]
        public JObject sects_award([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.awardCnt > 1)
            {
                ResObj["message"] = "今天已经领取过福利";
                return ResObj;
            }
            var dbh = DbContext.Get();
            var sect = dbh.GetEntityDB<F2.sects>().GetById(sectMember.sectId);
            int lqsl = (7 - sectMember.position_level) * (3 + sect.level) * 10 + 300;
            dbh.Db.UseTran(() => {
                //sectMember = dbh.GetEntityDB<F2.sect_member>().AsQueryable().With(SqlSugar.SqlWith.HoldLock).Single(ii => ii.id == sectMember.id);
                //sectMember.sect_coin += (7 - sectMember.position_level) * 30 + 200;
                //sectMember.awardCnt++;
                //dbh.Db.Updateable(sectMember).UpdateColumns(ii=>new { ii.awardCnt, ii.sect_coin }).ExecuteCommand();
                dbh.Db.Updateable<F2.sect_member>()
                    .ReSetValue(ii => ii.sect_coin == ii.sect_coin + lqsl)
                    .ReSetValue(ii => ii.awardCnt == ii.awardCnt + 1)
                    .UpdateColumns(ii => new { ii.sect_coin, ii.awardCnt})
                    .Where(ii => ii.id == sectMember.id).ExecuteCommand();
            });
            ResObj["data"] = new JObject(new JProperty("sect_coin", lqsl));
            ResObj["message"] = "success";
            ResObj["code"] = 0;
            ResObj["type"] = 35;
            return ResObj;
        }
        [HttpPost("api/v2/sects/donate")]
        public JObject sects_donate([FromBody] JObject value)
        {
            var dbh = DbContext.Get();
            int count = value["count"].AsInt();
            F2.user account = getUserFromCache();
            account.player_data = value["player_data"].ToString();
            account.player_zhong_yao = value["player_zhong_yao"].ToString();
            
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.donateCnt > 1)
            {
                JObject ResObj2 = new JObject();
                ResObj2["code"] = 1;
                ResObj2["type"] = 1;
                ResObj2["message"] = "今天已经捐赠 1 次！";
                return ResObj2;
            }
            dbh.Db.BeginTran();
            try
            {
                dbh.Db.Updateable(account).UpdateColumns(ii => new { ii.player_data, ii.player_zhong_yao }).ExecuteCommand();

                dbh.Db.Updateable<F2.sect_member>()
                    .ReSetValue(ii => ii.donateCnt == ii.donateCnt + 1)
                    .ReSetValue(ii => ii.sect_coin == ii.sect_coin + count / 10)
                    .ReSetValue(ii => ii.contribution == ii.contribution + count)
                    .UpdateColumns(ii => new { ii.donateCnt, ii.sect_coin, ii.contribution })
                    .Where(ii => ii.id == sectMember.id).ExecuteCommand();

                dbh.Db.Updateable<F2.sects>()
                    .ReSetValue(ii => ii.capital == ii.capital + count)
                    .ReSetValue(ii => ii.donate == ii.donate + 1)
                    .UpdateColumns(ii => new { ii.capital, ii.donate })
                    .Where(ii => ii.id == sectMember.sectId).ExecuteCommand();
                dbh.Db.CommitTran();
            }
            catch (Exception)
            {
                dbh.Db.RollbackTran();
                return null;
            }
            JObject ResObj = sects_info(value);
            ResObj["type"] = 34;
            return ResObj;
        }

        [HttpPost("api/v3/sects/add_dan_qi")]
        public JObject sects_add_dan_qi([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            int count = value["count"].AsInt();
            F2.user account = getUserFromCache();
            account.player_data = value["player_data"].ToString();
            account.player_zhong_yao = value["player_zhong_yao"].ToString();
            var dbh = DbContext.Get();
            F2.sect_member sectMember = updateSectInfo(account);
            dbh.Db.UseTran(() =>
            {
                dbh.Db.Updateable<F2.sect_member>()
                    .ReSetValue(ii => ii.contribution == ii.contribution + (count/10))
                    .ReSetValue(ii => ii.danqi == ii.danqi + count)
                    .UpdateColumns(ii => new { ii.contribution,ii.danqi })
                    .Where(ii => ii.id == sectMember.id).ExecuteCommand();

                dbh.Db.Updateable<F2.sects>()
                    .ReSetValue(ii => ii.danqi == ii.danqi + count)
                    .UpdateColumns(ii => new { ii.danqi })
                    .Where(ii => ii.id == sectMember.sectId).ExecuteCommand();
            });
            var sect = dbh.GetEntityDB<F2.sects>().GetById(sectMember.sectId);
            ResObj["data"] = new JObject(
                new JProperty("beast_level", sect.beast_level),
                new JProperty("boss_level", sect.boss_level),
                new JProperty("capital", sect.capital),
                new JProperty("dange_level", sect.dange_level),
                new JProperty("danqi", sect.danqi),
                new JProperty("dimension_door", 1),
                new JProperty("id", sect.id),
                new JProperty("join_time", sect.created_at.AsTimestamp()),
                new JProperty("leader_name", sect.leader_name),
                new JProperty("level", sect.level),
                new JProperty("library_level", sect.library_level),
                new JProperty("max_count", sect.max_count),
                new JProperty("member_count", sect.member_count),
                new JProperty("name", sect.name),
                new JProperty("uuid", sect.uuid)               
            );
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 46;
            return ResObj;
        }
        
        [HttpPost("api/v3/sects/danqi_rank")]
        public JObject sects_danqi_rank([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            int count = value["count"].AsInt();
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            var dbh = DbContext.Get();
            List<F2.sect_member> members = dbh.Db.Queryable<F2.sect_member>()
                .Where(ii => ii.sectId == sectMember.sectId)
                .OrderBy(ii => ii.danqi, SqlSugar.OrderByType.Desc).ToList();
            var list = new JArray();
            foreach (var item in members)
            {
                list.Add(new JObject(
                    new JProperty("contribution", item.contribution),
                    new JProperty("danqi", item.danqi),
                    new JProperty("player_name", item.playerName),
                    new JProperty("id", item.id),
                    new JProperty("position_level", item.position_level),
                    new JProperty("status", 0),
                    new JProperty("user_id", item.playerId),
                    new JProperty("user_uuid", item.playerUuid)
                    )
                );
            }
            ResObj["data"] = list;
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 45;
            return ResObj;
        }
        [HttpPost("api/v2/sects/use_sect_coin")]
        public JObject sects_use_sect_coin([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            int num = value["num"].AsInt();
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.sect_coin< num)
            {
                ResObj["message"] = "宗门币不足！";
                return ResObj;
            }

            var dbh = DbContext.Get();
            dbh.Db.BeginTran();
            try
            {
                int optcnt = dbh.Db.Updateable<F2.sect_member>()
                     .ReSetValue(ii => ii.sect_coin == ii.sect_coin - num)
                     .UpdateColumns(ii => new { ii.sect_coin })
                     .Where(ii => ii.id == sectMember.id && ii.sect_coin>=num).ExecuteCommand();
                if (optcnt != 1)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "宗门币不足！";
                    return ResObj;
                }
                dbh.Db.CommitTran();
            }
            catch (Exception)
            {
                dbh.Db.RollbackTran();
                ResObj["message"] = "宗门币不足！";
                return ResObj;
            }

            ResObj["data"] = new JObject(new JProperty("sect_coin", sectMember.sect_coin - num));
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 44;
            return ResObj;
        }
        //高级炼制
        [HttpPost("api/v4/sects/advanced_smelt")]
        public JObject sects_advanced_smelt([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;            
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.sect_coin < 抽奖费用*10)
            {
                ResObj["message"] = "宗门币不足！";
                return ResObj;
            }

            var dbh = DbContext.Get();
            dbh.Db.BeginTran();
            try
            {
                int optcnt = dbh.Db.Updateable<F2.sect_member>()
                     .ReSetValue(ii => ii.sect_coin == ii.sect_coin - 抽奖费用*10)
                     .ReSetValue(ii => ii.smelt_count == ii.smelt_count + 10)
                     .UpdateColumns(ii => new { ii.sect_coin, ii.smelt_count })
                     .Where(ii => ii.id == sectMember.id && ii.sect_coin >= 抽奖费用*10).ExecuteCommand();
                if (optcnt != 1)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "宗门币不足！";
                    return ResObj;
                }
                dbh.Db.CommitTran();
            }
            catch (Exception)
            {
                dbh.Db.RollbackTran();
                ResObj["message"] = "宗门币不足！";
                return ResObj;
            }

            ResObj["data"] = new JObject(
                new JProperty("sect_coin", sectMember.sect_coin - 抽奖费用), 
                new JProperty("id", 0), 
                new JProperty("advanced_smelt_count", sectMember.smelt_count + 10),
                new JProperty("reward_item_info", 抽奖Android(10,ref account))
            );
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 87;
            return ResObj;
        }
        [HttpPost("api/v4/sects/advanced_smelt_ten_times")]
        public JObject sects_advanced_smelt_ten_times([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;            
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.sect_coin < 抽奖费用*100)
            {
                ResObj["message"] = "宗门币不足！";
                return ResObj;
            }

            var dbh = DbContext.Get();
            dbh.Db.BeginTran();
            try
            {
                int optcnt = dbh.Db.Updateable<F2.sect_member>()
                     .ReSetValue(ii => ii.sect_coin == ii.sect_coin - 抽奖费用 * 100)
                     .ReSetValue(ii => ii.smelt_count == ii.smelt_count + 100)
                     .UpdateColumns(ii => new { ii.sect_coin, ii.smelt_count })
                     .Where(ii => ii.id == sectMember.id && ii.sect_coin >= 抽奖费用 * 100).ExecuteCommand();
                if (optcnt != 1)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "宗门币不足！";
                    return ResObj;
                }
                dbh.Db.CommitTran();
            }
            catch (Exception)
            {
                dbh.Db.RollbackTran();
                ResObj["message"] = "宗门币不足！";
                return ResObj;
            }
            //JArray reward_item_info = new JArray();
            //reward_item_info.Add(new JObject(new JProperty("childType", "124"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "124"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "124"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "124"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "124"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "124"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "124"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "124"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "124"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "124"), new JProperty("itemType", "0")));

            ResObj["data"] = new JObject(
                new JProperty("sect_coin", sectMember.sect_coin - 抽奖费用 * 10), 
                new JProperty("id", 0), 
                new JProperty("advanced_smelt_count", sectMember.smelt_count + 10),
                new JProperty("reward_item_info", 抽奖Android(100, ref account))
            );
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 88;
            return ResObj;
        }
        [HttpPost("api/v4/sects/deity_smelt")]
        public JObject sects_deity_smelt([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.smelt_count < 3000)
            {
                ResObj["message"] = "高级炼制次数不足！";
                return ResObj;
            }

            var dbh = DbContext.Get();
            dbh.Db.BeginTran();
            try
            {
                int optcnt = dbh.Db.Updateable<F2.sect_member>()
                     .ReSetValue(ii => ii.smelt_count == ii.smelt_count - 3000)
                     .UpdateColumns(ii => new { ii.smelt_count })
                     .Where(ii => ii.id == sectMember.id && ii.smelt_count >= 3000).ExecuteCommand();
                if (optcnt != 1)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "宗门币不足！";
                    return ResObj;
                }
                dbh.Db.CommitTran();
            }
            catch (Exception)
            {
                dbh.Db.RollbackTran();
                ResObj["message"] = "宗门币不足！";
                return ResObj;
            }
            //JArray reward_item_info = new JArray();
            //reward_item_info.Add(new JObject(new JProperty("childType", "0"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "1"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "2"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "3"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "4"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "5"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "6"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "7"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "8"), new JProperty("itemType", "0")));
            //reward_item_info.Add(new JObject(new JProperty("childType", "9"), new JProperty("itemType", "0")));

            ResObj["data"] = new JObject(
                new JProperty("sect_coin", sectMember.sect_coin),
                new JProperty("id", 0),
                new JProperty("advanced_smelt_count", sectMember.smelt_count - 1000),
                new JProperty("reward_item_info", 抽奖Android(1, ref account, true))
            );
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 89;
            return ResObj;
        }

        private static List<string> 先天 = new List<string>() {
           "10,72",//弑神舰
            "5,67",//先天灵盔
            "5,69",//初心头冠
            "6,43",//银狐飞羽鞋
            "11,69",//九幽战皇盾
            "11,78",//清风盾
            "11,80",//试炼盾.先天
            "7,67",//先天灵府
            //"7,70",//阴镜
            //"7,71",//阳镜
            //"8,88",//先天魂铁
            "0,146",//生命神杖
            "0,190",//郭老邪的矿镐
            "1,64",//御影战甲
            "2,72",//噬魂灵戒
            "2,73",//佩奇指环
            "2,75",//普利尼心戒
            "2,78",//炼妖手套
            //"3,30",//试炼之心.先天
            "3,87",//初心项链
            "4,65",//蜡笔护镯
            "4,68",//猫咪的铃铛
            "4,71",//初心手镯
            "4,73",//炼妖狼爪手
            "8,106",//极品仙玉
        };
        private static List<string> 神器 = new List<string>() {
            "5,66",//真言神盔
            "5,68",//金神
            "6,66",//风神靴
            "11,70",//木神盾
            "7,72",//阴阳镜
            //"8,89",//通灵神铁
            "0,182",//通灵神兵
            "1,68",//通灵神甲
            "1,70",//大地神甲
            "2,71",//噬魂神戒
            "2,74",//火神指环
            "4,67",//水神手镯
            "0,193",//吴刚神斧
            "0,194",//射日弓
        };
        private static List<string> 超神器假 = new List<string>() {
            "5,73",//东皇盔.赝品
            "6,67",//九黎靴.赝品
            "11,77",//盘古盾.赝品
            "7,74",//昆仑镜.赝品
            "12,65",//神农鼎.赝品
            "0,192",//轩辕剑.赝品
            "1,72",//崆峒甲.赝品
            "2,77",//伏羲戒.赝品
            "3,89",//女娲石.赝品
            "4,70",//昊天镯.赝品
        };
        private static List<string> 超神器真 = new List<string>() {
            "5,72",//东皇盔.真品
            "6,68",//九黎靴.真品
            "11,76",//盘古盾.真品
            "7,73",//昆仑镜.真品
            "12,64",//神农鼎.真品
            "0,191",//轩辕剑.真品
            "1,71",//崆峒甲.真品
            "2,76",//伏羲戒.真品
            "3,88",//女娲石.真品
            "4,69",//昊天镯.真品
        };
        //一个宗门一天极限2000w
        const int 抽奖费用 = 10000;
        
        private string 抽奖_IOS(DbContext dbh, int cnt,ref F2.user account,bool b1 = false)
        {
            JArray reward_item_info = new JArray();
            var r = new Random();
            int tzl = 100;
            for (int i = 0; i < cnt; i++)
            {
                account.cjCnt++;
                string item = "";
                int luckNum = r.Next(0, 1000);
                if (luckNum == r.Next(0, 1000))
                {
                    //千分之一，出先天
                    item = 先天[r.Next(0, 先天.Count)];
                }
                else if (luckNum == r.Next(0, 10000))
                {
                    //万分之一，出神器
                    item = 神器[r.Next(0, 神器.Count)];
                    account.cjs += 1;                    
                }
                else if ((account.cjcs == 0 || account.cz > 600) && luckNum == r.Next(0, 100000)) //没有充值的可以抽中一个神器
                {
                    //十万分之一，超神器假
                    item = 超神器假[r.Next(0, 超神器假.Count)];
                    account.cjcs += 1;                    
                }
                else if (account.cz > 1000 && luckNum == r.Next(0, 1000000))  //充值小于 1000 的不能抽真神器
                {
                    //百万分之一，超神器真
                    item = 超神器真[r.Next(0, 超神器真.Count)];
                    account.cjcs += 1;                    
                }
                else if (i == 0 && b1)
                {
                    //保底先天
                    item = 先天[r.Next(0, 先天.Count)];
                }
                else
                {
                    //啥都没中
                    tzl += 100;
                    continue;
                }
                var iarr = item.Split(",");
                reward_item_info.Add(new JObject(new JProperty("childType", iarr[1]), new JProperty("itemType", iarr[0]), new JProperty("itemNum", 1), new JProperty("num", 1)));
            }
            reward_item_info.Add(new JObject(new JProperty("childType", "33"), new JProperty("itemType", "8"), new JProperty("itemNum", tzl), new JProperty("num", tzl)));
            dbh.Db.Updateable(account).UpdateColumns(ii => new { ii.cjcs, ii.cjs, ii.cjCnt }).ExecuteCommand();
            JObject jo = new JObject(
                new JProperty("error", 0),
                new JProperty("GETBODY", new JObject(
                    new JProperty("itemGetArr", reward_item_info)
                    ))
                );
            return jo.ToString(Formatting.None);
        }
        private static JArray 抽奖Android(int cnt, ref F2.user account, bool b1 = false)
        {
            JArray reward_item_info = new JArray();
            var r = new Random();
            int tzl = 100;
            for (int i = 0; i < cnt; i++)
            {
                account.cjCnt++;
                string item = "";
                int luckNum = r.Next(0, 1000);
                if (luckNum == r.Next(0, 1000))
                {
                    //千分之一，出先天
                    item = 先天[r.Next(0, 先天.Count)];
                }
                else if (luckNum == r.Next(0, 10000))
                {
                    //万分之一，出神器
                    item = 神器[r.Next(0, 神器.Count)];
                    account.cjs += 1;
                }
                else if ((account.cjcs == 0 || account.cz > 600) && luckNum == r.Next(0, 100000))
                {
                    //十万分之一，超神器假
                    item = 超神器假[r.Next(0, 超神器假.Count)];
                    account.cjcs += 1;
                }
                else if (account.cz > 1000 && luckNum == r.Next(0, 1000000))
                {
                    //百万分之一，超神器真
                    item = 超神器真[r.Next(0, 超神器真.Count)];
                    account.cjcs += 1;
                }
                else if (i == 0 && b1)
                {
                    //保底先天
                    item = 先天[r.Next(0, 先天.Count)];
                }
                else
                {
                    //啥都没中
                    tzl += 100;
                    continue;
                }
                var iarr = item.Split(",");
                reward_item_info.Add(new JObject(new JProperty("childType", iarr[1]), new JProperty("itemType", iarr[0])));
            }
            DbContext.Get().Db.Updateable(account).UpdateColumns(ii => new { ii.cjcs, ii.cjs, ii.cjCnt }).ExecuteCommand();
            reward_item_info.Add(new JObject(new JProperty("childType", "33"), new JProperty("itemType", "8"), new JProperty("itemNum", tzl), new JProperty("num", tzl)));
            return reward_item_info;
        }
        //高级炼制
        [HttpPost("api/v3/sects/advanced_smelt")]
        public JObject sects_v3_advanced_smelt([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.sect_coin < 抽奖费用 * 10)
            {
                ResObj["message"] = "宗门币不足！";
                return ResObj;
            }
            var dbh = DbContext.Get();
            var vdata = 抽奖_IOS(dbh, 10, ref account);
            dbh.Db.BeginTran();
            try
            {
                int optcnt = dbh.Db.Updateable<F2.sect_member>()
                     .ReSetValue(ii => ii.sect_coin == ii.sect_coin - 抽奖费用 * 10)
                     .ReSetValue(ii => ii.smelt_count == ii.smelt_count + 10)
                     .UpdateColumns(ii => new { ii.sect_coin, ii.smelt_count })
                     .Where(ii => ii.id == sectMember.id && ii.sect_coin >= 抽奖费用 * 10).ExecuteCommand();
                if (optcnt != 1)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "宗门币不足！";
                    return ResObj;
                }

                F2.giftCode gif = new F2.giftCode();
                gif.create_at = DateTime.Now;
                gif.uuid = account.uuid;
                gif.code = 1;
                gif.itemData = vdata;
                dbh.Db.Insertable(gif).ExecuteCommand();
                ResObj["message"] = "兑换码:" + gif.code.ToString();

                dbh.Db.CommitTran();
            }
            catch (Exception)
            {
                dbh.Db.RollbackTran();
                ResObj["message"] = "宗门币不足！";
                return ResObj;
            }
            //ResObj["code"] = 0;
            //ResObj["message"] = "success";
            //ResObj["type"] = 67;
            return ResObj;
        }
        [HttpPost("api/v3/sects/advanced_smelt_ten_times")]
        public JObject sects_v3_advanced_smelt_ten_times([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.sect_coin < 抽奖费用 * 100)
            {
                ResObj["message"] = "宗门币不足！";
                return ResObj;
            }
            var dbh = DbContext.Get();
            var vdata = 抽奖_IOS(dbh, 100, ref account);
            dbh.Db.BeginTran();
            try
            {
                int optcnt = dbh.Db.Updateable<F2.sect_member>()
                     .ReSetValue(ii => ii.sect_coin == ii.sect_coin - 抽奖费用 * 100)
                     .ReSetValue(ii => ii.smelt_count == ii.smelt_count + 100)
                     .UpdateColumns(ii => new { ii.sect_coin, ii.smelt_count })
                     .Where(ii => ii.id == sectMember.id && ii.sect_coin >= 抽奖费用 * 100).ExecuteCommand();
                if (optcnt != 1)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "宗门币不足！";
                    return ResObj;
                }

                F2.giftCode gif = new F2.giftCode();
                gif.create_at = DateTime.Now;
                gif.uuid = account.uuid;
                gif.code = 1;
                gif.itemData = vdata;
                dbh.Db.Insertable(gif).ExecuteCommand();
                ResObj["message"] = "兑换码:" + gif.code.ToString();

                dbh.Db.CommitTran();
            }
            catch (Exception)
            {
                dbh.Db.RollbackTran();
                ResObj["message"] = "宗门币不足！";
                return ResObj;
            }
            return ResObj;
        }
        [HttpPost("api/v3/sects/deity_smelt")]
        public JObject sects_v3_deity_smelt([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.smelt_count < 3000)
            {
                ResObj["message"] = "高级炼制次数不足！";
                return ResObj;
            }
            var dbh = DbContext.Get();
            var vdata = 抽奖_IOS(dbh, 10, ref account, true);
            dbh.Db.BeginTran();
            try
            {
                int optcnt = dbh.Db.Updateable<F2.sect_member>()
                     .ReSetValue(ii => ii.smelt_count == ii.smelt_count - 3000)
                     .UpdateColumns(ii => new { ii.smelt_count })
                     .Where(ii => ii.id == sectMember.id && ii.smelt_count >= 3000).ExecuteCommand();
                if (optcnt != 1)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "宗门币不足！";
                    return ResObj;
                }
                F2.giftCode gif = new F2.giftCode();
                gif.create_at = DateTime.Now;
                gif.uuid = account.uuid;
                gif.code = 1;
                gif.itemData = vdata;
                dbh.Db.Insertable(gif).ExecuteCommand();
                ResObj["message"] = "兑换码:" + gif.code.ToString();
                dbh.Db.CommitTran();
            }
            catch (Exception)
            {
                dbh.Db.RollbackTran();
                ResObj["message"] = "宗门币不足！";
                return ResObj;
            }

            //ResObj["code"] = 0;
            //ResObj["message"] = "success";
            //ResObj["type"] = 70;
            return ResObj;
        }

        #endregion 宗门


        #region BOSS

        [HttpPost("api/v3/sects/get_boss_info")]
        public JObject get_boss_info([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            var dbh = DbContext.Get();
            if (sectMember.AttackBossCnt >= sectMember.CanAttackBossCnt)
            {
                ResObj["message"] = "已用完挑战次数！\n可在商会中增加“Boss挑战次数”";
                return ResObj;
            }
            F2.sects sect = dbh.GetEntityDB<F2.sects>().GetById(sectMember.sectId);
            if (sect.boss_HP <= 0)
            {
                ResObj["message"] = "Boss已被击杀！\n可在商会中“刷新宗门Boss”";
                return ResObj;
            }
            ResObj["data"] = new JObject(
                new JProperty("boss_level", sect.boss_level),
                new JProperty("remain_boss_hp", sect.boss_HP)
            );
            ResObj["code"] = 0;
            ResObj["type"] = 52;
            return ResObj;
        }
        [HttpPost("api/v3/sects/attack_damage")]
        public JObject attack_damage([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            long val = value["val"].AsLong();
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            var dbh = DbContext.Get();

            if (sectMember.AttackBossCnt >= sectMember.CanAttackBossCnt)
            {
                ResObj["message"] = "已用完挑战次数！\n可在商会中增加“Boss挑战次数”";
                return ResObj;
            }
            dbh.Db.BeginTran();
            try
            {
                dbh.Db.Updateable<F2.sect_member>().ReSetValue(ii => ii.AttackBossCnt == ii.AttackBossCnt + 1).UpdateColumns(ii => ii.AttackBossCnt).Where(ii => ii.id == sectMember.id).ExecuteCommand();
                F2.sectBossDamage damage = dbh.GetEntityDB<F2.sectBossDamage>().AsQueryable().First(ii => ii.playerId == sectMember.playerId && ii.sectid == sectMember.sectId);
                if (damage == null)
                {
                    damage = new F2.sectBossDamage();
                    damage.damage = val;
                    damage.playerId = sectMember.playerId;
                    damage.playerName = sectMember.playerName;
                    damage.playerUuid = sectMember.playerUuid;
                    damage.position_level = sectMember.position_level;
                    damage.sectid = sectMember.sectId;
                    dbh.Db.Insertable(damage).ExecuteCommand();
                }
                else
                {
                    damage.damage += val;
                    dbh.Db.Updateable(damage).UpdateColumns(ii => ii.damage).ExecuteCommand();
                }
                F2.sects sects = dbh.Db.Queryable<F2.sects>().With(SqlSugar.SqlWith.HoldLock).InSingle(sectMember.sectId);
                if (sects.boss_HP <= 0)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "Boss已被击杀";
                    return ResObj;
                }
                if (sects.boss_HP > val)
                {
                    sects.boss_HP -= damage.damage;
                    dbh.Db.Updateable(sects).UpdateColumns(ii => ii.boss_HP).ExecuteCommand();
                }
                else
                {
                    //killed
                    sects.boss_HP = 0;
                    dbh.Db.Updateable(sects).UpdateColumns(ii => ii.boss_HP).ExecuteCommand();

                    
                    F2.sectBossAward award = new F2.sectBossAward();
                    award.bossLevel = sects.boss_level;
                    award.playerId = sectMember.playerId;
                    //所有攻击过的人
                    //var dmglst = dbh.Db.Queryable<F2.sectBossDamage>()
                    //    .Where(ii => ii.sectid == sectMember.sectId)
                    //    .OrderBy(ii => ii.damage, SqlSugar.OrderByType.Desc).Select(ii => ii.playerId).ToList();
                    //两天内有登录的，都有奖励
                    var timelimit = DateTime.Now.AddDays(-2);
                    var dmglst = dbh.Db.Queryable<F2.sect_member>().Where(ii => ii.sectId == sectMember.sectId && ii.last_login_time >= timelimit).Select(ii => ii.playerId).ToList();
                    foreach (var item in dmglst)
                    {
                        award.playerId = item;
                        dbh.Db.Insertable(award).ExecuteCommand();
                    }
                    //sects.boss_level++;
                    //sects.boss_HP = 10000000 * sects.boss_level;
                    //dbh.Db.Updateable(sects).UpdateColumns(ii => new{ii.boss_HP,ii.boss_level}).ExecuteCommand();
                    //dbh.Db.Deleteable<F2.sectBossDamage>().Where(ii => ii.sectid == sectMember.sectId).ExecuteCommand();
                }
                dbh.Db.CommitTran();
            }
            catch (Exception)
            {

                dbh.Db.RollbackTran();
            }

            ResObj["code"] = 0;
            ResObj["type"] = 53;
            return ResObj;
        }
        [HttpPost("api/v3/sects/damage_list")]
        public JObject damage_list([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            long val = value["val"].AsLong();
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            var dbh = DbContext.Get();

            var dmglst = dbh.Db.Queryable<F2.sectBossDamage>().Where(ii => ii.sectid == sectMember.sectId).OrderBy(ii => ii.damage, SqlSugar.OrderByType.Desc).ToList();
            var list = new JArray();
            foreach (var item in dmglst)
            {
                var a = new JObject();
                a["damage"] = item.damage;
                a["player_name"] = item.playerName;
                a["position_level"] = item.position_level;
                a["uuid"] = item.playerUuid;
                list.Add(a);
            }
            ResObj["data"] = list;

            ResObj["code"] = 0;
            ResObj["type"] = 54;
            return ResObj;
        }
        [HttpPost("api/v3/sects/award_list")]
        public JObject award_list([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            var dbh = DbContext.Get();
            List<int> awards = dbh.Db.Queryable<F2.sectBossAward>().Take(10).Where(ii => ii.playerId == account.id).Select(ii => ii.bossLevel).ToList();
            var list = new JArray();
            foreach (var item in awards)
            {
                list.Add(item);
            }
            ResObj["data"] = list;
            ResObj["code"] = 0;
            ResObj["type"] = 56;
            return ResObj;
        }
        [HttpPost("api/v3/sects/get_boss_award")]
        public JObject get_boss_award([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            var dbh = DbContext.Get();
            int bosslevel = value["boss_level"].AsInt();
            var awardItem = dbh.Db.Queryable<F2.sectBossAward>().Where(ii => ii.playerId == account.id && ii.bossLevel== bosslevel).First();
            if (awardItem == null)
            {
                ResObj["message"] = "没有可领取的物品";
                return ResObj;
            }
            F2.sect_member sectMember = updateSectInfo(account);
            dbh.Db.Deleteable<F2.sectBossAward>().In(awardItem.id).ExecuteCommand();
            sectMember.sect_coin += bosslevel * 100;
            dbh.Db.Updateable(sectMember).UpdateColumns(ii => ii.sect_coin).ExecuteCommand();
            List<int> awards = dbh.Db.Queryable<F2.sectBossAward>().Take(10).Where(ii=>ii.playerId==account.id).Select(ii => ii.bossLevel).ToList();
            var list = new JArray();
            foreach (var item in awards)
            {
                list.Add(item);
            }
            ResObj["data"] = new JObject(new JProperty("award_list",list),new JProperty("sect_coin", sectMember.sect_coin));
            ResObj["code"] = 0;
            ResObj["type"] = 55;
            return ResObj;
        }

        #endregion BOSS

        #region 次元门
        private static List<int> dimensionBossSkills = new List<int>() {
           172, 180,182,183
        };

        [HttpPost("api/v3/sects/dimension_call")]
        public JObject dimension_call([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            var dbh = DbContext.Get();
            F2.sects sect = dbh.GetEntityDB<F2.sects>().GetById(sectMember.sectId);
            if (sect.remain_dimension_boss_hp>0)
            {
                ResObj["message"] = "boss未被击杀";
                return ResObj;
            }
            if (sect.remain_dimension_boss_killCnt >= sect.remain_dimension_boss_CankillCnt)
            {
                ResObj["message"] = $"今天Biss已击杀了 {sect.remain_dimension_boss_killCnt} 次";
                return ResObj;
            }
            var rr = new Random();
            var skills = new JArray();
            for (int i = 0; i < 6; i++)
            {
                skills.Add(new JObject(new JProperty("cysmJN", "100"), new JProperty("ID", rr.Next(172, 350).ToString())));
            }

            sect.remain_dimension_boss_skill = skills.ToString(Formatting.None);
            sect.remain_dimension_boss_Level++;
            if (sect.remain_dimension_boss_Level>420)
                sect.remain_dimension_boss_Level = 420;            
            sect.remain_dimension_boss_hp = (long)sect.remain_dimension_boss_Level * (long)5000000;
            dbh.Db.Updateable(sect).UpdateColumns(ii => new { ii.remain_dimension_boss_skill, ii.remain_dimension_boss_hp, ii.remain_dimension_boss_Level }).ExecuteCommand();

            dbh.Db.Deleteable<F2.sectDimensionDamage>().Where(ii => ii.sectid == sectMember.sectId).ExecuteCommand();

            ResObj["data"] = new JObject(
                new JProperty("current_dimension_door", sect.remain_dimension_boss_Level),
                new JProperty("dimension_boss_skill", skills)
            );
            ResObj["code"] = 0;
            ResObj["type"] = 96;
            return ResObj;
        }
        [HttpPost("api/v3/sects/get_dismension_boss")]
        public JObject get_dismension_boss([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            var dbh = DbContext.Get();
            if (sectMember.AckDimBossCnt >= sectMember.CanAckDimBossCnt)
            {
                ResObj["message"] = $"Boss今天已被你暴揍了{sectMember.AckDimBossCnt}次，明天再来吧";
                return ResObj;
            }
            F2.sects sect = dbh.GetEntityDB<F2.sects>().GetById(sectMember.sectId);
            if (sect.remain_dimension_boss_hp <= 0)
            {
                ResObj["message"] = "Boss已被击杀！";
                return ResObj;
            }
            ResObj["data"] = new JObject(
                new JProperty("current_dimension_door", sect.remain_dimension_boss_Level),
                new JProperty("hp", sect.remain_dimension_boss_hp),
                new JProperty("dimension_boss_skill", (JArray)JsonConvert.DeserializeObject(sect.remain_dimension_boss_skill))
            );
            ResObj["code"] = 0;
            ResObj["type"] = 93;
            return ResObj;
        }
        [HttpPost("api/v3/sects/dimension_boss_damage")]
        public JObject dimension_boss_damage([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            long val = value["val"].AsLong();
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            var dbh = DbContext.Get();

            if (sectMember.AckDimBossCnt >= sectMember.CanAckDimBossCnt)
            {
                ResObj["message"] = $"Boss今天已被你暴揍了{sectMember.AckDimBossCnt}次，明天再来吧";
                return ResObj;
            }
            dbh.Db.BeginTran();
            try
            {
                dbh.Db.Updateable<F2.sect_member>().ReSetValue(ii => ii.AckDimBossCnt == ii.AckDimBossCnt + 1).UpdateColumns(ii => ii.AckDimBossCnt).Where(ii => ii.id == sectMember.id).ExecuteCommand();
                F2.sectDimensionDamage damage = dbh.GetEntityDB<F2.sectDimensionDamage>().AsQueryable().First(ii => ii.playerId == sectMember.playerId && ii.sectid == sectMember.sectId);
                if (damage == null)
                {
                    damage = new F2.sectDimensionDamage();
                    damage.damage = val;
                    damage.playerId = sectMember.playerId;
                    damage.playerName = sectMember.playerName;
                    damage.playerUuid = sectMember.playerUuid;
                    damage.position_level = sectMember.position_level;
                    damage.sectid = sectMember.sectId;
                    dbh.Db.Insertable(damage).ExecuteCommand();
                }
                else
                {
                    damage.damage += val;
                    dbh.Db.Updateable(damage).UpdateColumns(ii => ii.damage).ExecuteCommand();
                }
                F2.sects sects = dbh.Db.Queryable<F2.sects>().With(SqlSugar.SqlWith.HoldLock).InSingle(sectMember.sectId);
                if (sects.remain_dimension_boss_hp <= 0)
                {
                    dbh.Db.RollbackTran();
                    ResObj["message"] = "Boss已被击杀";
                    return ResObj;
                }
                sects.remain_dimension_boss_hp -= damage.damage;
                if (sects.remain_dimension_boss_hp > 0)
                {
                    dbh.Db.Updateable(sects).UpdateColumns(ii => ii.remain_dimension_boss_hp).ExecuteCommand();
                }
                else
                {
                    //killed
                    sects.remain_dimension_boss_killCnt++;
                    sects.remain_dimension_boss_hp = 0;
                    dbh.Db.Updateable(sects).UpdateColumns(ii => new { ii.remain_dimension_boss_hp, ii.remain_dimension_boss_killCnt }).ExecuteCommand();

                    F2.sectDimensionBossAward award = new F2.sectDimensionBossAward();
                    award.bossLevel = sects.remain_dimension_boss_Level;
                    award.sect_coin = sects.remain_dimension_boss_Level * 1000;
                    //所有打过的都能获得
                    //var dmglst = dbh.Db.Queryable<F2.sectDimensionDamage>().Where(ii => ii.sectid == sectMember.sectId).OrderBy(ii => ii.damage, SqlSugar.OrderByType.Desc).Select(ii => ii.playerId).ToList();
                    //48小时内有登录的才发奖励
                    var timelimit = DateTime.Now.AddDays(-2);
                    var dmglst = dbh.Db.Queryable<F2.sect_member>().Where(ii => ii.sectId == sectMember.sectId && ii.last_login_time >= timelimit).Select(ii => ii.playerId).ToList();
                    foreach (var item in dmglst)
                    {
                        award.playerId = item;
                        dbh.Db.Insertable(award).ExecuteCommand();
                    }

                    //var rr = new Random();
                    //var skills = new JArray();
                    //for (int i = 0; i < 6; i++)
                    //{
                    //    skills.Add(new JObject(new JProperty("cysmJN", "1"), new JProperty("ID", rr.Next(172, 350))));
                    //}
                    //sects.remain_dimension_boss_skill = skills.ToString(Formatting.None);
                    //sects.remain_dimension_boss_Level++;
                    //sects.remain_dimension_boss_hp = sects.remain_dimension_boss_Level * 5000000;
                    //dbh.Db.Updateable(sects).UpdateColumns(ii => new { ii.remain_dimension_boss_skill, ii.remain_dimension_boss_hp, ii.remain_dimension_boss_Level }).ExecuteCommand();
                }
                dbh.Db.CommitTran();
            }
            catch (Exception)
            {

                dbh.Db.RollbackTran();
            }

            ResObj["code"] = 0;
            ResObj["type"] = 94;
            return ResObj;
        }
        [HttpPost("api/v3/sects/dimension_damage_list")]
        public JObject dimension_damage_list([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            long val = value["val"].AsLong();
            F2.user account = getUserFromCache();
            F2.sect_member sectMember = updateSectInfo(account);
            var dbh = DbContext.Get();

            var dmglst = dbh.Db.Queryable<F2.sectDimensionDamage>().Where(ii => ii.sectid == sectMember.sectId).OrderBy(ii => ii.damage, SqlSugar.OrderByType.Desc).ToList();
            var list = new JArray();
            foreach (var item in dmglst)
            {
                var a = new JObject();
                a["damage"] = item.damage;
                a["player_name"] = item.playerName;
                a["position_level"] = item.position_level;
                a["uuid"] = item.playerUuid;
                list.Add(a);
            }
            ResObj["data"] = list;

            ResObj["code"] = 0;
            ResObj["type"] = 95;
            return ResObj;
        }
        [HttpPost("api/v3/sects/dimension_reward_list")]
        public JObject dimension_reward_list([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            var dbh = DbContext.Get();
            F2.sect_member sectMember = updateSectInfo(account);
            var awards = dbh.Db.Queryable<F2.sectDimensionBossAward>().Take(10).Where(ii => ii.playerId == account.id).ToList();
            var list = new JArray();
            foreach (var item in awards)
            {
                list.Add(new JObject(
                  new JProperty("current_dimension_door", item.bossLevel),
                  new JProperty("position_level", sectMember.position_level),
                  new JProperty("sect_coin", item.sect_coin),
                  new JProperty("time", DateTime.Now.AsTimestamp())
                  ));
            }
            ResObj["data"] = list;
            ResObj["code"] = 0;
            ResObj["type"] = 97;
            return ResObj;
        }
        [HttpPost("api/v3/sects/dimension_reward")]
        public JObject dimension_reward([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            var dbh = DbContext.Get();
            int bosslevel = value["boss_level"].AsInt();
            F2.sect_member sectMember = updateSectInfo(account);
            dbh.Db.BeginTran();
            try
            {
                var awardItem = dbh.Db.Queryable<F2.sectDimensionBossAward>().Where(ii => ii.playerId == account.id).Select(ii => ii.sect_coin).With(SqlSugar.SqlWith.HoldLock).ToList();
                if (awardItem.Count == 0)
                {
                    ResObj["message"] = "没有可领取的奖励";
                    return ResObj;
                }              
                foreach (var item in awardItem)
                {
                    sectMember.sect_coin += item;
                }
                dbh.Db.Updateable(sectMember).UpdateColumns(ii => ii.sect_coin).ExecuteCommand();
                dbh.Db.Deleteable<F2.sectDimensionBossAward>().Where(ii => ii.playerId == account.id).ExecuteCommand();

                dbh.Db.CommitTran();
            }
            catch (Exception)
            {
                dbh.Db.RollbackTran();
            }

            var list = new JArray();
            //TODO:这个格式不对，或导致闪退
            //foreach (var item in awards)
            //{
            //    list.Add(new JObject(
            //    new JProperty("current_dimension_door", item.bossLevel),
            //    new JProperty("position_level", sectMember.position_level),
            //    new JProperty("sect_coin", item.sect_coin),
            //    new JProperty("time", DateTime.Now.AsTimestamp())
            //    ));
            //}
            ResObj["data"] = new JObject(
                new JProperty("sect_coin", sectMember.sect_coin)
                );
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 98;
            return ResObj;
        }
        #endregion 次元门
        #region 秘境
        public static List<F2.mi_jing> rankingsData = new List<F2.mi_jing>();
        public static List<F2.sects> sect_rankingsData = new List<F2.sects>();
        /// <summary>
        /// 秘境信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("api/v3/mi_jings/info")]
        public JObject mi_jings_info([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            var dbh = DbContext.Get();
            F2.sect_member sectMember = updateSectInfo(account);
            if (sectMember.sectId == 0)
            {
                ResObj["message"] = "未加入宗门";
                return ResObj;
            }
            F2.sects sect = dbh.GetEntityDB<F2.sects>().GetById(sectMember.sectId);
            if (sect == null)
            {
                ResObj["message"] = "未加入宗门";
                return ResObj;
            }
            F2.mi_jing mjInfo = dbh.GetEntityDB<F2.mi_jing>().AsQueryable().First(ii => ii.playerId == account.id);
            if (mjInfo == null)
            {
                mjInfo = new F2.mi_jing();
                mjInfo.playerId = account.id;
                mjInfo.playerUuid = account.uuid;
                mjInfo.sect_id = sectMember.sectId;
                mjInfo.isAndroid = account.isAndroid;
                dbh.Db.Insertable(mjInfo).ExecuteCommand();
            }
            else if (mjInfo.sect_id != sectMember.sectId)
            {
                mjInfo.sect_id = sectMember.sectId;
                dbh.Db.Updateable(mjInfo).UpdateColumns(ii => ii.sect_id).ExecuteCommand();
            }
            int owner_point = 0;
            int person_ranking = 0;
            int sect_point = 0;
            int sect_ranking = 0;
            lock (sect_rankingsData)
            {
                var q = from x in sect_rankingsData orderby x.mi_jing_point descending select x;
                int i = 0;
                foreach (var item in q)
                {
                    if (item.id == sectMember.sectId)
                    {
                        sect_ranking = i + 1;
                        sect_point = item.mi_jing_point;
                        break;
                    }
                    i++;
                }
            }
            lock (rankingsData)
            {
                var q = from x in rankingsData orderby x.point descending select x;
                int i = 0;
                foreach (var item in q)
                {
                    if (item.playerId == account.id)
                    {
                        person_ranking = i + 1;
                        owner_point = item.point;
                        break;
                    }
                    i++;
                }
            }
            ResObj["data"] = new JObject(
                new JProperty("isbm", mjInfo.isbm?1:0),
                new JProperty("leftnum", mjInfo.leftnum),
                new JProperty("mi_jing", string.IsNullOrEmpty(mjInfo.enemyData)? new JArray():((JArray)JsonConvert.DeserializeObject(mjInfo.enemyData))),
                new JProperty("owner_point", owner_point),
                new JProperty("owner_ranking", person_ranking),
                new JProperty("sect_point", sect_point),
                new JProperty("sect_ranking", sect_ranking)
            );
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 78;
            return ResObj;
        }
        /// <summary>
        /// 秘境报名
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("api/v3/mi_jing_zhen_rongs")]
        public JObject mi_jing_zhen_rongs([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            var dbh = DbContext.Get();
            F2.setting setting = CacheHelper.GetDBSetting(account.isAndroid);
            if (setting.mi_jing_state == 0)
            {
                ResObj["message"] = "秘境未开启";
                return ResObj;
            }
            else if (setting.mi_jing_state != 1)
            {
                ResObj["message"] = "报名时间已过";
                return ResObj;
            }
            F2.mi_jing mjInfo = dbh.GetEntityDB<F2.mi_jing>().AsQueryable().First(ii => ii.playerId == account.id);
            mjInfo.FirstRoleID = value["FirstRoleID"].AsInt();
            mjInfo.JJCRoles = value["JJCRoles"].ToString(Formatting.None);
            mjInfo.zfDict = value["zfDict"].ToString(Formatting.None);
            mjInfo.playerLv = value["playerlv"].AsInt();
            mjInfo.playerName = value["playerName"].ToString();
            mjInfo.playerUuid = value["uuid"].ToString();
            mjInfo.playerHYJF = value["HYJF"].AsInt();
            mjInfo.isbm = true;
            mjInfo.point = 0;
            mjInfo.leftnum = 0;
            dbh.Db.Updateable(mjInfo).ExecuteCommand();

            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 72;
            return ResObj;
        }
        /// <summary>
        /// 秘境奖励浏览
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("api/v3/mi_jings/reward_info")]
        public JObject mi_jing_reward_info([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            ResObj["data"] = CacheHelper.Get_Mi_jing_reward_info();
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 84;
            return ResObj;
        }
        /// <summary>
        /// 秘境日志
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("api/v3/mi_jings/mi_jing_logs")]
        public JObject mi_jing_logs([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            F2.user account = getUserFromCache();
            var dbh = DbContext.Get();
            F2.mi_jing mjInfo = dbh.GetEntityDB<F2.mi_jing>().AsQueryable().First(ii => ii.playerId == account.id);
            var tmpArr = new JArray();
            if (!string.IsNullOrEmpty(mjInfo.Logs))
            {
                var s = mjInfo.Logs.Split("|");
                foreach (var item in s)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        tmpArr.Add(item);
                    }
                }
            }
            ResObj["data"] = tmpArr;
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 82;
            return ResObj;
        }
        /// <summary>
        /// 进入秘境（报名完成后参战）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("api/v3/mi_jings/explore")]
        public JObject mi_jing_explore([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            var dbh = DbContext.Get();            
            F2.mi_jing mjInfo = dbh.GetEntityDB<F2.mi_jing>().AsQueryable().First(ii => ii.playerId == account.id);
            if (!mjInfo.isbm)
            {
                ResObj["message"] = "你未报名秘境";
                return ResObj;
            }
            if (CacheHelper.GetDBSetting(account.isAndroid).mi_jing_state != 2)
            {
                ResObj["message"] = "挑战时间未到";
                return ResObj;
            }

            if (rankingsData.Count==0)
            {
                lock (locker)
                {
                    if (rankingsData.Count == 0)
                    {
                        rankingsData = dbh.GetEntityDB<F2.mi_jing>().GetList(ii => ii.isRobot || ii.isbm);
                        sect_rankingsData = dbh.GetEntityDB<F2.sects>().GetList();
                    }
                }
            }

            ResObj["data"] = GenMi_jing_enemy(mjInfo, 0);
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 73;
            return ResObj;
        }        
        /// <summary>
        /// 个人排行榜
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("api/v3/mi_jings/rankings")]
        public JObject mi_jing_rankings([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            var dbh = DbContext.Get();            
            if (dbh.GetEntityDB<F2.mi_jing>().AsQueryable().Count(ii => ii.playerId == account.id && ii.isbm == true) == 0)
            {
                ResObj["message"] = "你未报名秘境";
                return ResObj;
            }
            JArray rankings = new JArray();
            int rankingIdx = 0;
            F2.mi_jing mjInfo = new F2.mi_jing();
            lock (rankingsData)
            {
                var q = from x in rankingsData orderby x.point descending select x;
                int i = 0;
                foreach (var item in q)
                {
                    if (item.playerId == account.id)
                    {
                        rankingIdx = i + 1;
                        mjInfo = item;
                    }
                    if (rankings.Count < 100)
                    {
                        rankings.Add(new JObject(
                            new JProperty("FirstRoleID", item.FirstRoleID),
                            new JProperty("HYJF", item.playerHYJF),
                            new JProperty("jjcJF", item.point),
                            new JProperty("jjcPM", i + 1),
                            new JProperty("playerlv", item.playerLv),
                            new JProperty("playerName", item.playerName),
                            new JProperty("uuid", item.id)
                            ));
                    }
                    i++;
                }
            }
            JObject owner = new JObject(
                new JProperty("FirstRoleID", mjInfo.FirstRoleID),
                new JProperty("HYJF", mjInfo.playerHYJF),
                new JProperty("jjcJF", mjInfo.point),
                new JProperty("playerlv", mjInfo.playerLv),
                new JProperty("playerName", mjInfo.playerName),
                new JProperty("ranking", rankingIdx)
            );
            ResObj["data"] = new JObject(new JProperty("owner", owner), new JProperty("rankings", rankings));
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 79;
            return ResObj;
        }        
        /// <summary>
        /// 宗门排行榜
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("api/v3/mi_jings/sect_rankings")]
        public JObject mi_jing_sect_rankings([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            F2.user account = getUserFromCache();
            var dbh = DbContext.Get();
            F2.mi_jing mjInfo = dbh.GetEntityDB<F2.mi_jing>().AsQueryable().First(ii => ii.playerId == account.id);
            if (!mjInfo.isbm)
            {
                ResObj["message"] = "你未报名秘境";
                return ResObj;
            }
            F2.sect_member sectMember = updateSectInfo(account);
            JArray rankings = new JArray();
            int SectIdx = 0;
            int SectPoint = 0;
            lock (sect_rankingsData)
            {
                var q = from x in sect_rankingsData orderby x.mi_jing_point descending select x;
                int i = 0;
                foreach (var item in q)
                {
                    if (item.id == sectMember.sectId)
                    {
                        SectIdx = i + 1;
                        SectPoint = item.mi_jing_point;
                    }
                    if (rankings.Count < 100)
                    {
                        rankings.Add(new JObject(
                            new JProperty("id", item.id),
                            new JProperty("level", item.level),
                            new JProperty("name", item.name),
                            new JProperty("point", item.mi_jing_point),
                            new JProperty("sect_id", item.id)
                        ));
                    }
                    i++;
                }
            }
            JObject owner = new JObject(
                new JProperty("point", SectPoint),
                new JProperty("ranking", SectIdx)
            );
            ResObj["data"] = new JObject(new JProperty("owner", owner), new JProperty("rankings", rankings));
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 81;
            return ResObj;
        }
        /// <summary>
        /// 成功挑战秘境
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("api/v3/mi_jings/challeng_success")]
        public JObject mi_jing_challeng_success([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 1;
            int type = value["type"].AsInt();
            if (type < 0 || type > 2)
            {
                ResObj["message"] = "挑战发生错误";
                return ResObj;
            }
            F2.user account = getUserFromCache();
            if (CacheHelper.GetDBSetting(account.isAndroid).mi_jing_state != 2)
            {
                ResObj["message"] = "挑战时间未到";
                return ResObj;
            }
            var dbh = DbContext.Get();
            F2.mi_jing mjInfo = dbh.GetEntityDB<F2.mi_jing>().AsQueryable().First(ii => ii.playerId == account.id);
            if (string.IsNullOrEmpty(mjInfo.enemyData))
            {
                ResObj["message"] = "你未进入秘境";
                return ResObj;
            }
            JArray enemyData = (JArray)JsonConvert.DeserializeObject(mjInfo.enemyData);
            int jf = 0;
            int kccs = 0;
            switch (type)
            {
                case 0:
                    jf = enemyData[0]["level"].AsInt();
                    kccs = 1;
                    break;
                case 1: jf = enemyData[1]["level"].AsInt()*2;
                    kccs = 10;
                    break;
                case 2:
                    jf = enemyData[2]["level"].AsInt() / 10;
                    if (jf < 5)
                    {
                        jf = 5;
                    }
                    kccs = 10;
                    break;
                default:
                    break;
            }
            if (mjInfo.leftnum < kccs)
            {
                ResObj["message"] = "挑战次数不足";
                return ResObj;
            }
            mjInfo.point += jf;
            //mjInfo.leftnum -= kccs;
            F2.sect_member sectMember = updateSectInfo(account);

            if (rankingsData.Count == 0) ctl_reloadrankingsData();

            //个人列表
            lock (rankingsData)
            {
                var x = rankingsData.Find(ii => ii.playerId == account.id);
                if (x == null)
                {
                    ResObj["message"] = "你未进入秘境";
                    return ResObj;
                }
                x.point += jf;
                x.leftnum -= kccs;
            }
            //宗门列表
            lock (sect_rankingsData)
            {
                var x = sect_rankingsData.Find(ii => ii.id == sectMember.sectId);
                if (x == null)
                {
                    ResObj["message"] = "你未加入宗门";
                    return ResObj;
                }
                x.mi_jing_point += jf;
            }
            dbh.Db.BeginTran();
            try
            {
                int optcnt = dbh.Db.Updateable<F2.mi_jing>()
                    .ReSetValue(ii => ii.point == ii.point + jf)
                    .ReSetValue(ii => ii.leftnum == ii.leftnum - kccs)
                    .UpdateColumns(ii => new { ii.point, ii.leftnum })
                    .Where(ii => ii.id == mjInfo.id && ii.leftnum >= kccs).ExecuteCommand();
                if (optcnt!=1)
                {
                    ResObj["message"] = "挑战次数不足";
                    return ResObj;
                }

                dbh.Db.Updateable<F2.sects>()
                    .ReSetValue(ii => ii.mi_jing_point == ii.mi_jing_point + jf)
                    .UpdateColumns(ii => ii.mi_jing_point).Where(ii => ii.id == sectMember.sectId).ExecuteCommand();
                if (type == 2)
                {
                    //挑战角色，要减去对方的 10%
                    int iid = enemyData[2]["playerDict"]["id"].AsInt();
                    var mjd = dbh.GetEntityDB<F2.mi_jing>().GetById(iid);
                    if (!mjd.isRobot)
                    {
                        jf = (int)(mjd.point * 0.1);
                        var logStr = $"你于{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}被 {sectMember.playerName} 击败，损失积分{jf}";

                        mjd.Logs += "|" + logStr;
                        dbh.Db.Updateable<F2.mi_jing>()
                            .SetColumns(ii => ii.point == ii.point - jf)
                            .SetColumns(ii => ii.Logs == mjd.Logs)
                            .Where(ii => ii.id == iid).ExecuteCommand();

                        var xx = rankingsData.Find(ii => ii.playerId == iid);
                        if (xx != null)
                        {
                            xx.point -= jf;
                        }
                    }
                }
                dbh.Db.CommitTran();
            }
            catch (Exception)
            {
                dbh.Db.RollbackTran();
                throw;
            }
           // int owner_point = 0;
            int person_ranking = 0;
            int sect_point = 0;
            int sect_ranking = 0;
            lock (sect_rankingsData)
            {
                var q = from x in sect_rankingsData orderby x.mi_jing_point descending select x;
                int i = 0;
                foreach (var item in q)
                {
                    if (item.id == sectMember.sectId)
                    {
                        sect_ranking = i + 1;
                        sect_point = item.mi_jing_point;
                        break;
                    }
                    i++;
                }
            }
            lock (rankingsData)
            {
                var q = from x in rankingsData orderby x.point descending select x;
                int i = 0;
                foreach (var item in q)
                {
                    if (item.playerId == account.id)
                    {
                        mjInfo = item;
                        person_ranking = i + 1;
                        //owner_point = item.point;
                        break;
                    }
                    i++;
                }
            }
            ResObj["data"] = new JObject(
                new JProperty("info", GenMi_jing_enemy(mjInfo, person_ranking)),
                new JProperty("leftnum", mjInfo.leftnum),
                new JProperty("owner_point", mjInfo.point),
                new JProperty("person_ranking", person_ranking),
                new JProperty("sect_point", sect_point),
                new JProperty("sect_ranking", sect_ranking)
                );
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 74;
            return ResObj;
        }

        /// <summary>
        /// 查看秘境角色装备
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("api/v3/mi_jings/detail_info")]
        public JObject mi_jing_detail_info([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            F2.user account = getUserFromCache();
            int other_uuid = value["other_uuid"].AsInt();
            var dbh = DbContext.Get();
            F2.mi_jing mjInfo = dbh.GetEntityDB<F2.mi_jing>().AsQueryable().First(ii => ii.id == other_uuid);

            ResObj["data"] = new JObject(new JProperty("JJCRoles", mjInfo.JJCRoles), new JProperty("playerName", mjInfo.playerName), new JProperty("zfDict", mjInfo.zfDict));
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 80;
            return ResObj;
        }

        [HttpPost("api/v3/mi_jings/get_sect_rewards")]
        public JObject mi_jing_get_sect_rewards([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 77;
            F2.user account = getUserFromCache();
            if (CacheHelper.GetDBSetting(account.isAndroid).mi_jing_state != 3)
            {
                ResObj["message"] = "还没有到领取时间";
                return ResObj;
            }

            F2.sect_member sectMember = updateSectInfo(account);
            var dbh = DbContext.Get();
            F2.mi_jing mjInfo = dbh.GetEntityDB<F2.mi_jing>().AsQueryable().First(ii => ii.playerId == account.id);
            if (mjInfo == null)
            {
                ResObj["message"] = "你没有参加本次秘境";
                return ResObj;
            }
            if (mjInfo.reward_sect)
            {
                ResObj["message"] = "你已领取过宗门奖励";
                return ResObj;
            }
            int sect_point = 0;
            int sect_ranking = 0;
            lock (sect_rankingsData)
            {
                var q = from x in sect_rankingsData orderby x.mi_jing_point descending select x;
                int i = 0;
                foreach (var item in q)
                {
                    if (item.id == sectMember.sectId)
                    {
                        sect_ranking = i + 1;
                        sect_point = item.mi_jing_point;
                        break;
                    }
                    i++;
                }
            }
            var datas = CacheHelper.Getmi_jing_reward().Find(ii => sect_ranking >= ii.ranking && sect_ranking <= ii.end_ranking && ii.isSect);
            if (datas != null)
            {
                dbh.Db.BeginTran();
                try
                {
                    mjInfo.reward_sect = true;
                    var opt = dbh.Db.Updateable(mjInfo).UpdateColumns(ii => new { ii.reward_sect }).Where(ii => ii.id == mjInfo.id && ii.reward_sect == false).ExecuteCommand();
                    if (opt != 1)
                    {
                        ResObj["message"] = "你已领取过宗门奖励";
                        return ResObj;
                    }
                    sectMember.sect_coin += datas.sect_coin;
                    dbh.Db.Updateable(sectMember).UpdateColumns(ii => new { ii.sect_coin }).ExecuteCommand();
                    dbh.Db.CommitTran();
                }
                catch (Exception)
                {
                    dbh.Db.RollbackTran();
                    throw;
                }
                ResObj["data"] = new JObject(
                    new JProperty("add_sect_coin", datas.sect_coin),
                    new JProperty("sect_coin", sectMember.sect_coin)
                   );
                ResObj["code"] = 0;
                ResObj["message"] = "success";
                ResObj["type"] = 77;
                return ResObj;
            }
            else
            {
                ResObj["message"] = "你当前名次没有可领取的奖励";
                return ResObj;
            }            
        }
        [HttpPost("api/v3/mi_jings/get_rewards")]
        public JObject mi_jing_get_rewards([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 76;
            F2.user account = getUserFromCache();
            if (CacheHelper.GetDBSetting(account.isAndroid).mi_jing_state != 3)
            {
                ResObj["message"] = "还没有到领取时间";
                return ResObj;
            }

            F2.sect_member sectMember = updateSectInfo(account);
            var dbh = DbContext.Get();
            F2.mi_jing mjInfo = dbh.GetEntityDB<F2.mi_jing>().AsQueryable().First(ii => ii.playerId == account.id);
            if (mjInfo == null)
            {
                ResObj["message"] = "你没有参加本次秘境";
                return ResObj;
            }
            if (mjInfo.reward_person)
            {
                ResObj["message"] = "你已领取过宗门奖励";
                return ResObj;
            }
            int ranking = 0;
            lock (rankingsData)
            {
                var q = from x in rankingsData orderby x.point descending select x;
                int i = 0;
                foreach (var item in q)
                {
                    if (item.playerId == account.id)
                    {
                        ranking = i + 1;
                        break;
                    }
                    i++;
                }
            }
            var datas = CacheHelper.Getmi_jing_reward().Find(ii => ranking >= ii.ranking && ranking <= ii.end_ranking && !ii.isSect);
            if (datas != null)
            {
                dbh.Db.BeginTran();
                try
                {
                    mjInfo.reward_person = true;
                    var opt = dbh.Db.Updateable(mjInfo).UpdateColumns(ii => new { ii.reward_person }).Where(ii => ii.id == mjInfo.id && ii.reward_person == false).ExecuteCommand();
                    if (opt != 1)
                    {
                        ResObj["message"] = "你已领取过宗门奖励";
                        return ResObj;
                    }
                    sectMember.sect_coin += datas.sect_coin;
                    dbh.Db.Updateable(sectMember).UpdateColumns(ii => new { ii.sect_coin }).ExecuteCommand();

                    dbh.Db.Updateable(account).ReSetValue(ii => ii.shl == ii.shl + datas.shl).UpdateColumns(ii => new { ii.shl }).ExecuteCommand();

                    dbh.Db.CommitTran();
                }
                catch (Exception)
                {
                    dbh.Db.RollbackTran();
                    throw;
                }
                ResObj["data"] = new JObject(
                    new JProperty("content", datas.content),
                    new JProperty("sect_coin", datas.sect_coin)
                   );
                ResObj["code"] = 0;
                ResObj["message"] = "success";
                ResObj["type"] = 76;
                return ResObj;
            }
            else
            {
                ResObj["message"] = "你当前名次没有可领取的奖励";
                return ResObj;
            }
        }
        [HttpPost("api/v3/mi_jings/sect_info")]
        public JObject mi_jing_sect_info([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            F2.user account = getUserFromCache();
            int sect_id = value["sect_id"].AsInt();

            JArray rankings = new JArray();
            lock (rankingsData)
            {
                var q = from x in rankingsData orderby x.point descending select x;
                foreach (var item in q)
                {
                    if (item.sect_id == sect_id)
                    {
                        rankings.Add(new JObject(
                            new JProperty("jjcJF", item.point),
                            new JProperty("playerlv", item.playerLv),
                            new JProperty("playerName", item.playerName)
                            ));
                    }
                }
            }

            ResObj["data"] = rankings;
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 85;
            return ResObj;
        }
        #endregion 秘境


        [HttpPost("api/v3/shi_lian_ta/info")]
        public JObject shi_lian_ta_info([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            F2.user account = getUserFromCache();
            
            ResObj["data"] = new JObject(new JProperty("slt_num", 0));
            ResObj["code"] = 0;
            ResObj["message"] = "success";
            ResObj["type"] = 101;
            return ResObj;
        }

        #region 星空探索
        private JArray decodeStarInfo(string a)
        {
            var tmpDV = new JArray();
            if (!string.IsNullOrEmpty(a))
            {
                foreach (var item in a.Split(","))
                {
                    int Tmpint;
                    if (!string.IsNullOrEmpty(item) && int.TryParse(item, out Tmpint))
                    {
                        tmpDV.Add(Tmpint);
                    }
                }
            }
            return tmpDV;
        }
        private int getXktsZdtl(int cz)
        {
            int cz1 = cz % 1000;
            int cz2 = cz / 1000;
            int lszd = 8;
            if (cz2 > 0)
            {
                lszd += 5 + cz2;
            }
            else if (cz1 >= 800)
            {
                lszd += 5;
            }
            else if (cz1 >= 600)
            {
                lszd += 4;
            }
            else if (cz1 >= 400)
            {
                lszd += 3;
            }
            else if (cz1 >= 200)
            {
                lszd += 2;
            }
            else if (cz1 > 0)
            {
                lszd += 1;
            }
            return lszd;
        }
        private JObject getNewSearch(DbContext dbh,F2.user account ,ref F2.xkts xkts)
        {
            JObject zhen_rong = new JObject();
            int starid = new Random().Next(0, 300);
            if (starid < 180)
            {
                string ttstr = "," + starid.ToString() + ",";
                var dd = dbh.Db.Queryable<F2.xkts>().First(ii => ii.star_info.Contains(ttstr));
                if (dd != null)
                {
                    if (dd.playerId == account.id)
                    {
                        //已被自己占领的星球
                        starid = 9999;
                    }
                    else if (!string.IsNullOrEmpty(dd.JJCRoles) && dd.JJCRoles.Length>10)
                    {
                        //被别人占领的(且有阵容
                        zhen_rong = new JObject(
                        new JProperty("uuid", dd.playerUuid),
                        new JProperty("zfDict", dd.zfDict),
                        new JProperty("JJCRoles", dd.JJCRoles),
                        new JProperty("playerlv", dd.playerLevel),
                        new JProperty("playerName", dd.playerName)
                        );
                    }
                }
            }
            if (starid < 100)
            {
                //小
                xkts.current_explore = 1;
                xkts.current_explore_id = starid;
            }
            else if (starid < 150)
            {
                //中
                xkts.current_explore = 2;
                xkts.current_explore_id = starid;
            }
            else if (starid < 170)
            {
                //大
                xkts.current_explore = 3;
                xkts.current_explore_id = starid;
            }
            else if (starid < 180)
            {
                //巨星
                xkts.current_explore = 4;
                xkts.current_explore_id = starid;
            }
            else
            {
                //荒芜星球
                xkts.current_explore = 0;
                xkts.current_explore_id = 0;
            }

            return zhen_rong;
        }
        [HttpPost("api/v3/xkts/get_info")]
        public JObject xkts_get_info([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 102;
            F2.user account = getUserFromCache();
            //{"code":0,"type":102,"data":{"max_tl":20,"tl":1,"czjf":7005300,"last_explore":1569692945,"current_explore":0,"star_info":[120,125,161,176],"recover_time":8447}}
            var dbh = DbContext.Get();
            var ztl = getXktsZdtl(account.cz);
            F2.xkts xkts = dbh.Db.Queryable<F2.xkts>().First(ii => ii.playerId == account.id);
            if (xkts == null)
            {
                xkts = new F2.xkts();
                xkts.playerId = account.id;
                xkts.playerUuid = account.uuid;
   
                var playerDict = ((JObject)JsonConvert.DeserializeObject(account.player_data))["playerDict"];
                xkts.playerName = playerDict["playName"]?.ToString() ?? account.username;
                xkts.playerLevel = GetExpLevel(playerDict["juntuanExp"].AsLong());
                xkts.playerHYJF = GetVIPLevel(playerDict["hyJiFen"].AsInt());//会员等级
                xkts.tl = ztl;
                xkts.last_explore = DateTime.Now;
                xkts.start_recover = DateTime.Now;
                xkts.FirstRoleID="60005";
                xkts.zfDict="{}";
                xkts.JJCRoles="{}";
                xkts.zhen_rong="{}";

                dbh.Db.Insertable(xkts).ExecuteCommand();
            }
            else
            {
                var playerDict = ((JObject)JsonConvert.DeserializeObject(account.player_data))["playerDict"];
                xkts.playerName = playerDict["playName"]?.ToString() ?? account.username;
                xkts.playerLevel = GetExpLevel(playerDict["juntuanExp"].AsLong());
                xkts.playerHYJF = GetVIPLevel(playerDict["hyJiFen"].AsInt());//会员等级

                var ttt = DateTime.Now - xkts.start_recover;
                int addtl =(int)(ttt.TotalHours / 3);
                xkts.tl += addtl;
                if (xkts.tl >= ztl)
                {
                    xkts.tl = ztl;
                    xkts.recover_time = 0;
                    xkts.start_recover = DateTime.Now;
                }
                else
                {
                    xkts.recover_time = 3 * 60 * 60 - ((int)ttt.TotalSeconds % (3 * 60 * 60));
                    xkts.start_recover = xkts.start_recover.AddHours(addtl * 3);
                }

                dbh.Db.Updateable(xkts).ExecuteCommand();
            }

            ResObj["data"] = new JObject(
                new JProperty("zhen_rong", (JObject)JsonConvert.DeserializeObject(xkts.zhen_rong??"{}")),


                new JProperty("max_tl", ztl),
                new JProperty("tl", xkts.tl),
                new JProperty("czjf", 0),
                new JProperty("last_explore", xkts.last_explore.AsTimestamp()),
                new JProperty("current_explore", xkts.current_explore),
                new JProperty("star_info", decodeStarInfo(xkts.star_info)),
                new JProperty("recover_time", xkts.recover_time)

                );
            ResObj["code"] = 0;
            ResObj["type"] = 102;
            return ResObj;
        }
        [HttpPost("api/v3/xkts/explore_xq")]
        public JObject xkts_explore_xq([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 103;
            F2.user account = getUserFromCache();
            //{"code":0,"type":103,"data":{"zhen_rong":{},"reward":"getSj","current_explore":0,"max_tl":20,"tl":0,"start_recover":1569690592,"last_explore":1569692984,"recover_time":8408,"last_explore_event":0,"star_info":[120,125,161,176]}}
            var dbh = DbContext.Get();
            F2.xkts xkts = dbh.Db.Queryable<F2.xkts>().First(ii => ii.playerId == account.id);
            string rewardData = "none";
            if (xkts == null)
            {
                ResObj["message"] = "未开启星空探索";
                return ResObj;
            }
            if (xkts.current_explore > 0)
            {
                //星球
                if (!string.IsNullOrEmpty(xkts.zhen_rong) && xkts.zhen_rong.Length > 10)
                {
                    xkts.tl--;
                    dbh.Db.Updateable(xkts).UpdateColumns("tl").ExecuteCommand();
                    return xkts_bug_tl(value);
                }
                else
                {
                    //(直接占领
                    rewardData = "getSMBY";
                    xkts.star_info += "," + xkts.current_explore_id + ",";
                }
            }
            else
            {
                int xtype = new Random().Next(0, 5);
                if (xtype == 0)
                {
                    rewardData = "getSj";
                }
                else if (xtype == 1)
                {
                    rewardData = "getSMBY";
                }
            }
            xkts.tl--;
            var xkts_zhen_rong = getNewSearch(dbh, account, ref xkts);
            xkts.zhen_rong = xkts_zhen_rong.ToString(Formatting.None);
            dbh.Db.Updateable(xkts).UpdateColumns("tl", "current_explore", "current_explore_id", "star_info", "zhen_rong").ExecuteCommand();
            var ztl = getXktsZdtl(account.cz);
            ResObj["data"] = new JObject(
                new JProperty("zhen_rong", xkts_zhen_rong),
                new JProperty("reward", rewardData),   //none,getSj,getSMBY
                new JProperty("current_explore", xkts.current_explore),
                new JProperty("max_tl", ztl),
                new JProperty("tl", xkts.tl),
                new JProperty("start_recover", xkts.start_recover.AsTimestamp()),
                new JProperty("last_explore", xkts.last_explore.AsTimestamp()),
                new JProperty("recover_time", xkts.recover_time),
                new JProperty("last_explore_event", 0),
                new JProperty("star_info", decodeStarInfo(xkts.star_info))
            );
            ResObj["code"] = 0;
            ResObj["type"] = 103;
            return ResObj;
        }
        [HttpPost("api/v3/xkts/get_zhenrong")]
        public JObject xkts_get_zhenrong([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 105;
            F2.user account = getUserFromCache();
            //{"code":0,"type":105,"data":{"uuid":"","zfDict":"{}","JJCRoles":"{}", "playerlv","1000", "playerName":"xxx"}}
            var dbh = DbContext.Get();
            F2.xkts xkts = dbh.Db.Queryable<F2.xkts>().First(ii => ii.playerId == account.id);
            if (xkts == null)
            {
                ResObj["message"] = "未开启星空探索";
                return ResObj;
            }

            ResObj["data"] = new JObject(
                new JProperty("uuid", account.uuid),
                new JProperty("zfDict", xkts.zfDict), 
                new JProperty("JJCRoles", xkts.JJCRoles),
                new JProperty("playerlv", xkts.playerLevel),
                new JProperty("playerName", xkts.playerName)
                );
            ResObj["code"] = 0;
            ResObj["type"] = 105;
            ResObj["message"] = "success";
            return ResObj;
        }
        [HttpPost("api/v3/xkts/save_zhenrong")]
        public JObject xkts_save_zhenrong([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 104;
            F2.user account = getUserFromCache();
            var dbh = DbContext.Get();
            F2.xkts xkts = dbh.Db.Queryable<F2.xkts>().First(ii => ii.playerId == account.id);
            if (xkts == null)
            {
                ResObj["message"] = "未开启星空探索";
                return ResObj;
            }
            xkts.FirstRoleID = value["FirstRoleID"].ToString();
            xkts.zfDict = value["zfDict"].ToString();
            xkts.JJCRoles = value["JJCRoles"].ToString();
            dbh.Db.Updateable(xkts).UpdateColumns("FirstRoleID", "zfDict", "JJCRoles").ExecuteCommand();

            //{"code":0,"type":104,"message":"success"}
            ResObj["code"] = 0;
            ResObj["type"] = 104;
            ResObj["message"] = "success";
            return ResObj;
        }
        [HttpPost("api/v3/xkts/get_reward")]
        public JObject xkts_get_reward([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 108;
            F2.user account = getUserFromCache();
            var dbh = DbContext.Get();
            F2.xkts xkts = dbh.Db.Queryable<F2.xkts>().First(ii => ii.playerId == account.id);
            if (xkts == null)
            {
                ResObj["message"] = "未开启星空探索";
                return ResObj;
            }
            if (xkts.getRewardTime.Date >= DateTime.Now.Date)
            {
                ResObj["message"] = "今天已经领取过奖励";
                return ResObj;
            }
            xkts.getRewardTime = DateTime.Now;
            dbh.Db.Updateable(xkts).UpdateColumns("getRewardTime").ExecuteCommand();
            var dval = decodeStarInfo(xkts.star_info);
            int zlsmby = 1;
            int zlsj = 100;
            foreach (int item in dval)
            {
                if (item < 100)
                {
                    zlsmby += 1;
                    zlsj += 100;
                }
                else if (item < 150)
                {
                    zlsmby += 2;
                    zlsj += 200;

                }
                else if (item < 170)
                {
                    zlsmby += 3;
                    zlsj += 300;

                }
                else if (item < 180)
                {
                    zlsmby += 4;
                    zlsj += 400;
                }
            }
            ResObj["data"] = new JObject(
                new JProperty("zlsmby", zlsmby),
                new JProperty("zlsj", zlsj)
            );
            //{"code":0,"type":108,"message":"success","data":{"zlsmby":12,"zlsj":1200}}
            ResObj["code"] = 0;
            ResObj["type"] = 108;
            ResObj["message"] = "success";
            return ResObj;
        }
        [HttpPost("api/v3/xkts/leave")]
        public JObject xkts_leave([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 106;
            F2.user account = getUserFromCache();

            var dbh = DbContext.Get();
            F2.xkts xkts = dbh.Db.Queryable<F2.xkts>().First(ii => ii.playerId == account.id);
            if (xkts == null)
            {
                ResObj["message"] = "未开启星空探索";
                return ResObj;
            }
            if (xkts.tl < 1)
            {
                ResObj["message"] = "体力不足";
                return ResObj;
            }

            xkts.tl--;
            var xkts_zhen_rong = getNewSearch(dbh, account, ref xkts);
            xkts.zhen_rong = xkts_zhen_rong.ToString(Formatting.None);
            dbh.Db.Updateable(xkts).UpdateColumns("tl", "current_explore", "current_explore_id", "zhen_rong").ExecuteCommand();
            var ztl = getXktsZdtl(account.cz);
            //{"code":1,"type":106,"message":"体力不足"}
            ResObj["data"] = new JObject(
                new JProperty("zhen_rong", xkts_zhen_rong),
              new JProperty("max_tl", ztl),
              new JProperty("tl", xkts.tl),
              new JProperty("czjf", 0),
              new JProperty("last_explore", xkts.last_explore.AsTimestamp()),
              new JProperty("current_explore", xkts.current_explore),
              new JProperty("star_info", decodeStarInfo(xkts.star_info)),
              new JProperty("recover_time", xkts.recover_time)
              );
            ResObj["code"] = 0;
            ResObj["type"] = 106;
            ResObj["message"] = "success";
            return ResObj;
        }
        [HttpPost("api/v3/xkts/xkts_logs")]
        public JObject xkts_xkts_logs([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 111;
            F2.user account = getUserFromCache();

            var dbh = DbContext.Get();
            F2.xkts xkts = dbh.Db.Queryable<F2.xkts>().First(ii => ii.playerId == account.id);
            if (xkts == null)
            {
                ResObj["message"] = "未开启星空探索";
                return ResObj;
            }
            JArray ja = new JArray();
            if (!string.IsNullOrEmpty(xkts.logs))
            {
                var logsArr = xkts.logs.Split('|');
                string s2 = "";
                if (logsArr.Length > 20)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        s2 += "|" + logsArr[i];
                    }
                    xkts.logs = s2.Substring(1);
                    dbh.Db.Updateable(xkts).UpdateColumns("logs").ExecuteCommand();
                }
                for (int i = logsArr.Length - 1; i > Math.Max(0, logsArr.Length - 20); i--)
                {
                    ja.Add(logsArr[i]);
                }
            }
            //{"code":0,"type":111,"message":"success","data":[]}
            ResObj["data"] = ja;
            ResObj["code"] = 0;
            ResObj["type"] = 111;
            ResObj["message"] = "success";
            return ResObj;
        }
        [HttpPost("api/v3/xkts/bug_tl")]
        public JObject xkts_bug_tl([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 110;
            F2.user account = getUserFromCache();

            var dbh = DbContext.Get();
            F2.xkts xkts = dbh.Db.Queryable<F2.xkts>().First(ii => ii.playerId == account.id);
            if (xkts == null)
            {
                ResObj["message"] = "未开启星空探索";
                return ResObj;
            }
            ResObj["message"] = "暂不支持购买";
            return ResObj;


            var ztl = getXktsZdtl(account.cz);
            ResObj["data"] = new JObject(
                new JProperty("zhen_rong", (JObject)JsonConvert.DeserializeObject(xkts.zhen_rong ?? "{}")),
              new JProperty("max_tl", ztl),
              new JProperty("tl", xkts.tl),
              new JProperty("czjf", 0),
              new JProperty("last_explore", xkts.last_explore.AsTimestamp()),
              new JProperty("current_explore", xkts.current_explore),
              new JProperty("star_info", decodeStarInfo(xkts.star_info)),
              new JProperty("recover_time", xkts.recover_time)
              );

            ResObj["code"] = 0;
            ResObj["type"] = 110;
            ResObj["message"] = "success";
            return ResObj;
        }

        [HttpPost("api/v3/xkts/give_up_star")]
        public JObject xkts_give_up_star([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 109;
            F2.user account = getUserFromCache();

            var dbh = DbContext.Get();
            F2.xkts xkts = dbh.Db.Queryable<F2.xkts>().First(ii => ii.playerId == account.id);
            if (xkts == null)
            {
                ResObj["message"] = "未开启星空探索";
                return ResObj;
            }
            int starid = value["star_id"].AsInt();
            var dval = decodeStarInfo(xkts.star_info);
            var tmpv = "";
            foreach (int item in dval)
            {
                if (item != starid)
                {
                    tmpv += "," + item.ToString()+",";
                }
            }
            xkts.star_info = tmpv;

            dbh.Db.Updateable(xkts).UpdateColumns("star_info").ExecuteCommand();
            var ztl = getXktsZdtl(account.cz);
            ResObj["data"] = new JObject(
                new JProperty("zhen_rong", (JObject)JsonConvert.DeserializeObject(xkts.zhen_rong ?? "{}")),
              new JProperty("max_tl", ztl),
              new JProperty("tl", xkts.tl),
              new JProperty("czjf", 0),
              new JProperty("last_explore", xkts.last_explore.AsTimestamp()),
              new JProperty("current_explore", xkts.current_explore),
              new JProperty("star_info", decodeStarInfo(xkts.star_info)),
              new JProperty("recover_time", xkts.recover_time)
              );

            ResObj["code"] = 0;
            ResObj["type"] = 109;
            ResObj["message"] = "success";
            return ResObj;
        }

        [HttpPost("api/v3/xkts/success")]
        public JObject xkts_success([FromBody] JObject value)
        {
            JObject ResObj = new JObject();
            ResObj["code"] = 1;
            ResObj["type"] = 103;
            F2.user account = getUserFromCache();
            var dbh = DbContext.Get();
            F2.xkts xkts = dbh.Db.Queryable<F2.xkts>().First(ii => ii.playerId == account.id);
            if (xkts == null)
            {
                ResObj["message"] = "未开启星空探索";
                return ResObj;
            }
            string ttstr = "," + xkts.current_explore_id.ToString() + ",";
            var dd = dbh.Db.Queryable<F2.xkts>().First(ii => ii.star_info.Contains(ttstr));
            if (dd == null)
            {
                ResObj["message"] = "星球已被其他人抢走了";
                return ResObj;
            }
            if (dd.playerId == account.id)
            {
                //已被自己占领的星球
                ResObj["message"] = "星球已被自己占领";
                return ResObj;
            }
            else if (!string.IsNullOrEmpty(dd.JJCRoles) && dd.JJCRoles.Length > 10)
            {
                dd.star_info = dd.star_info.Replace(ttstr, ",");
                dd.logs += "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "你的第" + xkts.current_explore_id + "号星球被 " + xkts.playerName + " 抢走";
                dbh.Db.Updateable(dd).UpdateColumns("star_info","logs").ExecuteCommand();
            }
            else
            {
                ResObj["message"] = "星球已被其他人抢走了！";
                return ResObj;
            }

            xkts.star_info += "," + xkts.current_explore_id + ",";
            var xkts_zhen_rong = getNewSearch(dbh, account, ref xkts);
            xkts.zhen_rong = xkts_zhen_rong.ToString(Formatting.None);
            dbh.Db.Updateable(xkts).UpdateColumns("current_explore", "current_explore_id", "star_info", "zhen_rong").ExecuteCommand();
            var ztl = getXktsZdtl(account.cz);
            ResObj["data"] = new JObject(
                new JProperty("zhen_rong", xkts_zhen_rong),
                new JProperty("reward", "getSMBY"),   //none,getSj,getSMBY
                new JProperty("current_explore", xkts.current_explore),
                new JProperty("max_tl", ztl),
                new JProperty("tl", xkts.tl),
                new JProperty("start_recover", xkts.start_recover.AsTimestamp()),
                new JProperty("last_explore", xkts.last_explore.AsTimestamp()),
                new JProperty("recover_time", xkts.recover_time),
                new JProperty("last_explore_event", 0),
                new JProperty("star_info", decodeStarInfo(xkts.star_info))
            );

            ResObj["code"] = 0;
            ResObj["type"] = 103;
            ResObj["message"] = "success";
            return ResObj;
        }



        #endregion 星空探索



    }


    class GameItem
    {
        public int id;
        public int num;
        public int itemType;
        public int childType;
    }
}