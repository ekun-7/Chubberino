﻿using Chubberino.Client;
using Chubberino.Client.Services;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Utility;
using System;
using System.Collections.Generic;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public sealed class QuestManager : AbstractCommandStrategy, IQuestManager
    {
        private IList<IQuest> Quests { get; }
        public static TimeSpan QuestCooldown { get; set; } = TimeSpan.FromHours(2);
        public IDateTimeService DateTime { get; }

        public QuestManager(
            IApplicationContext context,
            ITwitchClientManager client,
            Random random,
            IEmoteManager emoteManager,
            IDateTimeService dateTime)
            : base(context, client, random, emoteManager)
        {
            Quests = new List<IQuest>();
            DateTime = dateTime;
        }

        public IQuestManager AddQuest(IQuest quest)
        {
            Quests.Add(quest);

            return this;
        }

        public void StartQuest(ChatMessage message)
        {
            var player = GetPlayer(message);
            var now = DateTime.Now;

            var timeSinceLastQuestVentured = now - player.LastQuestVentured;

            if (timeSinceLastQuestVentured >= QuestCooldown)
            {
                var quest = Random.GetElement(Quests);

                quest.Start(message, player);

                player.LastQuestVentured = now;

                Context.SaveChanges();
            }
            else
            {
                var timeUntilNextQuestAvailable = QuestCooldown - timeSinceLastQuestVentured;

                var timeToWait = timeUntilNextQuestAvailable.Format();

                TwitchClientManager.SpoolMessageAsMe(message.Channel, player, $"You must wait {timeToWait} until you can go on your next quest.");
            }
        }
    }
}