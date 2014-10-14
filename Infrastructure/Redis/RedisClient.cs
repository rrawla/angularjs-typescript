using System;
using System.Linq;
using StackExchange.Redis;

namespace Infrastructure.Redis
{
    public class RedisClient
    {

        public readonly ConnectionMultiplexer RedisConnection;

        private readonly IDatabase _redisDb;
        internal RedisClient(ConnectionMultiplexer connection)
        {
            RedisConnection = connection;
            _redisDb = connection.GetDatabase();
        }

        public string Get(string key)
        {
            return _redisDb.StringGet(key);
        }

        public long GetNextSequence(string key)
        {
            return _redisDb.StringIncrement(key);
        }

        public void Set(string key, string value, TimeSpan? expiry = null)
        {
            _redisDb.StringSet(key, value, expiry);
        }

        public long Push(string key, string[] values)
        {
            RedisValue[] rValues = values.Select(v => (RedisValue)v).ToArray();
            return _redisDb.ListLeftPush(key, rValues);

        }

        public long Push(string key, string value)
        {
            RedisValue[] rValues = { value };
            return _redisDb.ListLeftPush(key, rValues);

        }

        public string Pop(string key)
        {
            return _redisDb.ListRightPop(key);

        }

        public string[] GetListItems(string key)
        {
            long count = _redisDb.ListLength(key);
            string[] values = _redisDb.ListRange(key, 0, count - 1).Select(v => (string)v).ToArray();
            return values;

        }
 
        public RedisClient Subscribe(string channelName, Action<string, string> handler)
        {
            if (handler != null)
            {
                ISubscriber sub = RedisConnection.GetSubscriber();
                sub.Subscribe(channelName, (channel, message) =>
                {
                    handler(channel, message);
                });
            }

            return this;
        }

        public RedisClient Publish(string channelName, string message)
        {
            ISubscriber sub = RedisConnection.GetSubscriber();
            sub.Publish(channelName, message);

            return this;
        }

        public void SetAdd(string key, string value)
        {
            _redisDb.SetAdd(key, value);
        }

        public string[] SetMembers(string key)
        {
           return _redisDb.SetMembers(key).Select(v => (string)v).ToArray();
        }
    }
}
