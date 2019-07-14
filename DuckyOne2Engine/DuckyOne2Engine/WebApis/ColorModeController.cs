using DuckyOne2Engine.DuckyDevices;
using DuckyOne2Engine.DuckyDevices.ColorModes;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace DuckyOne2Engine.WebApis
{
    public class ColorModeController : ApiController
    {
        private DuckyDevice Device { get; }

        public ColorModeController() : this(Cache.ActiveDuckyDevice) { }

        public ColorModeController(DuckyDevice device)
        {
            Device = device;
        }

        private bool IsValidRgb(string rgb)
        {
            return Regex.IsMatch(rgb, @"^\d+-\d+-\d+$");
        }

        private byte[] ParseRgb(string rgb)
        {
            return rgb.Split('-').Select(_ => Convert.ToByte(_)).ToArray();
        }

        public void PostReactiveMode(string backRgb, string activeRgb, int steps)
        {
            if (!IsValidRgb(backRgb) || !IsValidRgb(activeRgb))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Device.Use(new ReactiveMode(ParseRgb(backRgb), ParseRgb(activeRgb), steps));
        }

        public void PostBlinkMode(string backRgb, int interval)
        {
            if (!IsValidRgb(backRgb))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Device.Use(new BlinkMode(ParseRgb(backRgb), interval));
        }

        public void PostBreathMode(string backRgb, int steps)
        {
            if (!IsValidRgb(backRgb))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Device.Use(new BreathMode(ParseRgb(backRgb), steps));
        }

        public void PostSprintMode(string backRgb, string sprintRgb, int speed)
        {
            if (!IsValidRgb(backRgb) || !IsValidRgb(sprintRgb))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Device.Use(new SprintMode(ParseRgb(backRgb), ParseRgb(sprintRgb), speed));
        }
    }
}
