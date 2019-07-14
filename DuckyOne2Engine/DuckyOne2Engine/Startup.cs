using Owin;
using System.Web.Http;

namespace DuckyOne2Engine
{
    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            var config = new HttpConfiguration();

            config.Routes
                .MapHttpRoute
                (
                    "SetAllColors",
                    "api/v1/{controller}/all/{rgb}"
                );

            builder.UseWebApi(config);
        }
    }
}
