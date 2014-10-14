using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Infrastructure.Data;
using Infrastructure.Redis;
using System.Net.Http;
using System.Net;

namespace WebApp.Controllers
{
    public class TeamsController : ApiController
    {
        // GET: api/Teams
        public HttpResponseMessage Get()
        {
            var typedClient = RedisClientFactory.GetRedisTypedClient<Team>();
            System.Threading.Thread.Sleep(2000);
            return Request.CreateResponse(HttpStatusCode.OK, typedClient.GetAll());
        }

        // GET: api/Teams/5
        public HttpResponseMessage Get(int id)
        {
            var typedClient = RedisClientFactory.GetRedisTypedClient<Team>();
            return Request.CreateResponse(HttpStatusCode.OK, typedClient.GetById(id));
        }

        // POST: api/Teams
        public HttpResponseMessage Post([FromBody]Team value)
        {
            var typedClient = RedisClientFactory.GetRedisTypedClient<Team>();
            value.TeamId = typedClient.GetNextSequence();

            typedClient.Store(value);

            return Request.CreateResponse(HttpStatusCode.Created, value);
        }

        // PUT: api/Teams/5
        public HttpResponseMessage Put(int id, [FromBody]Team value)
        {
            var typedClient = RedisClientFactory.GetRedisTypedClient<Team>();
            typedClient.Store(value);

            return Request.CreateResponse(HttpStatusCode.OK, value);
        }

        // DELETE: api/Teams/5
        public void Delete(int id)
        {
        }
    }
}
