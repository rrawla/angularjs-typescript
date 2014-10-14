using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Infrastructure.Redis
{
    public class RedisTypedClient<T> 
    {
        private readonly RedisClient _client;

        private readonly string _typeName = typeof(T).FullName;
        private readonly string _idsKey;
        private readonly string _idsSequence;
        
        internal RedisTypedClient(ConnectionMultiplexer connection)
        {
            _client = new RedisClient(connection);
            _idsKey = string.Format("{0}:ids", _typeName);

            _idsSequence = string.Format("{0}:sequence", _typeName);
        }

        public T GetById(string id)
        {
            string key = GetObjectKey(id);
            string valueEncoded = _client.Get(key);
            if (valueEncoded != null)
            {
                return JsonConvert.DeserializeObject<T>(valueEncoded);
            }
            else
            {
                return default(T);
            }
        }

        public T GetById(long id)
        {
            return this.GetById(id.ToString());
        }

        public T GetById(int id)
        {
            return this.GetById(id.ToString());
        }

        public T GetById(Guid id)
        {
            return this.GetById(id.ToString());
        }

        public IEnumerable<T> GetAll()
        {
            string[] ids = GetAllIds();
            List<T> items = new List<T>();
            foreach (var id in ids)
            {
                items.Add(this.GetById(id));
            }
            return items;
        }

        public string[] GetAllIds()
        {
            return _client.SetMembers(_idsKey);
        }

        public long GetNextSequence()
        {
            return _client.GetNextSequence(_idsSequence);
        }

        public void Store(T value, TimeSpan? expiry = null)
        {
            string valueId = GetObjectId(value);
            string key = GetObjectKey(valueId);
            string valueEncoded = JsonConvert.SerializeObject(value);
            _client.Set(key, valueEncoded, expiry);
            _client.SetAdd(_idsKey, valueId);
        }

        public long Push(string key, T[] values)
        {
            string[] rValues = values.Select(v => JsonConvert.SerializeObject(v)).ToArray();
            return _client.Push(key, rValues);

        }

        public long Push(string key, T value)
        {
            string[] rValues = { JsonConvert.SerializeObject(value) };
            return _client.Push(key, rValues);


        }

        public T Pop(string key)
        {
            string valueEncoded = _client.Pop(key);
            return (valueEncoded == null) ? default(T) : JsonConvert.DeserializeObject<T>(valueEncoded);

        }

        public T[] GetListItems(string key)
        {
            string[] values = _client.GetListItems(key);
            T[] typedValues = values.Select(v => JsonConvert.DeserializeObject<T>(v)).ToArray();
            return typedValues;

        }


        private string GetObjectKey(string id)
        {
            return string.Format("json:{0}:{1}", _typeName, id.ToLower());
        }

        private string GetObjectId(T value)
        {
            Type valueType = typeof(T);

            PropertyInfo[] properties = valueType.GetProperties();

            PropertyInfo idProperty = properties.Where(p => p.IsDefined(typeof(StorageKeyAttribute), true)).FirstOrDefault();

            if (idProperty == null)
            {
                string className = valueType.Name;
                idProperty = value.GetType().GetProperty(className + "Id");
                if (idProperty == null)
                {
                    idProperty = value.GetType().GetProperty(className + "ID");
                    if (idProperty == null)
                    {
                        idProperty = value.GetType().GetProperty(className + "id");
                        if (idProperty == null)
                        {
                            idProperty = value.GetType().GetProperty("Id");
                            if (idProperty == null)
                            {
                                idProperty = value.GetType().GetProperty("ID");
                                if (idProperty == null)
                                {
                                    idProperty = value.GetType().GetProperty("id");
                                }
                            }
                        }
                    }
                }

            }

            if (idProperty == null)
            {
                throw new InvalidOperationException(
                    string.Format("{0} does not have an 'Id' property nor a property defined with the 'KeyValueId' attribute.",
                    valueType.FullName));
            }
            else
            {
                return idProperty.GetValue(value, null).ToString();
            }
        }

        public RedisTypedClient<T> Subscribe(string channelName, Action<string, T> handler)
        {
            if (handler != null)
            {
                ISubscriber sub = _client.RedisConnection.GetSubscriber();
                sub.Subscribe(channelName, (channel, message) =>
                {
                    T value = JsonConvert.DeserializeObject<T>(message);
                    handler(channel, value);
                });
            }

            return this;

        }

        public RedisTypedClient<T> Publish(string channelName, T value)
        {
            ISubscriber sub = _client.RedisConnection.GetSubscriber();
            string encodedValue = JsonConvert.SerializeObject(value);
            sub.Publish(channelName, encodedValue);

            return this;

        }
    }
}
