using DuckyOne2Engine.ColorControls;
using DuckyOne2Engine.KeyMappers;
using System.Threading;
using System.Threading.Tasks;

namespace DuckyOne2Engine.DuckyDevices.ColorModes
{
    public class ProgressMode : IColorMode
    {
        private readonly string[] _innerKeys =
        {
            Keys.One, Keys.Q, Keys.A, Keys.Z,
            Keys.X, Keys.S, Keys.W, Keys.Two,
            Keys.Three, Keys.E, Keys.D, Keys.C,
            Keys.V, Keys.F, Keys.R, Keys.Four,
            Keys.Five, Keys.T, Keys.G, Keys.B,
            Keys.N, Keys.H, Keys.Y, Keys.Six,
            Keys.Seven, Keys.U, Keys.J, Keys.M,
            Keys.Comma, Keys.K, Keys.I, Keys.Eight,
            Keys.Nine, Keys.O, Keys.L, Keys.Period,
            Keys.Question, Keys.Semicolon, Keys.P, Keys.Zero,
            Keys.Hyphen, Keys.Lbracket, Keys.Quote,
            Keys.Rbracket, Keys.Equal
        };

        private readonly string[] _outerKeys =
        {
            Keys.Lctrl, Keys.Lshift, Keys.Caps, Keys.Tab, Keys.Backtick, Keys.Esc,
            Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12,
            Keys.Print, Keys.Scroll, Keys.Pause, Keys.Pageup, Keys.Pagedown, Keys.Rarrow, Keys.Darrow, Keys.Larrow,
            Keys.Rctrl, Keys.Fn, Keys.Rwindow, Keys.Ralt, Keys.Space, Keys.Lalt, Keys.Lwindow
        };

        private int InnerSpeed { get; }
        private int OuterSpeed { get; }
        private int InnerIndex { get; set; }
        private int OuterIndex { get; set; }
        private bool IsProgressing { get; set; } = true;
        private byte[] BackRgb { get; }
        private byte[] InnerRgb { get; }
        private byte[] OuterRgb { get; }

        public ProgressMode
        (
            byte[] backRgb,
            byte[] innerRgb,
            byte[] outerRgb,
            int innerSpeed = 90,
            int outerSpeed = 140
        )
        {
            BackRgb = backRgb;
            InnerRgb = innerRgb;
            OuterRgb = outerRgb;
            InnerSpeed = innerSpeed;
            OuterSpeed = outerSpeed;
        }

        public void Setup(IColorControl colorControl)
        {
            colorControl.SetAll(BackRgb);
            colorControl.ApplyColors();
            Task.Run(() => InnerProgress(colorControl));
            Task.Run(() => OuterProgress(colorControl));
        }

        public void Unload()
        {
            IsProgressing = false;
        }

        private void InnerProgress(IColorControl colorControl)
        {
            while (IsProgressing)
            {
                Thread.Sleep(InnerSpeed);
                colorControl.SetColor(new KeyColor(_innerKeys[InnerIndex], BackRgb));
                InnerIndex = InnerIndex + 1 > _innerKeys.Length - 1 ? 0 : InnerIndex + 1;
                colorControl.SetColor(new KeyColor(_innerKeys[InnerIndex], InnerRgb));
                colorControl.ApplyColors();
            }
        }

        private void OuterProgress(IColorControl colorControl)
        {
            while (IsProgressing)
            {
                Thread.Sleep(OuterSpeed);
                colorControl.SetColor(new KeyColor(_outerKeys[OuterIndex], BackRgb));
                OuterIndex = OuterIndex + 1 > _outerKeys.Length - 1 ? 0 : OuterIndex + 1;
                colorControl.SetColor(new KeyColor(_outerKeys[OuterIndex], OuterRgb));
            }
        }
    }
}
