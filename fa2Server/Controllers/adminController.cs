using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fa2Server.Models;
using MakC.Data;
using MakC.Data.Model;
using Microsoft.AspNetCore.Mvc;

namespace fa2Server.Controllers
{
    [Route("97ca200f-471f-487d-9722-178f8c858fb9")]
    public class adminController : Controller
    {

        private bool checkCookie()
        {
            var sx = DbContext.Get().Db.Queryable<F2.user>().Select(ii => new { ii.id, ii.password }).First(ii => ii.id == 1);
            return HttpContext.Request.Cookies["utok"] == sx.password;
        }
        public IActionResult Index()
        {
            if (checkCookie())
            {
                return showlist();
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult list(string name)
        {
            var dbh = DbContext.Get();
            if (dbh.Db.Queryable<F2.user>().Count(ii => ii.id == 1 && ii.password == name) == 0)
            {
                return RedirectToAction("Index");
            }
            HttpContext.Response.Cookies.Append("utok", name);
            return showlist();
        }
        private IActionResult showlist()
        {
            var value = DbContext.Get().Db.Queryable<F2.user, F2.sect_member>((t1, t2) => new object[] { SqlSugar.JoinType.Left, t1.id == t2.playerId })
                .Select((t1, t2) => new { t1.id, t1.uuid, t1.username, t1.isAndroid, t1.isBan, t2.playerName })
                .OrderBy(t1 => t1.username)
                .ToList();
            allUserLst model = new allUserLst();
            model.list = new List<allUserLstItem>();
            foreach (var item in value)
            {
                model.list.Add(new allUserLstItem()
                {
                    id = item.id,
                    uuid = item.uuid,
                    username = item.username,
                    isAndroid = item.isAndroid,
                    isBan = item.isBan,
                    playerName = item.playerName
                });
            }
            return View("list",model);
        }

        [Route("/u")]
        public IActionResult u(string uid)
        {
            if (!checkCookie()) return NotFound();

            var dbh = DbContext.Get();
            uModel model = new uModel();
            model.user= dbh.Db.Queryable<F2.user>().IgnoreColumns(ii => new { ii.player_data, ii.player_zhong_yao }).First(ii => ii.uuid == uid);
            if (model.user == null)
            {
                return NotFound();
            }
            model.sectInfo = dbh.Db.Queryable<F2.sect_member>().First(ii => ii.playerUuid == uid);
            return View(model);
        }
        [HttpPost("/u/p")]
        public IActionResult userpay(string uuid,int amount)
        {
            if (string.IsNullOrEmpty(uuid) || amount<=0)
            {
                return NotFound();
            }
            var dbh = DbContext.Get();
            var user = dbh.Db.Queryable<F2.user>().IgnoreColumns(ii => new { ii.player_data, ii.player_zhong_yao }).First(ii => ii.uuid == uuid);
            if (user == null)
            {
                return NotFound();
            }
            user.cheatMsg += ";充值+" + amount;
            user.cz += amount;
            user.shl += amount * 15;
            dbh.Db.Updateable<F2.user>(user)
                .UpdateColumns(ii => new { ii.cheatMsg, ii.cz, ii.shl })
                .ExecuteCommand();

            gavegifthyJiFen(uuid, 1, amount * 1000);
            return Redirect("/u?uid=" + user.uuid + "&ok=1");
        }
        private void gavegifthyJiFen(string uuid,int code, int num)
        {
            F2.giftCode gift = new F2.giftCode();
            gift.code = code;
            gift.create_at = DateTime.Now;
            gift.uuid = uuid;
            gift.itemData = "{\"error\":0,\"GETBODY\":{\"hyJiFen\":" + num + "}}";
            DbContext.Get().Db.Insertable(gift).ExecuteCommand();
        }        
        private void gavegift(string uuid, int code, int itemtype, int itemid, int num)
        {
            F2.giftCode gift = new F2.giftCode();
            gift.code = code;
            gift.create_at = DateTime.Now;
            gift.uuid = uuid;
            gift.itemData = "{\"error\":0,\"GETBODY\":{\"itemGetArr\":[{\"childType\":\"" + itemid + "\",\"itemType\":\"" + itemtype + "\",\"itemNum\":" + num + ",\"num\":" + num + "}]}}";
            DbContext.Get().Db.Insertable(gift).ExecuteCommand();
        }
        [HttpPost("/u/u")]
        public IActionResult editUser(F2.user user)
        {
            if (string.IsNullOrEmpty(user.uuid))
            {
                return NotFound();
            }
            var dbh = DbContext.Get();
            uModel model = new uModel();
            model.user = dbh.Db.Queryable<F2.user>().IgnoreColumns(ii => new { ii.player_data, ii.player_zhong_yao }).First(ii => ii.uuid == user.uuid);
            if (model.user == null)
            {
                return NotFound();
            }
            model.user.isBan = user.isBan;
            model.user.password = user.password;
            if (model.user.shl != user.shl)
            {
                model.user.cheatMsg += ";shl+" + (user.shl - model.user.shl);
                model.user.shl = user.shl;
            }
            if (model.user.bdshl != user.bdshl)
            {
                model.user.cheatMsg += ";bdshl+" + (user.bdshl - model.user.bdshl);
                model.user.bdshl = user.bdshl;
            }
            dbh.Db.Updateable<F2.user>(model.user)
                .UpdateColumns(ii => new { ii.isBan, ii.password, ii.shl, ii.bdshl, ii.cheatMsg })
                .ExecuteCommand();
            return Redirect("/u?uid=" + user.uuid + "&ok=1");
        }
        [HttpPost("/u/s")]
        public IActionResult editSectinfo(F2.sect_member sect_Member)
        {
            if (string.IsNullOrEmpty(sect_Member.playerUuid))
            {
                return NotFound();
            }
            var dbh = DbContext.Get();
            var sectinfo = dbh.Db.Queryable<F2.sect_member>().First(ii => ii.playerUuid == sect_Member.playerUuid);
            if (sectinfo == null)
            {
                return NotFound();
            }
            sectinfo.sectId = sect_Member.sectId;
            sectinfo.position_level = sect_Member.position_level;
            sectinfo.sect_coin = sect_Member.sect_coin;
            sectinfo.CanAttackBossCnt = sect_Member.CanAttackBossCnt;
            sectinfo.CanAckDimBossCnt = sect_Member.CanAckDimBossCnt;

            dbh.Db.Updateable(sectinfo)
                .UpdateColumns(ii => new { ii.sectId, ii.position_level, ii.sect_coin, ii.CanAttackBossCnt, ii.CanAckDimBossCnt })
                .ExecuteCommand();
            return Redirect("/u?uid=" + sect_Member.playerUuid + "&ok=1");
        }

        [HttpPost("/u/g")]

        public IActionResult gift(string uuid, int code, int itemtype, int itemid, int num)
        {
            if (string.IsNullOrEmpty(uuid))
            {
                return NotFound();
            }
            gavegift(uuid, code, itemtype, itemid, num);
            return Redirect("/u?uid=" + uuid + "&ok=1");
        }
        [HttpPost("/u/h")]

        public IActionResult giftPoint(string uuid, int code, int num)
        {
            if (string.IsNullOrEmpty(uuid))
            {
                return NotFound();
            }
            gavegifthyJiFen(uuid, code, num);
            return Redirect("/u?uid=" + uuid + "&ok=1");
        }
        [Route("/au")]

        public IActionResult au()
        {
            return View();
        }
        [HttpPost("/aus")]

        public IActionResult aus(string username, string pwd, int shl)
        {
            if (!checkCookie()) return NotFound();

            int aa = DbContext.Get().Db.Insertable(new F2.user()
            {
                username=username,
                password=pwd,
                shl=shl,
                lastLoginTime=DateTime.Now,
                lastGetShlTime=DateTime.Now
            }).ExecuteCommand();

            if (aa==1)
            {
                return Redirect("/au?ok=1");
            }
            else
            {
                return Redirect("/au?ok=0");
            }

        }
    }
}