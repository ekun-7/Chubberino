﻿using Chubberino.Database.Models;
using Chubberino.Modules.CheeseGame.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace Chubberino.Database.Contexts
{
    public interface IApplicationContext
    {
        DbSet<Player> Players { get; set; }

        DbSet<StartupChannel> StartupChannels { get; set; }

        DbSet<UserCredentials> UserCredentials { get; set; }

        DbSet<ApplicationCredentials> ApplicationCredentials { get; set; }

        EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

        Int32 SaveChanges();
    }
}