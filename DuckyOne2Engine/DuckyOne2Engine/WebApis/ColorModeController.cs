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
            Device.Use(new ProgressMode(back, inner, outer, meta.Speed));
        }

        [Route("wave")]
        public void PostWaveMode([FromBody]WaveModeDto meta)
        {
            if (!IsValidRgb(meta.BackRgb) || !IsValidRgb(meta.WaveRgb))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var back = ParseRgb(meta.BackRgb);
            var wave = ParseRgb(meta.WaveRgb);
            Device.Use(new WaveMode(back, wave));
        }

        [Route("blink")]
        public void PostBlinkMode([FromBody]BlinkModeDto meta)
        {
            if (!IsValidRgb(meta.BackRgb) || !IsValidRgb(meta.BlinkRgb) || !IsValidRgb(meta.SpecialRgb))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var back = ParseRgb(meta.BackRgb);
            var blink = ParseRgb(meta.BlinkRgb);
            var special = ParseRgb(meta.SpecialRgb);
            Device.Use(new BlinkMode(back, blink, special, meta.SpecialKeys, meta.Interval));
        }

        [Route("shift")]
        public void PostShiftMode([FromBody]ShiftModeDto meta)
        {
            var rgbs = meta.BackRgbs.Split('|');

            if (!rgbs.All(IsValidRgb))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var backs = rgbs.Select(ParseRgb).ToArray();
            Device.Use(new ShiftMode(backs, meta.Interval));
        }

        [Route("sprint")]
        public void PostSprintMode([FromBody]SprintModeDto meta)
        {
            if (!IsValidRgb(meta.BackRgb) || !IsValidRgb(meta.DropRgb) || !IsValidRgb(meta.SprintRgb))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var back = ParseRgb(meta.BackRgb);
            var drop = ParseRgb(meta.DropRgb);
            var sprint = ParseRgb(meta.SprintRgb);
            Device.Use(new SprintMode(back, drop, sprint, meta.Speed));
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
