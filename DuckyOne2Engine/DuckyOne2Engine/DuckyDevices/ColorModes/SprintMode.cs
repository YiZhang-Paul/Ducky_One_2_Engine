using DuckyOne2Engine.ColorControls;
using DuckyOne2Engine.KeyMappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DuckyOne2Engine.DuckyDevices.ColorModes
{
    public class SprintMode : IColorMode
    {
        private readonly string[][] _arrowKeys =
        {
            new[] { Keys.Two, Keys.W, Keys.S, Keys.Z },
            new[] { Keys.Three, Keys.E, Keys.D, Keys.X },
            new[] { Keys.Four, Keys.R, Keys.F, Keys.C },
            new[] { Keys.Five, Keys.T, Keys.G, Keys.V },
            new[] { Keys.Six, Keys.Y, Keys.H, Keys.B },
            new[] { Keys.Seven, Keys.U, Keys.J, Keys.N },
            new[] { Keys.Eight, Keys.I, Keys.K, Keys.M },
            new[] { Keys.Nine, Keys.O, Keys.L, Keys.Comma },
            new[] { Keys.Zero, Keys.P, Keys.Semicolon, Keys.Period },
            new[] { Keys.Hyphen, Keys.Lbracket, Keys.Quote, Keys.Question }
        };

        private const int BreathSteps = 20;
        private const int DropSteps = 30;
        private const int MaxDrops = 45;

        private int CurrentBreathStep { get; set; }
        private int Position { get; set; }
        private int Speed { get; }
        private bool IsSprinting { get; set; } = true;
        private Dictionary<string, int> Drops { get; } = new Dictionary<string, int>();
        private byte[] BackRgb { get; }
        private byte[] DropRgb { get; }
        private byte[] SprintRgb { get; }

        public SprintMode(byte[] backRgb, byte[] dropRgb, byte[] sprintRgb, int speed = 30)
        {
            BackRgb = backRgb;
            DropRgb = dropRgb;
            SprintRgb = sprintRgb;
            Speed = speed;
        }

        public void Setup(IColorControl colorControl)
        {
            colorControl.SetAll(BackRgb);
            colorControl.ApplyColors();
            Task.Run(() => Sprint(colorControl));
        }

        public void Unload()
        {
            IsSprinting = false;
        }

        private void Sprint(IColorControl colorControl, bool off = true)
        {
            while (IsSprinting)
            {
                Thread.Sleep(Speed);
                off = off ? CurrentBreathStep >= 1 : CurrentBreathStep + 1 > BreathSteps;
                SetBreathColor(colorControl, off);
                SetDropsColor(colorControl);
                SetArrowColor(colorControl);
                colorControl.ApplyColors();
            }
        }

        private void SetBreathColor(IColorControl colorControl, bool off)
        {
            CurrentBreathStep = off ? --CurrentBreathStep : ++CurrentBreathStep;
            colorControl.SetAll(NextColor(BackRgb, BreathSteps, CurrentBreathStep));
        }

        private void SetArrowColor(IColorControl colorControl)
        {
            colorControl.SetColors(_arrowKeys[Position], SprintRgb);
            Position = Position > _arrowKeys.Length - 2 ? 0 : Position + 1;
            colorControl.SetColors(_arrowKeys[Position], SprintRgb);
        }

        private void SetDropsColor(IColorControl colorControl)
        {
            if (Drops.Count < MaxDrops)
            {
                Drops[Keys.RandomKey] = DropSteps;
            }

            foreach (var key in Drops.Keys.ToList())
            {
                var color = NextColor(DropRgb, DropSteps, Drops[key]);
                colorControl.SetColor(new KeyColor(key, color));

                if (--Drops[key] == 0)
                {
                    Drops.Remove(key);
                }
            }
        }

        private byte[] NextColor(byte[] color, int totalSteps, int currentStep)
        {
            byte NextValue(byte value, byte min = 35)
            {
                if (value < min)
                {
                    return value;
                }

                var delta = (int)Math.Ceiling((double)(value - min) / totalSteps);

                return (byte)Math.Max(min, value - (totalSteps - currentStep) * delta);
            }

            var r = NextValue(color[0]);
            var g = NextValue(color[1]);
            var b = NextValue(color[2]);

            return new[] { r, g, b };
        }
    }
}
