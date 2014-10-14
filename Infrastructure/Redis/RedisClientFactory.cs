using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Infrastructure.Redis
{
    public class RedisClientFactory
    {
        private static ConnectionMultiplexer _redisConnection;

        private readonly static string _redisConnectionString = "localhost:6379";

        public static RedisClient GetRedisClient()
        {
            if (_redisConnection == null)
            {
                _redisConnection = ConnectionMultiplexer.Connect(_redisConnectionString);
            }
            return new RedisClient(_redisConnection);
        }

        public static RedisTypedClient<T> GetRedisTypedClient<T>()
        {
            if (_redisConnection == null)
            {
                _redisConnection = ConnectionMultiplexer.Connect(_redisConnectionString);
            }
            return new RedisTypedClient<T>(_redisConnection);
        }
    }
}
