using Microsoft.Extensions.Configuration;
using StackExchange.Redis;//Nuget kütüphanesinden eklenmeli.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Services
{
    public class RedisService
    {
        private readonly string _redisHost;

        private readonly string _redisPort;
        private ConnectionMultiplexer _redis;
        public IDatabase db { get; set; }

        public RedisService(IConfiguration configuration)
        {
            //redis Host - Port appsettings.json içerisinden alınır.
            _redisHost = configuration["Redis:Host"];

            _redisPort = configuration["Redis:Port"];
        }

        public void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";

            //ConnectionMultiplexer.Connect: Belirtilen Host - Port üzerinden bağlantı kurulur.
            _redis = ConnectionMultiplexer.Connect(configString);
        }

        public IDatabase GetDb(int db)
        {
            //GetDatabase: redis sunucusunda belirtilen db ye erişim sağlar.
            return _redis.GetDatabase(db);
        }
    }
}
