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
                "SetAllColors",
                "api/v1/{controller}/all/{rgb}"
            );

            config.Routes.MapHttpRoute
            (
                "SetColor",
                "api/v1/{controller}/{key}/{rgb}"
            );

            config.Routes.MapHttpRoute
            (
                "ManualApplyColors",
                "api/v1/{controller}/apply"
            );

            config.Routes.MapHttpRoute
            (
                "SetMode",
                "api/v1/{controller}/{mode}/{backRgb}"
            );

            builder.UseWebApi(config);
        }
    }
}
