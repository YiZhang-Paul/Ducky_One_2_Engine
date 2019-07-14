using DuckyOne2Engine.ColorControls;
using DuckyOne2Engine.KeyMappers;
using System.Collections.Generic;
using System.Linq;
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
        private HashSet<string> ActiveKeys { get; } = new HashSet<string>();
        private IColorControl ColorControl { get; }

        public SprintMode(IColorControl colorControl, byte[] backRgb, byte[] sprintRgb, int speed = 30)
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
                Position = Position < 24 ? Position + 1 : 0;
                ColorControl.SetAll(BackRgb);
                SetSprintColors();
                ColorControl.ApplyColors();
            }
        }

        private void SetSprintColors()
        {
            switch (Position)
            {
                case 1:
                    ActiveKeys.Add(Keys.Backtick);
                    ActiveKeys.Add(Keys.Tab);
                    ActiveKeys.Add(Keys.Caps);
                    ActiveKeys.Add(Keys.Lshift);
                    ActiveKeys.Add(Keys.Lctrl);
                    break;
                case 2:
                    ActiveKeys.Add(Keys.Esc);
                    ActiveKeys.Add(Keys.One);
                    ActiveKeys.Add(Keys.Q);
                    ActiveKeys.Add(Keys.A);
                    break;
                case 3:
                    ActiveKeys.Add(Keys.Two);
                    ActiveKeys.Add(Keys.W);
                    ActiveKeys.Add(Keys.S);
                    ActiveKeys.Add(Keys.Z);
                    ActiveKeys.Add(Keys.Lwindow);
                    break;
                case 4:
                    ActiveKeys.Remove(Keys.Backtick);
                    ActiveKeys.Remove(Keys.Tab);
                    ActiveKeys.Remove(Keys.Caps);
                    ActiveKeys.Add(Keys.F1);
                    ActiveKeys.Add(Keys.Three);
                    ActiveKeys.Add(Keys.E);
                    ActiveKeys.Add(Keys.D);
                    ActiveKeys.Add(Keys.X);
                    ActiveKeys.Add(Keys.Lalt);
                    break;
                case 5:
                    ActiveKeys.Remove(Keys.Lshift);
                    ActiveKeys.Remove(Keys.Lctrl);
                    ActiveKeys.Remove(Keys.Esc);
                    ActiveKeys.Remove(Keys.One);
                    ActiveKeys.Remove(Keys.Q);
                    ActiveKeys.Remove(Keys.A);
                    ActiveKeys.Add(Keys.F2);
                    ActiveKeys.Add(Keys.Four);
                    ActiveKeys.Add(Keys.R);
                    ActiveKeys.Add(Keys.F);
                    ActiveKeys.Add(Keys.C);
                    break;
                case 6:
                    ActiveKeys.Remove(Keys.Two);
                    ActiveKeys.Remove(Keys.W);
                    ActiveKeys.Remove(Keys.S);
                    ActiveKeys.Remove(Keys.Z);
                    ActiveKeys.Remove(Keys.Lwindow);
                    ActiveKeys.Add(Keys.F3);
                    ActiveKeys.Add(Keys.Five);
                    ActiveKeys.Add(Keys.T);
                    ActiveKeys.Add(Keys.G);
                    ActiveKeys.Add(Keys.V);
                    break;
                case 7:
                    ActiveKeys.Remove(Keys.F1);
                    ActiveKeys.Remove(Keys.Three);
                    ActiveKeys.Remove(Keys.E);
                    ActiveKeys.Remove(Keys.D);
                    ActiveKeys.Remove(Keys.X);
                    ActiveKeys.Remove(Keys.Lalt);
                    ActiveKeys.Add(Keys.F4);
                    ActiveKeys.Add(Keys.Six);
                    ActiveKeys.Add(Keys.Y);
                    ActiveKeys.Add(Keys.H);
                    ActiveKeys.Add(Keys.B);
                    break;
                case 8:
                    ActiveKeys.Remove(Keys.F2);
                    ActiveKeys.Remove(Keys.Four);
                    ActiveKeys.Remove(Keys.R);
                    ActiveKeys.Remove(Keys.F);
                    ActiveKeys.Remove(Keys.C);
                    ActiveKeys.Add(Keys.F5);
                    ActiveKeys.Add(Keys.Seven);
                    ActiveKeys.Add(Keys.U);
                    ActiveKeys.Add(Keys.J);
                    ActiveKeys.Add(Keys.N);
                    break;
                case 9:
                    ActiveKeys.Remove(Keys.F3);
                    ActiveKeys.Remove(Keys.Five);
                    ActiveKeys.Remove(Keys.T);
                    ActiveKeys.Remove(Keys.G);
                    ActiveKeys.Remove(Keys.V);
                    ActiveKeys.Add(Keys.F6);
                    ActiveKeys.Add(Keys.Eight);
                    ActiveKeys.Add(Keys.I);
                    ActiveKeys.Add(Keys.K);
                    ActiveKeys.Add(Keys.M);
                    break;
                case 10:
                    ActiveKeys.Remove(Keys.F4);
                    ActiveKeys.Remove(Keys.Six);
                    ActiveKeys.Remove(Keys.Y);
                    ActiveKeys.Remove(Keys.H);
                    ActiveKeys.Remove(Keys.B);
                    ActiveKeys.Add(Keys.F7);
                    ActiveKeys.Add(Keys.Nine);
                    ActiveKeys.Add(Keys.O);
                    ActiveKeys.Add(Keys.L);
                    ActiveKeys.Add(Keys.Comma);
                    break;
                case 11:
                    ActiveKeys.Remove(Keys.F5);
                    ActiveKeys.Remove(Keys.Seven);
                    ActiveKeys.Remove(Keys.U);
                    ActiveKeys.Remove(Keys.J);
                    ActiveKeys.Remove(Keys.N);
                    ActiveKeys.Add(Keys.F8);
                    ActiveKeys.Add(Keys.Zero);
                    ActiveKeys.Add(Keys.P);
                    ActiveKeys.Add(Keys.Semicolon);
                    ActiveKeys.Add(Keys.Period);
                    break;
                case 12:
                    ActiveKeys.Remove(Keys.F6);
                    ActiveKeys.Remove(Keys.Eight);
                    ActiveKeys.Remove(Keys.I);
                    ActiveKeys.Remove(Keys.K);
                    ActiveKeys.Remove(Keys.M);
                    ActiveKeys.Add(Keys.Hyphen);
                    ActiveKeys.Add(Keys.Lbracket);
                    ActiveKeys.Add(Keys.Quote);
                    ActiveKeys.Add(Keys.Question);
                    ActiveKeys.Add(Keys.Ralt);
                    break;
                case 13:
                    ActiveKeys.Remove(Keys.F7);
                    ActiveKeys.Remove(Keys.Nine);
                    ActiveKeys.Remove(Keys.O);
                    ActiveKeys.Remove(Keys.L);
                    ActiveKeys.Remove(Keys.Comma);
                    ActiveKeys.Add(Keys.F9);
                    ActiveKeys.Add(Keys.Equal);
                    ActiveKeys.Add(Keys.Rbracket);
                    ActiveKeys.Add(Keys.Enter);
                    ActiveKeys.Add(Keys.Rshift);
                    ActiveKeys.Add(Keys.Rwindow);
                    break;
                case 14:
                    ActiveKeys.Remove(Keys.F8);
                    ActiveKeys.Remove(Keys.Zero);
                    ActiveKeys.Remove(Keys.P);
                    ActiveKeys.Remove(Keys.Semicolon);
                    ActiveKeys.Remove(Keys.Period);
                    ActiveKeys.Add(Keys.F10);
                    ActiveKeys.Add(Keys.F11);
                    ActiveKeys.Add(Keys.Backspace);
                    ActiveKeys.Add(Keys.Pipe);
                    ActiveKeys.Add(Keys.Fn);
                    break;
                case 15:
                    ActiveKeys.Remove(Keys.Hyphen);
                    ActiveKeys.Remove(Keys.Lbracket);
                    ActiveKeys.Remove(Keys.Quote);
                    ActiveKeys.Remove(Keys.Question);
                    ActiveKeys.Remove(Keys.Ralt);
                    ActiveKeys.Add(Keys.F12);
                    ActiveKeys.Add(Keys.Delete);
                    ActiveKeys.Add(Keys.Rctrl);
                    ActiveKeys.Add(Keys.Fn);
                    break;
                case 16:
                    ActiveKeys.Remove(Keys.F9);
                    ActiveKeys.Remove(Keys.Equal);
                    ActiveKeys.Remove(Keys.Rbracket);
                    ActiveKeys.Remove(Keys.Rwindow);
                    ActiveKeys.Add(Keys.Insert);
                    ActiveKeys.Add(Keys.End);
                    break;
                case 17:
                    ActiveKeys.Remove(Keys.F10);
                    ActiveKeys.Add(Keys.Print);
                    ActiveKeys.Add(Keys.Home);
                    ActiveKeys.Add(Keys.Pagedown);
                    ActiveKeys.Add(Keys.Uarrow);
                    ActiveKeys.Add(Keys.Larrow);
                    break;
                case 18:
                    ActiveKeys.Remove(Keys.F11);
                    ActiveKeys.Remove(Keys.Backspace);
                    ActiveKeys.Remove(Keys.Pipe);
                    ActiveKeys.Remove(Keys.Enter);
                    ActiveKeys.Remove(Keys.Rshift);
                    ActiveKeys.Remove(Keys.Fn);
                    ActiveKeys.Add(Keys.Scroll);
                    ActiveKeys.Add(Keys.Pageup);
                    ActiveKeys.Add(Keys.Darrow);
                    break;
                case 19:
                    ActiveKeys.Add(Keys.Pause);
                    ActiveKeys.Add(Keys.Rarrow);
                    break;
                case 20:
                    ActiveKeys.Remove(Keys.F12);
                    ActiveKeys.Remove(Keys.Delete);
                    ActiveKeys.Remove(Keys.Rctrl);
                    break;
                case 21:
                    ActiveKeys.Remove(Keys.Print);
                    ActiveKeys.Remove(Keys.Insert);
                    ActiveKeys.Remove(Keys.End);
                    ActiveKeys.Remove(Keys.Uarrow);
                    ActiveKeys.Remove(Keys.Larrow);
                    break;
                case 22:
                    ActiveKeys.Remove(Keys.Home);
                    ActiveKeys.Remove(Keys.Pagedown);
                    ActiveKeys.Remove(Keys.Darrow);
                    break;
                case 23:
                    ActiveKeys.Remove(Keys.Scroll);
                    ActiveKeys.Remove(Keys.Pageup);
                    break;
                case 24:
                    ActiveKeys.Clear();
                    break;
                default:
                    return;
            }
            
            ColorControl.SetColors(ActiveKeys.Select(_ => new KeyColor(_, SprintRgb)));
        }
    }
}
