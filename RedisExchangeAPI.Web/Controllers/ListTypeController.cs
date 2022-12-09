using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;

        private readonly IDatabase db;

        private string listKey = "names";

        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(1);
        }

        public IActionResult Index()
        {
            List<string> namesList = new List<string>();
            if (db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }

            return View(namesList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            //ListRightPush: redis db de oluşturulacak listede kaydı en sona ekler.
            db.ListRightPush(listKey, name);

            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            //ListRemove: redis db de oluşturulan kayıtlardan seçili olan kayıt silinir(çıkarılır).
            db.ListRemoveAsync(listKey, name).Wait();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteFirstItem()
        {
            //ListLeftPop: redis db de oluşturulan listede en baştaki kayıt silinir(çıkarılır).
            db.ListLeftPop(listKey);

            return RedirectToAction("Index");
        }
    }
}
