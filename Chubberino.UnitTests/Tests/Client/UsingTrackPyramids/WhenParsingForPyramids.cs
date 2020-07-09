﻿using Chubberino.Client;
using Chubberino.Client.Abstractions;
using Chubberino.Client.Commands.Settings;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Api.Core.Models.Undocumented.ChannelPanels;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.UsingTrackPyramids
{
    public sealed class WhenParsingForPyramids
    {
        private TrackPyramids Sut { get; }

        private Mock<ITwitchClient> TwitchClient { get; }

        private Mock<IMessageSpooler> MessageSpooler { get; }

        public WhenParsingForPyramids()
        {
            TwitchClient = new Mock<ITwitchClient>();

            MessageSpooler = new Mock<IMessageSpooler>();

            Sut = new TrackPyramids(TwitchClient.Object, MessageSpooler.Object);
        }

        [Theory]
        [MemberData(nameof(ValidPyramids))]
        public void ShouldDetectPyramid(
            (String Username, String Message)[] messages,
            String[] expectedContributors,
            String expectedPyramidBlock,
            Int32 expectedPyramidHeight)
        {
            MessageSpooler.Setup(x => x.SpoolMessage(It.IsAny<String>(), It.IsAny<Priority>()))
                .Callback((String message, Priority priority) =>
                {
                    Assert.Contains(expectedPyramidHeight.ToString(), message);
                    Assert.Contains(expectedPyramidBlock.ToString(), message);

                    foreach (String contributor in expectedContributors)
                    {
                        Assert.Contains($"@{contributor}", message);
                    }
                });

            foreach (var (Username, Message) in messages)
            {
                var chatMessage = new ChatMessage(
                    botUsername: default,
                    userId: default,
                    userName: Username,
                    displayName: default,
                    colorHex: default,
                    color: default,
                    emoteSet: default,
                    message: Message,
                    userType: default,
                    channel: default,
                    id: default,
                    isSubscriber: default,
                    subscribedMonthCount: default,
                    roomId: default,
                    isTurbo: default,
                    isModerator: default,
                    isMe: default,
                    isBroadcaster: default,
                    noisy: default,
                    rawIrcMessage: default,
                    emoteReplacedMessage: default,
                    badges: default,
                    cheerBadge: default,
                    bits: default,
                    bitsInDollars: default);

                Sut.TwitchClient_OnMessageReceived(null, new OnMessageReceivedArgs()
                {
                    ChatMessage = chatMessage
                });
            }

            Assert.Equal(expectedPyramidBlock, Sut.PyramidBlock);

            MessageSpooler.Verify(x => x.SpoolMessage(It.IsAny<String>(), It.IsAny<Priority>()), Times.Once());
        }

        public static IEnumerable<Object[]> ValidPyramids { get; } = new List<Object[]>
        {
            // 2 height
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("a", "x"),
                    ("a", "x x"),
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                2
            },
            // 3 height
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("a", "x"),
                    ("a", "x x"),
                    ("a", "x x x"),
                    ("a", "x x"),
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                3
            },
            // 2 authors
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("a", "x"),
                    ("b", "x x"),
                    ("a", "x"),
                },
                new String[] { "a", "b", },
                "x",
                2
            },
            // 2 authors
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("a", "x"),
                    ("a", "x x"),
                    ("b", "x"),
                },
                new String[] { "a", "b", },
                "x",
                2
            },
            // 2 authors
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("a", "x"),
                    ("b", "x x"),
                    ("b", "x"),
                },
                new String[] { "a", "b", },
                "x",
                2
            },
            // 3 authors
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("a", "x"),
                    ("b", "x x"),
                    ("c", "x"),
                },
                new String[] { "a", "b", "c", },
                "x",
                2
            },
        };

        [Theory]
        [MemberData(nameof(InvalidPyramids))]
        public void ShouldNotDetectPyramid(
            (String Username, String Message)[] messages,
            String[] expectedContributors,
            String expectedPyramidBlock,
            Int32 expectedPyramidHeight)
        {
            foreach (var (Username, Message) in messages)
            {
                var chatMessage = new ChatMessage(
                    botUsername: default,
                    userId: default,
                    userName: Username,
                    displayName: default,
                    colorHex: default,
                    color: default,
                    emoteSet: default,
                    message: Message,
                    userType: default,
                    channel: default,
                    id: default,
                    isSubscriber: default,
                    subscribedMonthCount: default,
                    roomId: default,
                    isTurbo: default,
                    isModerator: default,
                    isMe: default,
                    isBroadcaster: default,
                    noisy: default,
                    rawIrcMessage: default,
                    emoteReplacedMessage: default,
                    badges: default,
                    cheerBadge: default,
                    bits: default,
                    bitsInDollars: default);

                Sut.TwitchClient_OnMessageReceived(null, new OnMessageReceivedArgs()
                {
                    ChatMessage = chatMessage
                });
            }

            Assert.Equal(expectedPyramidBlock, Sut.PyramidBlock);

            MessageSpooler.Verify(x => x.SpoolMessage(It.IsAny<String>(), It.IsAny<Priority>()), Times.Never());
        }

        public static IEnumerable<Object[]> InvalidPyramids { get; } = new List<Object[]>
        {
            // 2 height
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                1
            },
            // 3 height
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("a", "x"),
                    ("a", "x x"),
                    ("a", "x x x"),
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                1
            },
            // 3 height
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("a", "x"),
                    ("a", "x x x"),
                    ("a", "x x"),
                    ("a", "x"),
                },
                new String[] { "a" },
                "x",
                1
            },
            // 2 authors
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("b", "x x"),
                    ("a", "x"),
                },
                new String[] { "a", },
                "x",
                1
            },
            // 2 authors
            new Object[]
            {
                new (String Username, String Message)[]
                {
                    ("a", "x"),
                    ("b", "x"),
                },
                new String[] { "b", },
                "x",
                1
            },
        };
    }
}