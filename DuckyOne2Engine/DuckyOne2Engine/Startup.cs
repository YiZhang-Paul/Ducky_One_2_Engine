using Owin;
using System.Web.Http;

namespace DuckyOne2Engine
{
    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute
            (
                "SetMode",
                "api/v1/{controller}/{mode}/{backRgb}"
            );

            builder.UseWebApi(config);
        }
    }
}
