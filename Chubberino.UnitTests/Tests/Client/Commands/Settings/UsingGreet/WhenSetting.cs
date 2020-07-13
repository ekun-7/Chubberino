﻿using Chubberino.Client.Commands.Settings;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.UsingGreet
{
    public sealed class WhenSetting : GreetTestBase
    {
        [Theory]
        [InlineData("mode", new String[] { "default" }, Greet.Mode.Default)]
        [InlineData("mOdE", new String[] { "DeFaUlt" }, Greet.Mode.Default)]
        [InlineData("mode", new String[] { "" }, Greet.Mode.Default)]
        [InlineData("mode", new String[] { }, Greet.Mode.Default)]
        [InlineData("mode", new String[] { "wholesome" }, Greet.Mode.Wholesome)]
        [InlineData("MoDe", new String[] { "WhOlEsOmE" }, Greet.Mode.Wholesome)]
        [InlineData("mode", new String[] { "w" }, Greet.Mode.Wholesome)]
        [InlineData("mOdE", new String[] { "W" }, Greet.Mode.Wholesome)]
        public void ShouldSetMode(String value, String[] arguments, Greet.Mode expectedMode)
        {
            Sut.Set(value, arguments);

            Assert.Equal(expectedMode, Sut.CurrentMode);
        }
    }
}