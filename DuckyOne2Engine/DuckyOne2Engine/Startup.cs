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
                "SetReactiveMode",
                "api/v1/{controller}/reactive/{backRgb}/{activeRgb}/{steps}"
            );

            config.Routes.MapHttpRoute
            (
                "SetBlinkMode",
                "api/v1/{controller}/blink/{backRgb}/{interval}"
            );

            config.Routes.MapHttpRoute
            (
                "SetBreathMode",
                "api/v1/{controller}/breath/{backRgb}/{steps}"
            );

            config.Routes.MapHttpRoute
            (
                "SetSprintMode",
                "api/v1/{controller}/sprint/{backRgb}/{sprintRgb}/{speed}"
            );

            builder.UseWebApi(config);
        }
    }
}
