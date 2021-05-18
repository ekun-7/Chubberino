﻿using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public sealed class GainCheeseQuest : Quest
    {
        public GainCheeseQuest(
            String location,
            String failureMessage,
            String successMessage,
            Int32 rewardPoints,
            Rank rankToUnlock,
            Double rankPricePercentPrice)
            : base(location,
                  failureMessage,
                  (player, emote) =>
                  {
                      Int32 finalPoints = player.GetModifiedPoints(rewardPoints);
                      player.AddPoints(finalPoints);
                      return $"{successMessage} {emote} (+{finalPoints} cheese)";
                  },
                  player => $"+{player.GetModifiedPoints(rewardPoints)} cheese",
                  rankToUnlock,
                  rankPricePercentPrice)
        {
        }
    }
}
