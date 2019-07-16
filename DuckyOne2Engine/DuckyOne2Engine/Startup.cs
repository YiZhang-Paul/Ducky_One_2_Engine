using Owin;
using System.Web.Http;

namespace DuckyOne2Engine
{
    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            builder.UseWebApi(config);
        }
    }
}
