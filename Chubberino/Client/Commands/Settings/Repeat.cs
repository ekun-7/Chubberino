﻿using Chubberino.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings
{
    /// <summary>
    /// Repeat a specified message at the throttle limit.
    /// It is not recommended to sent messages manually while messages are
    /// being repeated from this, or you may incur a global IP shadow ban.
    /// </summary>
    public sealed class Repeat : Setting
    {
        private String RepeatMessage { get; set; }

        /// <summary>
        /// Indicates that we are waiting for the repeat message to be typed in chat.
        /// </summary>
        private Boolean WaitingForRepeatMessage { get; set; }

        private IRepeater Repeater { get; }

        public Repeat(IExtendedClient client, IRepeater repeater)
            : base(client)
        {
            Repeater = repeater;
            Repeater.Action = SpoolRepeatMessages;
            Repeater.Interval = TimeSpan.FromSeconds(0.3);
        }

        private void SpoolRepeatMessages()
        {
            TwitchClient.SpoolMessage(RepeatMessage);
        }

        public override String Status => base.Status
            + $"\n\tMessage: {RepeatMessage}"
            + $"\n\tInterval: {Repeater.Interval.TotalSeconds} seconds"
            + $"\n\tVariance: {Repeater.Variance.TotalSeconds} seconds"
            + $"\n\tWait for repeat message: {WaitingForRepeatMessage}";

        public override void Execute(IEnumerable<String> arguments)
        {
            String proposedRepeatMessage = String.Join(" ", arguments);

            if (String.IsNullOrEmpty(proposedRepeatMessage))
            {
                // No arguments toggles.
                IsEnabled = !IsEnabled;
            }
            else
            {
                // Update the message and keep repeating.
                RepeatMessage = proposedRepeatMessage;
                IsEnabled = true;
            }

            Repeater.IsRunning = IsEnabled;
        }

        public override Boolean Set(String property, IEnumerable<String> arguments)
        {
            switch (property?.ToLower())
            {
                case "m":
                case "message":
                    RepeatMessage = String.Join(" ", arguments);
                    return true;
                case "i":
                case "interval":
                {
                    if (Double.TryParse(arguments.FirstOrDefault(), out Double result))
                    {
                        Repeater.Interval = result >= 0
                            ? TimeSpan.FromSeconds(result)
                            : TimeSpan.Zero;

                        return true;
                    }
                }
                break;
                case "v":
                case "variance":
                {
                    if (Double.TryParse(arguments.FirstOrDefault(), out Double result))
                    {
                        Repeater.Variance = result >= 0
                            ? TimeSpan.FromSeconds(result)
                            : TimeSpan.Zero;

                        return true;
                    }
                }
                break;
                case "w":
                case "wait":
                    if (!WaitingForRepeatMessage)
                    {
                        TwitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
                        WaitingForRepeatMessage = true;
                    }
                    return true;
            }
            return false;
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!e.ChatMessage.Username.Equals(e.ChatMessage.BotUsername)) { return; }

            RepeatMessage = e.ChatMessage.Message;

            WaitingForRepeatMessage = false;
            TwitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;
            Console.WriteLine($"Received repeat message: \"{RepeatMessage}\"");
        }

        public override String GetHelp()
        {
            return @"
interval - the time between each message being sent

variance - the random range of time to add or subtract from each interval

wait - Indicates that we are waiting for the repeat message to be typed in chat.
       When true, this waits for the next message to be sent by the bot in
       chat and saves that as the repeat message.
       This is useful for messages that contain emojis or characters that
       otherwise cannot be probably encoded by typing them in the command line.
";
        }
    }
}
