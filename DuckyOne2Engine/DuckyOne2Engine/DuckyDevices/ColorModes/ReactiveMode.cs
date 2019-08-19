using DuckyOne2Engine.ColorControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuckyOne2Engine.DuckyDevices.ColorModes
{
    public class ReactiveMode : IColorMode
    {
        private readonly object _keysLock = new object();

        private int Steps { get; }
        private bool IsActive { get; set; } = true;
        private byte[] BackRgb { get; }
        private byte[] ActiveRgb { get; }
        private byte[] MinActiveRgb { get; }
        private IColorControl ColorControl { get; set; }
        private Dictionary<string, int> KeyPressed { get; } = new Dictionary<string, int>();

        public ReactiveMode(byte[] backRgb, byte[] activeRgb, int steps = 60)
        {
            BackRgb = backRgb;
            ActiveRgb = activeRgb;
            MinActiveRgb = BackRgb.Select(_ => Math.Min(_, (byte)150)).ToArray();
            Steps = steps;
        }

        public void Setup(IColorControl colorControl)
        {
            ColorControl = colorControl;
            ColorControl.SetAll(BackRgb);
            ColorControl.ApplyColors();
            Cache.GlobalKeyboardEvents.KeyUp += OnKeyUp;
            Cache.GlobalKeyboardEvents.KeyDown += OnKeyDown;
            Task.Run(() => TurnOffColors());
        }

        public void Unload()
        {
            Cache.GlobalKeyboardEvents.KeyUp -= OnKeyUp;
            Cache.GlobalKeyboardEvents.KeyDown -= OnKeyDown;
            IsActive = false;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            lock (_keysLock)
            {
                KeyPressed[e.KeyCode.ToString()] = Steps;
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode.ToString();
            ColorControl.SetColor(new KeyColor(key, ActiveRgb));
            ColorControl.ApplyColors();

            if (KeyPressed.ContainsKey(key))
            {
                lock (_keysLock)
                {
                    KeyPressed.Remove(key);
                }
            }
        }

        private void TurnOffColors()
        {
            while (IsActive)
            {
                Thread.Sleep(30);

                lock (_keysLock)
                {
                    foreach (var key in KeyPressed.Keys.ToList())
                    {
                        var color = KeyPressed[key] == 0 ? BackRgb : NextColor(KeyPressed[key]);
                        ColorControl.SetColor(new KeyColor(key, color));

                        if (KeyPressed[key]-- == 0)
                        {
                            KeyPressed.Remove(key);
                        }
                    }
                }

                ColorControl.ApplyColors();
            }
        }

        private byte[] NextColor(int step)
        {
            byte NextValue(byte min, byte value)
            {
                var delta = (int)Math.Ceiling((double)(value - min) / Steps);

                return (byte)Math.Max(min, value - (Steps - step) * delta);
            }

            var r = NextValue(MinActiveRgb[0], ActiveRgb[0]);
            var g = NextValue(MinActiveRgb[1], ActiveRgb[1]);
            var b = NextValue(MinActiveRgb[2], ActiveRgb[2]);

            if (r <= MinActiveRgb[0] && g <= MinActiveRgb[1] && b <= MinActiveRgb[2])
            {
                return BackRgb;
            }

            return new[] { r, g, b };
        }
    }
}
