using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace IziHardGames.Playgrounds.ForEfCore
{
    public class QueryDbContext : DbContext
    {
        public DbSet<EntQueryOne> Ones { get; set; }
        public DbSet<EntQueryToOne> ToOnes { get; set; }
        public DbSet<EntQueryToMany> Manies { get; set; }
        public DbSet<EntityWithValue> EntityWithValues { get; set; }

        public QueryDbContext(DbContextOptions<QueryDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EntQueryOne>().HasOne(x => x.One).WithOne(x => x.One).HasForeignKey<EntQueryToOne>(b => b.OneId);
            modelBuilder.Entity<EntQueryOne>().HasMany(x => x.Many).WithOne(x => x.One);
        }
    }

    public class EntityWithValue
    {
        public Guid Id { get; set; }
        public int UniqValue { get; set; }
        public int Repeat2 { get; set; }
        public int Repeat4 { get; set; }
    }
    public class EntQueryOne
    {
        public int Id { get; set; }
        public EntQueryToOne One { get; set; }
        public ICollection<EntQueryToMany>? Many { get; set; }
    }
    public class EntQueryToOne
    {
        public int Id { get; set; }
        public int OneId { get; set; }
        [JsonIgnore] public EntQueryOne One { get; set; }
    }
    public class EntQueryToMany
    {
        public int Id { get; set; }
        [JsonIgnore] public EntQueryOne One { get; set; }
    }
}
