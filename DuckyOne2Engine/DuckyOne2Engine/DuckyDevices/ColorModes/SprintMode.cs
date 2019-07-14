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
            string[] add;
            string[] remove;

            switch (Position)
            {
                case 1:
                    add = new[] { Keys.Backtick, Keys.Tab, Keys.Caps, Keys.Lshift, Keys.Lctrl };
                    SetActiveKeys(add, new string[0]);
                    break;
                case 2:
                    add = new[] { Keys.Esc, Keys.One, Keys.Q, Keys.A };
                    SetActiveKeys(add, new string[0]);
                    break;
                case 3:
                    add = new[] { Keys.Two, Keys.W, Keys.S, Keys.Z, Keys.Lwindow };
                    SetActiveKeys(add, new string[0]);
                    break;
                case 4:
                    add = new[] { Keys.F1, Keys.Three, Keys.E, Keys.D, Keys.X, Keys.Lalt };
                    remove = new[] { Keys.Backtick, Keys.Tab, Keys.Caps };
                    SetActiveKeys(add, remove);
                    break;
                case 5:
                    add = new[] { Keys.F2, Keys.Four, Keys.R, Keys.F, Keys.C };
                    remove = new[] { Keys.Lshift, Keys.Lctrl, Keys.Esc, Keys.One, Keys.Q, Keys.A };
                    SetActiveKeys(add, remove);
                    break;
                case 6:
                    add = new[] { Keys.F3, Keys.Five, Keys.T, Keys.G, Keys.V };
                    remove = new[] { Keys.Two, Keys.W, Keys.S, Keys.Z, Keys.Lwindow };
                    SetActiveKeys(add, remove);
                    break;
                case 7:
                    add = new[] { Keys.F4, Keys.Six, Keys.Y, Keys.H, Keys.B };
                    remove = new[] { Keys.F1, Keys.Three, Keys.E, Keys.D, Keys.X, Keys.Lalt };
                    SetActiveKeys(add, remove);
                    break;
                case 8:
                    add = new[] { Keys.F5, Keys.Seven, Keys.U, Keys.J, Keys.N };
                    remove = new[] { Keys.F2, Keys.Four, Keys.R, Keys.F, Keys.C };
                    SetActiveKeys(add, remove);
                    break;
                case 9:
                    add = new[] { Keys.F6, Keys.Eight, Keys.I, Keys.K, Keys.M };
                    remove = new[] { Keys.F3, Keys.Five, Keys.T, Keys.G, Keys.V };
                    SetActiveKeys(add, remove);
                    break;
                case 10:
                    add = new[] { Keys.F7, Keys.Nine, Keys.O, Keys.L, Keys.Comma };
                    remove = new[] { Keys.F4, Keys.Six, Keys.Y, Keys.H, Keys.B };
                    SetActiveKeys(add, remove);
                    break;
                case 11:
                    add = new[] { Keys.F8, Keys.Zero, Keys.P, Keys.Semicolon, Keys.Period };
                    remove = new[] { Keys.F5, Keys.Seven, Keys.U, Keys.J, Keys.N };
                    SetActiveKeys(add, remove);
                    break;
                case 12:
                    add = new[] { Keys.Hyphen, Keys.Lbracket, Keys.Quote, Keys.Question, Keys.Ralt };
                    remove = new[] { Keys.F6, Keys.Eight, Keys.I, Keys.K, Keys.M };
                    SetActiveKeys(add, remove);
                    break;
                case 13:
                    add = new[] { Keys.F9, Keys.Equal, Keys.Rbracket, Keys.Enter, Keys.Rshift, Keys.Rwindow };
                    remove = new[] { Keys.F7, Keys.Nine, Keys.O, Keys.L, Keys.Comma };
                    SetActiveKeys(add, remove);
                    break;
                case 14:
                    add = new[] { Keys.F10, Keys.F11, Keys.Backspace, Keys.Pipe, Keys.Fn };
                    remove = new[] { Keys.F8, Keys.Zero, Keys.P, Keys.Semicolon, Keys.Period };
                    SetActiveKeys(add, remove);
                    break;
                case 15:
                    add = new[] { Keys.F12, Keys.Delete, Keys.Rctrl };
                    remove = new[] { Keys.Hyphen, Keys.Lbracket, Keys.Quote, Keys.Question, Keys.Ralt };
                    SetActiveKeys(add, remove);
                    break;
                case 16:
                    add = new[] { Keys.Insert, Keys.End };
                    remove = new[] { Keys.F9, Keys.Equal, Keys.Rbracket, Keys.Rwindow };
                    SetActiveKeys(add, remove);
                    break;
                case 17:
                    add = new[] { Keys.Print, Keys.Home, Keys.Pagedown, Keys.Uarrow, Keys.Larrow };
                    remove = new[] { Keys.F10 };
                    SetActiveKeys(add, remove);
                    break;
                case 18:
                    add = new[] { Keys.Scroll, Keys.Pageup, Keys.Darrow };
                    remove = new[] { Keys.F11, Keys.Backspace, Keys.Pipe, Keys.Enter, Keys.Rshift, Keys.Fn };
                    SetActiveKeys(add, remove);
                    break;
                case 19:
                    add = new[] { Keys.Pause, Keys.Rarrow };
                    SetActiveKeys(add, new string[0]);
                    break;
                case 20:
                    remove = new[] { Keys.F12, Keys.Delete, Keys.Rctrl };
                    SetActiveKeys(new string[0], remove);
                    break;
                case 21:
                    remove = new[] { Keys.Print, Keys.Insert, Keys.End, Keys.Uarrow, Keys.Larrow };
                    SetActiveKeys(new string[0], remove);
                    break;
                case 22:
                    remove = new[] { Keys.Home, Keys.Pagedown, Keys.Darrow };
                    SetActiveKeys(new string[0], remove);
                    break;
                case 23:
                    remove = new[] { Keys.Scroll, Keys.Pageup };
                    SetActiveKeys(new string[0], remove);
                    break;
                case 24:
                    ActiveKeys.Clear();
                    break;
                default:
                    return;
            }
            
            ColorControl.SetColors(ActiveKeys.Select(_ => new KeyColor(_, SprintRgb)));
        }

        private void SetActiveKeys(IEnumerable<string> add, IEnumerable<string> remove)
        {
            foreach (var key in add)
            {
                ActiveKeys.Add(key);
            }

            foreach (var key in remove)
            {
                ActiveKeys.Remove(key);
            }
        }
    }
}
