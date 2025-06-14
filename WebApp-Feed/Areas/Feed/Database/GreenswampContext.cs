﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApp_Feed.Models;
using WebApp_Feed.Areas.Feed.Models;

namespace WebApp_Feed.Areas.Feed.Database;
//
// This file was created by EF Tools:
// Scaffold-DbContext "Data Source=../Data/Greenswamp.db" Microsoft.EntityFrameworkCore.Sqlite -Force
//
public class GreenswampContext : DbContext
{
    public GreenswampContext(DbContextOptions<GreenswampContext> options) : base(options) {}

    public DbSet<Breed> Breeds { get; set; }
    public DbSet<Image> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Breed>() // One-to-Many
            .HasMany(b => b.Images)
            .WithOne(i => i.Breed)
            .HasForeignKey(i => i.BreedId)
            .OnDelete(DeleteBehavior.Cascade); // удалит и Images

        modelBuilder.Entity<Breed>() //Many-to-Many
            .HasMany(b => b.Temperaments)
            .WithMany(t => t.Breeds)
            .UsingEntity(j => j.ToTable("BreedTemperament"));  // удаляет только связанные строки

    }

}

