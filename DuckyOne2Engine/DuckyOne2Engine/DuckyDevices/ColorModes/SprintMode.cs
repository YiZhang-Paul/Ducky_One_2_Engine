using DuckyOne2Engine.ColorControls;
using DuckyOne2Engine.KeyMappers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DuckyOne2Engine.DuckyDevices.ColorModes
{
    public class SprintMode : IColorMode
    {
        private int Speed { get; }
        private int Position { get; set; }
        private bool IsSprinting { get; set; } = true;
        private byte[] BackRgb { get; }
        private byte[] SprintRgb { get; }
        private IColorControl ColorControl { get; }

        public SprintMode(IColorControl colorControl, byte[] backRgb, byte[] sprintRgb, int speed = 35)
        {
            ColorControl = colorControl;
            BackRgb = backRgb;
            SprintRgb = sprintRgb;
            Speed = speed;
        }

        public void Setup()
        {
            ColorControl.SetAll(BackRgb);
            ColorControl.ApplyColors();
            Task.Run(() => Sprint());
        }

        public void Unload()
        {
            IsSprinting = false;
        }

        private void Sprint()
        {
            while (IsSprinting)
            {
                Thread.Sleep(Speed);
                Position = Position < 26 ? Position + 1 : 0;

                if (Position <= 20)
                {
                    ColorControl.SetAll(BackRgb);
                    SetSprintColors();
                    ColorControl.ApplyColors();
                }
            }
        }

        private void SetSprintColors()
        {
            var colors = new List<KeyColor>();

            switch (Position)
            {
                case 1:
                    colors.Add(new KeyColor(Keys.Backtick, SprintRgb));
                    colors.Add(new KeyColor(Keys.Tab, SprintRgb));
                    colors.Add(new KeyColor(Keys.Caps, SprintRgb));
                    colors.Add(new KeyColor(Keys.Lshift, SprintRgb));
                    colors.Add(new KeyColor(Keys.Lctrl, SprintRgb));
                    break;
                case 2:
                    colors.Add(new KeyColor(Keys.Backtick, SprintRgb));
                    colors.Add(new KeyColor(Keys.Tab, SprintRgb));
                    colors.Add(new KeyColor(Keys.Caps, SprintRgb));
                    colors.Add(new KeyColor(Keys.Lshift, SprintRgb));
                    colors.Add(new KeyColor(Keys.Lctrl, SprintRgb));
                    colors.Add(new KeyColor(Keys.Esc, SprintRgb));
                    colors.Add(new KeyColor(Keys.One, SprintRgb));
                    colors.Add(new KeyColor(Keys.Q, SprintRgb));
                    colors.Add(new KeyColor(Keys.A, SprintRgb));
                    break;
                case 3:
                    colors.Add(new KeyColor(Keys.Backtick, SprintRgb));
                    colors.Add(new KeyColor(Keys.Tab, SprintRgb));
                    colors.Add(new KeyColor(Keys.Caps, SprintRgb));
                    colors.Add(new KeyColor(Keys.Lshift, SprintRgb));
                    colors.Add(new KeyColor(Keys.Lctrl, SprintRgb));
                    colors.Add(new KeyColor(Keys.Esc, SprintRgb));
                    colors.Add(new KeyColor(Keys.One, SprintRgb));
                    colors.Add(new KeyColor(Keys.Q, SprintRgb));
                    colors.Add(new KeyColor(Keys.A, SprintRgb));
                    colors.Add(new KeyColor(Keys.Two, SprintRgb));
                    colors.Add(new KeyColor(Keys.W, SprintRgb));
                    colors.Add(new KeyColor(Keys.S, SprintRgb));
                    colors.Add(new KeyColor(Keys.Z, SprintRgb));
                    colors.Add(new KeyColor(Keys.Lwindow, SprintRgb));
                    break;
                case 4:
                    colors.Add(new KeyColor(Keys.Lshift, SprintRgb));
                    colors.Add(new KeyColor(Keys.Lctrl, SprintRgb));
                    colors.Add(new KeyColor(Keys.Esc, SprintRgb));
                    colors.Add(new KeyColor(Keys.One, SprintRgb));
                    colors.Add(new KeyColor(Keys.Q, SprintRgb));
                    colors.Add(new KeyColor(Keys.A, SprintRgb));
                    colors.Add(new KeyColor(Keys.Two, SprintRgb));
                    colors.Add(new KeyColor(Keys.W, SprintRgb));
                    colors.Add(new KeyColor(Keys.S, SprintRgb));
                    colors.Add(new KeyColor(Keys.Z, SprintRgb));
                    colors.Add(new KeyColor(Keys.Lwindow, SprintRgb));
                    colors.Add(new KeyColor(Keys.F1, SprintRgb));
                    colors.Add(new KeyColor(Keys.Three, SprintRgb));
                    colors.Add(new KeyColor(Keys.E, SprintRgb));
                    colors.Add(new KeyColor(Keys.D, SprintRgb));
                    colors.Add(new KeyColor(Keys.X, SprintRgb));
                    colors.Add(new KeyColor(Keys.Lalt, SprintRgb));
                    break;
                case 5:
                    colors.Add(new KeyColor(Keys.Two, SprintRgb));
                    colors.Add(new KeyColor(Keys.W, SprintRgb));
                    colors.Add(new KeyColor(Keys.S, SprintRgb));
                    colors.Add(new KeyColor(Keys.Z, SprintRgb));
                    colors.Add(new KeyColor(Keys.Lwindow, SprintRgb));
                    colors.Add(new KeyColor(Keys.F1, SprintRgb));
                    colors.Add(new KeyColor(Keys.Three, SprintRgb));
                    colors.Add(new KeyColor(Keys.E, SprintRgb));
                    colors.Add(new KeyColor(Keys.D, SprintRgb));
                    colors.Add(new KeyColor(Keys.X, SprintRgb));
                    colors.Add(new KeyColor(Keys.Lalt, SprintRgb));
                    colors.Add(new KeyColor(Keys.F2, SprintRgb));
                    colors.Add(new KeyColor(Keys.Four, SprintRgb));
                    colors.Add(new KeyColor(Keys.R, SprintRgb));
                    colors.Add(new KeyColor(Keys.F, SprintRgb));
                    colors.Add(new KeyColor(Keys.C, SprintRgb));
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
                case 10:
                    break;
                case 11:
                    break;
                case 12:
                    break;
                case 13:
                    break;
                case 14:
                    break;
                case 15:
                    break;
                case 16:
                    break;
                case 17:
                    break;
                case 18:
                    break;
                case 19:
                    break;
                case 20:
                    break;
                default:
                    return;
            }

            ColorControl.SetColors(colors);
        }
    }
}
