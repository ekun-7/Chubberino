﻿using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Settings;
using Chubberino.Client.Commands.Settings.Replies;
using Chubberino.Client.Commands.Strategies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TwitchLib.Api.Core.Extensions.System;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands
{
    internal sealed class CommandRepository
    {
        private IReadOnlyList<ICommand> Commands { get; }

        public CommandRepository(ITwitchClient client, IMessageSpooler spooler)
        {
            var stopSettingStrategy = new StopSettingStrategy();

            var commands = new List<ICommand>()
            {
                new AutoChat(client, spooler, stopSettingStrategy),
                new AutoPogO(client, spooler),
                new Color(client, spooler),
                new Copy(client, spooler),
                new Count(client, spooler, new Repeater()),
                new Greet(client, spooler),
                new Jimbox(client, spooler),
                new Join(client, spooler),
                new Log(client, spooler),
                new MockStreamElements(client, spooler),
                new Repeat(client, spooler, new Repeater(), stopSettingStrategy),
                new Reply(client, spooler, new EqualsComparator(), new ContainsComparator()),
                new TimeoutAlert(client, spooler),
                new TrackJimbox(client, spooler),
                new TrackPyramids(client, spooler),
                new YepKyle(client, spooler),
            };

            var disableAll = new DisableAll(client, spooler, commands);

            commands.Add(disableAll);

            Commands = commands;
        }

        public void RefreshAll(ITwitchClient twitchClient, IMessageSpooler messageSpooler)
        {
            foreach (ICommand command in Commands)
            {
                command.Refresh(twitchClient, messageSpooler);
            }
        }

        public String GetStatus()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (ICommand command in Commands)
            {
                if (command is ISetting setting)
                {
                    stringBuilder.Append(setting.Name + ": " + setting.Status + Environment.NewLine);
                }
            }

            return stringBuilder.ToString();
        }

        public void Execute(String commandName, IEnumerable<String> arguments)
        {
            if (String.IsNullOrWhiteSpace(commandName)) { return; }

            switch (commandName)
            {
                // Meta commands
                case "g":
                case "get":
                    Get(arguments.FirstOrDefault(), arguments.Skip(1));
                    break;
                case "s":
                case "set":
                    Set(arguments.FirstOrDefault(), arguments.Skip(1));
                    break;
                case "a":
                case "add":
                    Add(arguments.FirstOrDefault(), arguments.Skip(1));
                    break;
                case "r":
                case "remove":
                    Remove(arguments.FirstOrDefault(), arguments.Skip(1));
                    break;
                case "h":
                case "help":
                    Help(arguments.FirstOrDefault());
                    break;


                // Regular commands
                default:
                    ICommand commandToExecute = GetCommand(commandName);

                    if (commandToExecute == null)
                    {
                        Console.WriteLine($"Command \"{commandName}\" not found.");
                    }
                    else
                    {
                        commandToExecute.Execute(arguments);
                    }
                    break;
            }

        }

        private void Get(String commandName, IEnumerable<String> arguments)
        {
            ICommand commandToSet = GetCommand(commandName);

            if (commandToSet == null)
            {
                Console.WriteLine($"Command \"{commandName}\" not found to get.");
            }
            else
            {
                String value = commandToSet.Get(arguments);
                if (String.IsNullOrWhiteSpace(value))
                {
                    Console.WriteLine($"Command \"{commandName}\" value \"{String.Join(" ", arguments)}\" not found.");
                }
                else
                {
                    Console.WriteLine($"Command \"{commandName}\" value \"{String.Join(" ", arguments)}\" is \"{value}\".");
                }
            }
        }

        private void Set(String commandName, IEnumerable<String> arguments)
        {
            ICommand commandToSet = GetCommand(commandName);
            String property = arguments.FirstOrDefault();

            arguments = arguments.Skip(1);

            if (commandToSet == null)
            {
                Console.WriteLine($"Command \"{commandName}\" not found to set.");
            }
            else if (commandToSet.Set(property, arguments))
            {
                Console.WriteLine($"Command \"{commandName}\" property \"{property}\" set to \"{String.Join(" ", arguments)}\".");
            }
            else
            {
                Console.WriteLine($"Command \"{commandName}\" property \"{property}\" not set.");
            }
        }

        private void Add(String commandName, IEnumerable<String> arguments)
        {
            ICommand commandToAddTo = GetCommand(commandName);
            String property = arguments.FirstOrDefault();

            arguments = arguments.Skip(1);

            if (commandToAddTo == null)
            {
                Console.WriteLine($"Command \"{commandName}\" not found to add to.");
            }
            else if (commandToAddTo.Add(property, arguments))
            {
                Console.WriteLine($"Command \"{commandName}\" property \"{property}\" added \"{String.Join(" ", arguments)}\".");
            }
            else
            {
                Console.WriteLine($"Command \"{commandName}\" property \"{property}\" not added to.");
            }
        }

        private void Remove(String commandName, IEnumerable<String> arguments)
        {
            ICommand commandToRemoveFrom = GetCommand(commandName);
            String property = arguments.FirstOrDefault();

            arguments = arguments.Skip(1);

            if (commandToRemoveFrom == null)
            {
                Console.WriteLine($"Command \"{commandName}\" not found to add remove from.");
            }
            else if (commandToRemoveFrom.Remove(property, arguments))
            {
                Console.WriteLine($"Command \"{commandName}\" property \"{property}\" removed \"{String.Join(" ", arguments)}\".");
            }
            else
            {
                Console.WriteLine($"Command \"{commandName}\" property \"{property}\" not removed from.");
            }
        }

        private void Help(String commandName)
        {

            ICommand commandToSet = GetCommand(commandName);

            String message = commandToSet?.GetHelp();

            if (message != null)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine($"Command \"{commandName}\" not found.");
            }
        }

        private ICommand GetCommand(String commandName) => Commands
                .Where(command => command.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
    }
}
