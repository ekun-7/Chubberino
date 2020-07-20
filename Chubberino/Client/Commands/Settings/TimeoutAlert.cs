﻿using Chubberino.Client.Abstractions;
using System;
using TwitchLib.Client.Interfaces;

namespace Chubberino.Client.Commands.Settings
{
    internal class TimeoutAlert : Setting
    {
        public TimeoutAlert(ITwitchClient client, IMessageSpooler spooler)
            : base(client, spooler)
        {
            TwitchClient.OnUserTimedout += TwitchClient_OnUserTimedout;
        }

        private void TwitchClient_OnUserTimedout(Object sender, TwitchLib.Client.Events.OnUserTimedoutArgs e)
        {
            if (!IsEnabled) { return; }
            TwitchClient.SendMessage(e.UserTimeout.Channel, $"WideHardo FREE MY MAN {e.UserTimeout.Username.ToUpper()}");
        }
    }
}
