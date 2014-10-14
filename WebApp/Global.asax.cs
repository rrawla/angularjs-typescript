using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Infrastructure.Data;
using Infrastructure.Redis;
using Microsoft.AspNet.SignalR;
using WebApp.Hubs;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var client = RedisClientFactory.GetRedisTypedClient<ScoreUpdate>();

            client.Subscribe("scoreupdate", (channel, message) =>
                {
                    IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<ScoresHub>();
                    hubContext.Clients.All.addPointsToTeam(message.TeamId, message.Points);
                });


        }
    }
}
