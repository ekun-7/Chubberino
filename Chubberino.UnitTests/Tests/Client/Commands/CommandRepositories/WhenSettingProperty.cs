﻿using Chubberino.Client.Commands;
using Chubberino.Client.Commands.Settings;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.CommandRepositories
{
    /// <summary>
    /// When setting an invalid property on a valid command
    /// </summary>
    public sealed class WhenSettingProperty : UsingCommandRepository
    {
        /// <summary>
        /// Should output a message indicating the property failed to be set.
        /// A property is not set if the value being set is invalid, or if the
        /// property cannot be found.
        /// </summary>
        /// <param name="arguments"></param>
        [Theory]
        // Invalid property
        [InlineData("a", "1")]
        // Invalid property and value
        [InlineData("a", "b")]
        // Invalid value
        [InlineData("s", "a")]
        public void ShouldOutputPropertyNotSetMessage(params String[] arguments)
        {
            String validCommandName = new AutoChat(MockedClient.Object, MockedConsole.Object).Name;
            String propertyName = arguments[0];
            List<String> commandWithArguments = new List<String>() { validCommandName };
            commandWithArguments.AddRange(arguments);

            Sut.Execute("set", commandWithArguments);

            MockedConsole.Verify(x => x.WriteLine($"Command \"{validCommandName}\" property \"{propertyName}\" not set."), Times.Once());
        }

        /// <summary>
        /// Should output a message indicating the command was not found.
        /// </summary>
        /// <param name="arguments"></param>
        [Theory]
        [InlineData("a", "1")]
        [InlineData("a", "b")]
        [InlineData("s", "a")]
        public void ShouldOutputCommandNotFoundMessage(params String[] arguments)
        {
            String invalidCommandName = Guid.NewGuid().ToString();
            String propertyName = arguments[0];
            List<String> commandWithArguments = new List<String>() { invalidCommandName };
            commandWithArguments.AddRange(arguments);

            Sut.Execute("set", commandWithArguments);

            MockedConsole.Verify(x => x.WriteLine($"Command \"{invalidCommandName}\" not found to set."), Times.Once());
        }

        [Theory]
        [InlineData("message", "a")]
        [InlineData("message", "a b")]
        public void ShouldOutputPropertySetMessage(params String[] arguments)
        {
            String validCommandName = new Repeat(MockedClient.Object, new Repeater(), MockedConsole.Object).Name;
            String propertyName = arguments[0];
            IEnumerable<String> propertyValue = arguments.Skip(1);
            List<String> commandWithArguments = new List<String>() { validCommandName };
            commandWithArguments.AddRange(arguments);

            Sut.Execute("set", commandWithArguments);

            MockedConsole.Verify(x => x.WriteLine($"Command \"{validCommandName}\" property \"{propertyName}\" set to \"{String.Join(" ", propertyValue)}\"."), Times.Once());
        }
    }
}