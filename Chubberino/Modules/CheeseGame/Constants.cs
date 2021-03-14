﻿using System;

namespace Chubberino.Modules.CheeseGame
{
    public static class Constants
    {
        /// <summary>
        /// Bonus multiplier per every prestige level.
        /// </summary>
        public const Double PrestigeBonus = 0.1;

        public const Double StorageUpgradePercent = 0.5;

        public const Double WorkerUpgradePercent = 0.1;

        public const Double QuestBaseSuccessChance = 0.2;

        public const Double QuestSuccessUpgradePercent = 0.05;

        public const Double QuestWorkerSuccessPercent = 0.01;

        public const Double CriticalCheeseUpgradePercent = 0.005;

        public const Int32 CriticalCheeseMultiplier = 5;

        /// <summary>
        /// Base storage quantity gained when buying from the shop.
        /// </summary>
        public const Int32 ShopStorageQuantity = 100;
    }
}