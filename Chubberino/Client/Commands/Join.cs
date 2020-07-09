﻿using Chubberino.Client.Abstractions;
using Chubberino.Client.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands
{
    public sealed class Join : Command
    {
        private String JoinedChannelName { get; set; }

        public Join(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnJoinedChannel += TwitchClient_OnJoinedChannel;
        }

        private void TwitchClient_OnJoinedChannel(Object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
        {
            // For simplicity, we can only be in 1 channel at a time.
            if (e.Channel.Equals(JoinedChannelName, StringComparison.OrdinalIgnoreCase)) { return; }

            if (JoinedChannelName != null)
            {
                TwitchClient.LeaveChannel(JoinedChannelName);
                Console.WriteLine($"Left channel {JoinedChannelName}");
            }

            JoinedChannelName = e.Channel;
            Spooler.SetChannel(JoinedChannelName);
            Console.WriteLine($"Joined channel {JoinedChannelName}");
        }

        public override void Execute(IEnumerable<String> arguments)
        {
            if (arguments.Count() == 0) { return; }


            if (!TwitchClient.IsConnected)
            {
                TwitchClient.Connect();
            }
            TwitchClient.JoinChannel(arguments.FirstOrDefault());

            TwitchClient.EnsureJoinedToChannel(arguments.FirstOrDefault());
        }
    }
}