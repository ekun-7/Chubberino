﻿using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Rankings;
using Chubberino.Modules.CheeseGame.Upgrades;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Upgrades
{
    public sealed class WhenGettingLastUpgradeUnlocked
    {
        public Player Player { get; }

        public WhenGettingLastUpgradeUnlocked()
        {
            Player = new();
        }

        [Fact]
        public void ShouldReturnNoneForNewPlayer()
        {
            UpgradeType result = Player.GetPreviousUpgradeUnlocked();

            Assert.Equal(UpgradeType.None, result);
        }

        [Fact]
        public void ShouldReturnStorageForAllBronzeUpgradesBought()
        {
            Player.NextCheeseModifierUpgradeUnlock = Rank.Silver;
            Player.NextCriticalCheeseUpgradeUnlock = Rank.Silver;
            Player.NextQuestRewardUpgradeUnlock = Rank.Silver;
            Player.NextWorkerProductionUpgradeUnlock = Rank.Silver;
            Player.NextStorageUpgradeUnlock = Rank.Silver;

            UpgradeType result = Player.GetPreviousUpgradeUnlocked();

            Assert.Equal(UpgradeType.Storage, result);
        }

        [Fact]
        public void ShouldReturnQuest()
        {
            Player.NextCheeseModifierUpgradeUnlock = Rank.Silver;
            Player.NextCriticalCheeseUpgradeUnlock = Rank.Silver;
            Player.NextQuestRewardUpgradeUnlock = Rank.Silver;
            Player.NextWorkerProductionUpgradeUnlock = Rank.Bronze;
            Player.NextStorageUpgradeUnlock = Rank.Bronze;

            UpgradeType result = Player.GetPreviousUpgradeUnlocked();

            Assert.Equal(UpgradeType.Quest, result);
        }

        [Fact]
        public void ShouldReturnStorage()
        {
            Player.NextCheeseModifierUpgradeUnlock = Rank.None;
            Player.NextCriticalCheeseUpgradeUnlock = Rank.None;
            Player.NextQuestRewardUpgradeUnlock = Rank.None;
            Player.NextWorkerProductionUpgradeUnlock = Rank.None;
            Player.NextStorageUpgradeUnlock = Rank.None;

            UpgradeType result = Player.GetPreviousUpgradeUnlocked();

            Assert.Equal(UpgradeType.Storage, result);
        }
    }
}