using DuckyOne2Engine.ColorControl;
using DuckyOne2Engine.HidDevices;
using DuckyOne2Engine.KeyMappers;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuckyOne2Engine
{
    class Program
    {
        static void Main(string[] args)
        {
            var name = "vid_04d9&pid_0356&mi_01";
            var devices = HidDevice.GetConnectedDevices();
            var path = devices.FirstOrDefault(_ => Regex.IsMatch(_.DevicePath, name)).DevicePath;

            if (path?.Length > 0)
            {
                var device = new HidDevice(path, false);
                SendCommandFromFile(device, "Commands/open.txt");
                Thread.Sleep(1500);
                SetupKeyboard(device);
                Application.Run(new ApplicationContext());
            }
        }

        static void SendCommandFromFile(IHidDevice device, string name)
        {
            foreach (var line in File.ReadAllLines(name))
            {
                var hex = line.Trim().Split(' ');
                device.Write(hex.Select(_ => Convert.ToByte(_, 16)).ToArray());
            }
        }

        static void SetupKeyboard(IHidDevice device)
        {
            var controller = new ColorControl.ColorControl(device, new KeyColorMapper());
            var backColor = new byte[] { 1, 28, 73 };
            var activeColor = new byte[] { 255, 255, 255 };
            new CustomReactiveMode(controller, backColor, activeColor);
            
            Hook.GlobalEvents().OnCombination(new Dictionary<Combination, Action>
            {
                { Combination.FromString("Control+Shift+M"), () => Exit(device) }
            });
        }

        static void Exit(IHidDevice device)
        {
            SendCommandFromFile(device, "Commands/open.txt");
            Application.Exit();
        }

        class CustomReactiveMode
        {
            private int DelaySteps { get; }
            private int MaxSteps { get; }
            private bool IsPressed { get; set; }
            private byte[] BackColor { get; }
            private byte[] ActiveColor { get; }
            private ColorControl.ColorControl ColorControl { get; }
            private Dictionary<string, int> KeyPressed { get; } = new Dictionary<string, int>();

            public CustomReactiveMode(ColorControl.ColorControl colorControl, byte[] backColor, byte[] activeColor, int maxSteps = 60)
            {
                ColorControl = colorControl;
                BackColor = backColor;
                ActiveColor = activeColor;
                MaxSteps = maxSteps;
                Setup();
            }

            private void Setup()
            {
                ColorControl.SetAll(BackColor);
                ColorControl.ApplyColors();

                Hook.GlobalEvents().KeyUp += async (sender, e) =>
                {
                    var key = e.KeyCode.ToString();

                    if (!ColorControl.SetColor(new KeyColor(key, ActiveColor)))
                    {
                        return;
                    }

                    KeyPressed[key] = MaxSteps + DelaySteps;

                    if (!IsPressed)
                    {
                        IsPressed = true;
                        await Task.Run(TurnOffColors);
                    }
                };

                Hook.GlobalEvents().KeyDown += (sender, e) =>
                {
                    var key = e.KeyCode.ToString();

                    if (ColorControl.SetColor(new KeyColor(key, ActiveColor)))
                    {
                        ColorControl.ApplyColors();
                    }
                };
            }

            private async Task TurnOffColors()
            {
                foreach (var key in KeyPressed.Keys.ToList())
                {
                    if (KeyPressed[key] >= MaxSteps)
                    {
                        KeyPressed[key]--;

                        continue;
                    }

                    var color = KeyPressed[key] == 0 ? BackColor : NextColor(KeyPressed[key]);
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
                    var delta = (int)Math.Ceiling((double)value / MaxSteps);

                    return (byte)Math.Max(0, value - (MaxSteps - step) * delta);
                }

                var r = NextValue(ActiveColor[0]);
                var g = NextValue(ActiveColor[1]);
                var b = NextValue(ActiveColor[2]);

                if (r <= 60 && g <= 60 && b <= 60)
                {
                    return BackColor;
                }

                return new[] { r, g, b };
            }
        }
    }
}
