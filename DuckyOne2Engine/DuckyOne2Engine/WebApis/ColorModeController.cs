using DuckyOne2Engine.Dtos;
using DuckyOne2Engine.DuckyDevices;
using DuckyOne2Engine.DuckyDevices.ColorModes;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace DuckyOne2Engine.WebApis
{
    [RoutePrefix("api/v1/colorMode")]
    public class ColorModeController : ApiController
    {
        private DuckyDevice Device { get; }

        public ColorModeController() : this(Cache.ActiveDuckyDevice) { }

        public ColorModeController(DuckyDevice device)
        {
            Device = device;
        }

        [Route("reactive")]
        public void PostReactiveMode([FromBody]ReactiveModeDto meta)
        {
            if (!IsValidRgb(meta.BackRgb) || !IsValidRgb(meta.ActiveRgb))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var back = ParseRgb(meta.BackRgb);
            var active = ParseRgb(meta.ActiveRgb);
            Device.Use(new ReactiveMode(back, active, meta.Steps));
        }

        [Route("blink")]
        public void PostBlinkMode([FromBody]BlinkModeDto meta)
        {
            if (!IsValidRgb(meta.BackRgb))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Device.Use(new BlinkMode(ParseRgb(meta.BackRgb), meta.Interval));
        }

        [Route("breath")]
        public void PostBreathMode([FromBody]BreathModeDto meta)
        {
            if (!IsValidRgb(meta.BackRgb))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Device.Use(new BreathMode(ParseRgb(meta.BackRgb), meta.Steps));
        }

        [Route("progress")]
        public void PostProgressMode([FromBody]ProgressModeDto meta)
        {
            if (!IsValidRgb(meta.BackRgb) || !IsValidRgb(meta.InnerRgb) || !IsValidRgb(meta.OuterRgb))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var back = ParseRgb(meta.BackRgb);
            var inner = ParseRgb(meta.InnerRgb);
            var outer = ParseRgb(meta.OuterRgb);
            Device.Use(new ProgressMode(back, inner, outer, meta.InnerSpeed, meta.OuterSpeed));
        }

        [Route("sprint")]
        public void PostSprintMode([FromBody]SprintModeDto meta)
        {
            if (!IsValidRgb(meta.BackRgb) || !IsValidRgb(meta.SprintRgb))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var back = ParseRgb(meta.BackRgb);
            var sprint = ParseRgb(meta.SprintRgb);
            Device.Use(new SprintMode(back, sprint, meta.Speed));
        }

        private bool IsValidRgb(string rgb)
        {
            return Regex.IsMatch(rgb, @"^\d+,\d+,\d+$");
        }

        private byte[] ParseRgb(string rgb)
        {
            return rgb.Split(',').Select(_ => Convert.ToByte(_)).ToArray();
        }
    }
}
