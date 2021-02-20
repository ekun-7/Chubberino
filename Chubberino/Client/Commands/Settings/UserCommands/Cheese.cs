﻿using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Quests;
using Chubberino.Modules.CheeseGame.Rankings;
using Chubberino.Modules.CheeseGame.Shops;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Events;

namespace Chubberino.Client.Commands.Settings.UserCommands
{
    public sealed class Cheese : UserCommand
    {
        public Cheese(
            ITwitchClientManager client,
            IConsole console,
            IPointManager pointManager,
            IShop shop,
            IRankManager rankManager,
            IQuestManager questManager)
            : base(client, console)
        {
            PointManager = pointManager;
            Shop = shop;
            RankManager = rankManager;
            QuestManager = questManager;
            Enable = twitchClient => twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
            Disable = twitchClient => twitchClient.OnMessageReceived -= TwitchClient_OnMessageReceived;
        }

        private void TwitchClient_OnMessageReceived(Object sender, OnMessageReceivedArgs e)
        {
            if (!TryValidateCommand(e, out IEnumerable<String> words)) { return; }

            if (!words.Any())
            {
                PointManager.AddPoints(e.ChatMessage);
                return;
            }

            var cheeseCommand = words.First().ToLower();
            switch (cheeseCommand)
            {
                case "s":
                case "shop":
                    Shop.ListInventory(e.ChatMessage);
                    break;
                case "b":
                case "buy":
                    Shop.BuyItem(e.ChatMessage);
                    break;
                case "h":
                case "help":
                    Shop.HelpItem(e.ChatMessage);
                    break;
                case "r":
                case "rank":
                    RankManager.ShowRank(e.ChatMessage);
                    break;
                case "rankup":
                    RankManager.RankUp(e.ChatMessage);
                    break;
                case "q":
                case "quest":
                case "quests":
                    QuestManager.StartQuest(e.ChatMessage);
                    break;
                default:
                    PointManager.AddPoints(e.ChatMessage);
                    break;
            }
        }

        public IPointManager PointManager { get; }
        public IShop Shop { get; }
        public IRankManager RankManager { get; }
        public IQuestManager QuestManager { get; }
    }
}
