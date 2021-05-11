﻿using Chubberino.Modules.CheeseGame.Models;
using Monad;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Items
{
    public sealed class Worker : Item
    {
        public const String NotEnoughPopulationMessage = "You do not have enough population slots for another worker. Consider buying more population with \"!cheese buy population\".";

        public override IEnumerable<String> Names { get; } = new String[] { "Worker", "w", "workers" };

        public override Int32 GetPrice(Player player)
        {
            return (Int32)(100 + 10 * Math.Pow(player.WorkerCount, 1.4));
        }

        public override String GetSpecificNameForNotEnoughToBuy(Player player)
        {
            return "a worker";
        }

        public override String GetSpecificNameForSuccessfulBuy(Player player, Int32 quantity)
        {
            return quantity + (quantity == 1 ? " worker" : " workers");
        }

        public override Either<Int32, String> TryBuySingleUnit(Player player, Int32 price)
        {
            if (player.WorkerCount < player.PopulationCount)
            {
                player.WorkerCount++;
                player.Points -= price;
                return () => 1;
            }
            else
            {
                return () => NotEnoughPopulationMessage;
            }
        }
    }
}
