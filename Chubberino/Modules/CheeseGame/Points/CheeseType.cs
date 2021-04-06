﻿using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Points
{
    public sealed class CheeseType
    {
        public CheeseType(String name, Int32 pointValue)
        {
            Name = name;
            Points = pointValue;
            CostToUnlock = 0;
            RankToUnlock = Rank.Bronze;
            UnlocksNegativeCheese = false;
        }
        public CheeseType(
            String name,
            Int32 pointValue,
            Rank rankToUnlock,
            Int32 costToUnlock,
            Boolean unlocksNegativeCheese = false)
        {
            Name = name;
            Points = pointValue;
            RankToUnlock = rankToUnlock;
            CostToUnlock = costToUnlock;
            UnlocksNegativeCheese = unlocksNegativeCheese;
        }

        public String Name { get; }

        public Int32 Points { get; }

        public Int32 CostToUnlock { get; }

        public Rank RankToUnlock { get; }

        public Boolean UnlocksNegativeCheese { get; }
    }
}
