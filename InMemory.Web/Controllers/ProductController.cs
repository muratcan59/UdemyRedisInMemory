using InMemory.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemory.Web.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            //Set: Cache data eklenir.
            _memoryCache.Set<string>("zaman", DateTime.Now.ToString());

            //1.yol
            if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }

            //2.yol
            //TryGetValue Cache'da belirtilen key üzerinde data olup olmadığı kontrol edilir.
            if (!_memoryCache.TryGetValue("zaman", out string zamanCache))
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

            //AbsoluteExpiration: Cache için verilen zaman kadar veriyi sistemde tutar.
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);

            //SlidingExpiration: Cache için verilen zaman içerisinde belirlenen yerde erişim olmadığı zaman veriyi sistemden siler.
            //options.SlidingExpiration = TimeSpan.FromSeconds(10);
            //Priority High: Data önemli silinmesin.
            //Priority Low: Data önemli değil cache dolduğunda ilk önce silinebilir.
            //Priority NeverRemove: Data kesinlikle silinmesin.
            //Priority Normal: Data cache doluluğa bağlı olarak silinip silinmeyeceğine karar verilir.
            options.Priority = CacheItemPriority.High;

            //RegisterPostEvictionCallback: Cache için verilen süre dolduktan sonra belirtilen event tetiklenir.
            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}->{value} => sebep:{reason}");
            });

            _memoryCache.Set<string>("zaman", DateTime.Now.ToString());

            Product p = new Product { Id = 1, Name = "Kalem", Price = 200 };

            _memoryCache.Set<Product>("product:1", p);
            _memoryCache.Set<double>("money", 100.99);

            return View();
        }

        public IActionResult Show()
        {
            //Remove: Cache'da belirtilen key silinir.(temizlenir)
            _memoryCache.Remove("zaman");

            //GetOrCreate: Cache'da belirtilen key üzerinden value(data) kontrol yapılır. Value yoksa key için belirtilen value eklenir.
            _memoryCache.GetOrCreate<string>("zaman", entry =>
            {
                return DateTime.Now.ToString();
            });

            _memoryCache.TryGetValue("zaman", out string zamancache);
            _memoryCache.TryGetValue("callback", out string callback);

            //Get: Cache'e eklenen data getirilir.
            ViewBag.zaman = zamancache;
            ViewBag.callback = callback;
            ViewBag.product = _memoryCache.Get<Product>("product:1");
            return View();
        }
    }
}
