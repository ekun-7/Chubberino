﻿using MouseBot.Implementation.Abstractions;
using MouseBot.Implementation.Commands;
using MouseBot.Implementation.Extensions;
using System;
using System.Collections.Concurrent;
using TwitchLib.Client.Exceptions;
using TwitchLib.Client.Interfaces;

namespace MouseBot.Implementation
{
    /// <summary>
    /// Queues messages, and sends them over timed intervals.
    /// </summary>
    public sealed class MessageSpooler : Manager, IMessageSpooler
    {
        /// <summary>
        /// Empty character to evade identical message detection.
        /// </summary>
        private const String EmptyCharacter = "⠀";

        public String ChannelName { get; private set; } = String.Empty;

        private ConcurrentQueue<String> MessageQueue { get; set; }
            = new ConcurrentQueue<String>();

        private MessageTimer Timer { get; }

        private String repeatMessage;

        public String RepeatMessage
        {
            get => repeatMessage;
            set
            {
                MessageQueue.Clear();
                repeatMessage = value;
                Console.WriteLine(repeatMessage is null
                    ? "Repeat disabled."
                    : $"Repeating \"{repeatMessage}\"");
            }
        }

        public TimeSpan Interval
        {
            get => Timer.Interval;
            set => Timer.Interval = value;
        }

        public Int32 QueueSize => MessageQueue.Count;

        public MessageSpooler(ITwitchClient client)
            : base(client)
        {

        }

        public void SetChannel(String channelName)
        {
            MessageQueue.Clear();
            ChannelName = channelName;
        }

        private void SendMessage(String message)
        {
            // TwitchClient.EnsureJoinedToChannel(ChannelName);
            if (message == TwitchClient.JoinedChannels?[0].PreviousMessage?.Message)
            {
                message += " " + EmptyCharacter;
            }

            try
            {
                TwitchClient.SendMessage(ChannelName, message);
            }
            catch (BadStateException e)
            {
                Console.WriteLine("ERROR: Failed to send message");
                Console.WriteLine(e.Message);
            }
        }

        public void SpoolMessage(String message, Priority priority)
        {
            SendMessage(message);
            //switch (priority)
            //{
                //case Priority.Low:
                //    break;
                //case Priority.High:
                //    if (MessageQueue.Count < MaximumQueueSize)
                //    {
                //        MessageQueue.Enqueue(message);
                //    }
                //    break;
            //}
        }

        protected override void ManageTasks()
        {
            //if (MessageQueue.Count == 0)
            //{
            //    if (repeatMessage != null)
            //    {
            //        SpoolMessage(repeatMessage, Priority.High);
            //    }
            //}
            //if (MessageQueue.TryDequeue(out String message))
            //{
            //    SendMessage(message);
            //}
        }
    }
}
