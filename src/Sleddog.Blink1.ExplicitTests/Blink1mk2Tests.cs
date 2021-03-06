﻿using System;
using System.Drawing;
using System.Threading;
using Xunit;

namespace Sleddog.Blink1.ExplicitTests
{
    public class Blink1Mk2Tests : IUseFixture<Blink1Mk2Fixture>
    {
        private const string LowestSerialNumber = "0x20001000";
        private const string HighestSerialNumber = "0x20003710";

        private IBlink1Mk2 blink1;

        [RequireBlink1Mk2Hardware]
        public void SetAllPatterns()
        {
            blink1.Save(new Blink1Preset(Color.Cyan, TimeSpan.FromSeconds(5)), 0);
            blink1.Save(new Blink1Preset(Color.DarkCyan, TimeSpan.FromSeconds(5)), 1);
            blink1.Save(new Blink1Preset(Color.CadetBlue, TimeSpan.FromSeconds(5)), 2);
            blink1.Save(new Blink1Preset(Color.SteelBlue, TimeSpan.FromSeconds(5)), 3);
            blink1.Save(new Blink1Preset(Color.DodgerBlue, TimeSpan.FromSeconds(5)), 4);
            blink1.Save(new Blink1Preset(Color.MediumBlue, TimeSpan.FromSeconds(5)), 5);
            blink1.Save(new Blink1Preset(Color.DarkBlue, TimeSpan.FromSeconds(5)), 6);
            blink1.Save(new Blink1Preset(Color.Green, TimeSpan.FromSeconds(5)), 7);
            blink1.Save(new Blink1Preset(Color.SeaGreen, TimeSpan.FromSeconds(5)), 8);
            blink1.Save(new Blink1Preset(Color.MediumSeaGreen, TimeSpan.FromSeconds(5)), 9);
            blink1.Save(new Blink1Preset(Color.SpringGreen, TimeSpan.FromSeconds(5)), 10);
            blink1.Save(new Blink1Preset(Color.LightGreen, TimeSpan.FromSeconds(5)), 11);
        }

        [RequireBlink1Mk2Hardware]
        public void ReadSerialReadsValidSerialNumber()
        {
            var actual = blink1.SerialNumber;

            Assert.InRange(actual, LowestSerialNumber, HighestSerialNumber);
        }

        [RequireBlink1Mk2Hardware]
        public void ReadPresetReturnsValidPreset()
        {
            var actual = blink1.ReadPreset(0);

            Assert.NotNull(actual);
        }

        [RequireBlink1Mk2Hardware]
        public void SavePresetWritesToDevice()
        {
            var expected = new Blink1Preset(Color.DarkSlateGray, TimeSpan.FromSeconds(1.5));

            blink1.EnableGamma = false;

            blink1.Save(expected, 0);

            var actual = blink1.ReadPreset(0);

            Assert.Equal(expected, actual);
        }

        [RequireBlink1Mk2Hardware]
        public void SetColor()
        {
            var actual = blink1.Set(Color.Blue);

            Assert.True(actual);
        }

        [RequireBlink1Mk2Hardware]
        public void ShowColor()
        {
            var showColorTime = TimeSpan.FromSeconds(2);

            var actual = blink1.Show(Color.Chartreuse, showColorTime);

            Thread.Sleep(showColorTime);

            Assert.True(actual);
        }

        [RequireBlink1Mk2Hardware]
        public void FadeToColor()
        {
            var fadeDuration = TimeSpan.FromSeconds(2);

            var actual = blink1.Fade(Color.Red, fadeDuration);

            Thread.Sleep(fadeDuration);

            Assert.True(actual);
        }

        [RequireBlink1Mk2Hardware]
        public void SetPreset0AndPlayIt()
        {
            var presetDuration = TimeSpan.FromSeconds(2);

            var preset = new Blink1Preset(Color.DarkGoldenrod, presetDuration);

            blink1.Save(preset, 0);

            blink1.Play(0);

            Thread.Sleep(presetDuration);

            blink1.Pause();
        }

        [RequireBlink1Mk2Hardware]
        public void PlayPreset()
        {
            blink1.Play(0, 11, 0);
        }

        [RequireBlink1Mk2Hardware]
        public void PoliceBlinking()
        {
            for (var i = 0; i < 100; i++)
            {
                blink1.Fade(Color.Blue, TimeSpan.FromMilliseconds(25), (LEDPosition) (i%2));
                blink1.Fade(Color.Red, TimeSpan.FromMilliseconds(25), (LEDPosition) (i%2 + 1));

                Thread.Sleep(200);
            }

            blink1.Set(Color.Black);
        }

        [RequireBlink1Mk2Hardware]
        public void TurnOff()
        {
            blink1.Set(Color.Black);
        }

        [RequireBlink1Mk2Hardware]
        public void EnableInactivityMode()
        {
            blink1.EnableInactivityMode(TimeSpan.FromMilliseconds(50));

            Thread.Sleep(TimeSpan.FromMilliseconds(150));

            blink1.DisableInactivityMode();
        }

        public void SetFixture(Blink1Mk2Fixture data)
        {
            blink1 = data.Device;
        }
    }
}