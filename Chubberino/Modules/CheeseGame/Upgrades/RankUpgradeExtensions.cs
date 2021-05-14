﻿using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Rankings;
using Chubberino.Utility;
using System;

namespace Chubberino.Modules.CheeseGame.Upgrades
{
    public static class RankUpgradeExtensions
    {
        private const String ModifierDescription = "Chance to make {0} cheese (+{1} to base cheese value)";

        private const String CriticalCheeseDescription = "{0}% -> {1}% critical cheese chance";

        private const String ProductionDescription = "+{0}% -> +{1}% cheese per worker";

        private const String StorageDescription = "+{0}% -> +{1}% storage increase";

        private const String QuestDescription = "x{0} -> x{1} rare quest chance";

        public static Upgrade GetCheeseModifierUpgrade(this Rank rank)
        {
            if (CheeseModifierRepository.Modifiers.TryGet((Int32)(rank + 1), out var cheeseModifier))
            {
                return new Upgrade(
                    String.Format(ModifierDescription, cheeseModifier.Name, cheeseModifier.Points),
                    rank,
                    0.25,
                    x => x.NextCheeseModifierUpgradeUnlock++);
            }
            return null;
        }

        public static Upgrade GetCriticalCheeseUpgrade(this Rank rank)
        {
            Double currentUpgradePercent = (Int32)(rank) * Constants.CriticalCheeseUpgradePercent * 100;
            Double nextUpgradePercent = (Int32)(rank + 1) * Constants.CriticalCheeseUpgradePercent * 100;
            return new Upgrade(
                String.Format(CriticalCheeseDescription, String.Format("{0:0.0}", currentUpgradePercent), String.Format("{0:0.0}", nextUpgradePercent)),
                rank,
                0.40,
                x => x.NextCriticalCheeseUpgradeUnlock++);
        }

        public static Upgrade GetQuestUpgrade(this Rank rank)
        {
            String currentUpgradePercent = String.Format("{0:0.0}", rank.GetRareQuestChanceMultiplier());
            String nextUpgradePercent = String.Format("{0:0.0}", rank.Next().GetRareQuestChanceMultiplier());
            return new Upgrade(
                String.Format(QuestDescription, currentUpgradePercent, nextUpgradePercent),
                rank,
                0.55,
                x => x.NextQuestUpgradeUnlock++);
        }

        public static Upgrade GetWorkerProductionUpgrade(this Rank rank)
        {
            Int32 currentUpgradePercent = (Int32)(rank.GetWorkerPointMultiplier() * 100);
            Int32 nextUpgradePercent = (Int32)(rank.Next().GetWorkerPointMultiplier() * 100);
            return new Upgrade(
                String.Format(ProductionDescription, currentUpgradePercent, nextUpgradePercent),
                rank,
                0.70,
                x => x.NextWorkerProductionUpgradeUnlock++);
        }

        public static Upgrade GetStorageUpgrade(this Rank rank)
        {
            Double currentUpgradePercent = (Int32)(rank) * Constants.StorageUpgradePercent * 100;
            Double nextUpgradePercent = (Int32)(rank + 1) * Constants.StorageUpgradePercent * 100;
            return new Upgrade(
                String.Format(StorageDescription, currentUpgradePercent, nextUpgradePercent),
                rank,
                0.85,
                x => x.NextStorageUpgradeUnlock++);
        }

    }
}
