using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase db;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            var db = _redisService.GetDb(0);

            //StringSet: redis db ye belirtilen key value eklenir.
            db.StringSet("name", "Murat Döngel");
            db.StringSet("ziyaretci", 100);

            return View();
        }

        public IActionResult Show()
        {
            //StringGet: redis db den belirtilen key den value alınır.
            //var value = db.StringGet("name");
            //StringGetRange: redis db den belirtilen key den value içerisindeki ifadelerin belirlenen aralıkları alınır.
            //var value = db.StringGetRange("name", 0, 3);
            //StringLength: redis db den belirtilen key den value ya ait uzunluk alınır.
            var value = db.StringLength("name");

            Byte[] resimByte = default(byte[]);

            db.StringSet("resim", resimByte);

            //StringIncrement: redis db den belirtilen key den value(değer sayısal ise) belirtilen miktarda artırılır.
            db.StringIncrement("ziyaretci", 10);

            //StringDecrement: redis db den belirtilen key den value(değer sayısal ise) belirtilen miktarda azaltılır.
            //var count = db.StringDecrementAsync("ziyaretci", 1).Result;

            db.StringDecrementAsync("ziyaretci", 1).Wait();

            //if (value.HasValue)
            //{
                ViewBag.value = value.ToString();
            //}

            return View();
        }
    }
}
