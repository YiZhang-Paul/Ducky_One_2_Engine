using DuckyOne2Engine.ColorControls;
using Gma.System.MouseKeyHook;
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
        private int Steps { get; }
        private bool IsPressed { get; set; }
        private byte[] BackRgb { get; }
        private byte[] ActiveRgb { get; }
        private IColorControl ColorControl { get; }
        private Dictionary<string, int> KeyPressed { get; } = new Dictionary<string, int>();

        public ReactiveMode(IColorControl colorControl, byte[] backRgb, byte[] activeRgb, int steps = 60)
        {
            ColorControl = colorControl;
            BackRgb = backRgb;
            ActiveRgb = activeRgb;
            Steps = steps;
        }

        public void Setup()
        {
            ColorControl.SetAll(BackRgb);
            ColorControl.ApplyColors();
            Hook.GlobalEvents().KeyUp += OnKeyUp;
            Hook.GlobalEvents().KeyDown += OnKeyDown;
        }

        public void Unload()
        {
            Hook.GlobalEvents().KeyUp -= OnKeyUp;
            Hook.GlobalEvents().KeyDown -= OnKeyDown;
        }

        private async void OnKeyUp(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode.ToString();

            if (!ColorControl.SetColor(new KeyColor(key, ActiveRgb)))
            {
                return;
            }

            KeyPressed[key] = Steps;

            if (!IsPressed)
            {
                IsPressed = true;
                await Task.Run(TurnOffColors);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode.ToString();

            if (!ColorControl.SetColor(new KeyColor(key, ActiveRgb)))
            {
                return;
            }

            ColorControl.ApplyColors();

            if (KeyPressed.ContainsKey(key))
            {
                KeyPressed.Remove(key);
            }
        }

        private async Task TurnOffColors()
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

            ColorControl.ApplyColors();
            IsPressed = KeyPressed.Any();

            if (IsPressed)
            {
                Thread.Sleep(15);
                await Task.Run(TurnOffColors);
            }
        }

        private byte[] NextColor(int step)
        {
            byte NextValue(byte value)
            {
                var delta = (int)Math.Ceiling((double)value / Steps);

                return (byte)Math.Max(0, value - (Steps - step) * delta);
            }

            var r = NextValue(ActiveRgb[0]);
            var g = NextValue(ActiveRgb[1]);
            var b = NextValue(ActiveRgb[2]);

            if (r <= 60 && g <= 60 && b <= 60)
            {
                return BackRgb;
            }

            return new[] { r, g, b };
        }
    }
}
