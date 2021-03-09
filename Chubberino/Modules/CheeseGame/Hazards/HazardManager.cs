﻿using Chubberino.Client.Abstractions;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Utility;
using System;

namespace Chubberino.Modules.CheeseGame.Hazards
{
    public sealed class HazardManager : AbstractCommandStrategy, IHazardManager
    {
        public const String NewInfestationMessage = "A giant mouse sneaks into your factory, scaring away your workers. ";

        public const String OldInfestationMessage = "A giant mouse is still infesting your cheese factory, scaring away your workers. ";

        public const String KillOldRatMessage = "You set up a mousetrap, killing the giant mouse infesting your cheese factory. Your workers go back to the work. ";

        public const String KillNewRatMessage = "A giant mouse sneaks into your factory, but is promptly killed by a mousetrap you have set up. ";

        public HazardManager(IApplicationContext context, ITwitchClientManager client, Random random, IEmoteManager emoteManager) : base(context, client, random, emoteManager)
        {
        }

        public Int32 GetMouseInfestationPointLoss(Int32 points)
        {
            return (Int32)(points * 0.8);
        }

        public String UpdateMouseInfestationStatus(Player player)
        {
            String outputMessage = String.Empty;

            Double infestationChance = ((Double)player.Rank) / 100.0;

            if (player.IsMouseInfested || Random.TryPercentChance(infestationChance))
            {
                if (player.MouseTrapCount > 0)
                {
                    player.MouseTrapCount--;
                    Context.SaveChanges();

                    outputMessage = player.IsMouseInfested
                        ? KillOldRatMessage
                        : KillNewRatMessage;

                    player.IsMouseInfested = false;
                    Context.SaveChanges();
                }
                else
                {
                    outputMessage = player.IsMouseInfested
                        ? OldInfestationMessage
                        : NewInfestationMessage;

                    player.IsMouseInfested = true;
                    Context.SaveChanges();
                }
            }

            return outputMessage;
        }
    }
}
