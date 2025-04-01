using System;
using Microsoft.EntityFrameworkCore;

namespace IziHardGames.Playgrounds.ForEfCore
{
    public class TypesDbContext(DbContextOptions<TypesDbContext> options) : DbContext(options)
    {
        public DbSet<DateTimeEntity> DateTimes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DateTimeEntity>(x =>
            {
                x.HasKey(x => x.Id);
            });

            modelBuilder.Entity<DateTimeEntity>().HasData(
            new DateTimeEntity()
            {
                Id = 1,
                DateOnly = DateOnly.MinValue,
                DateTime = DateTime.MinValue,
                TimeOnly = TimeOnly.MinValue,
                TimeSpan = TimeSpan.MinValue,
                DateTimeOffset = DateTimeOffset.MinValue,
            },
            new DateTimeEntity()
            {
                Id = 2,
                DateOnly = DateOnly.MaxValue,
                DateTime = DateTime.MaxValue,
                TimeOnly = TimeOnly.MaxValue,
                TimeSpan = TimeSpan.MaxValue,
                DateTimeOffset = DateTimeOffset.MaxValue,
            });
        }
    }

    public class DateTimeEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// timestamptz
        /// </summary>
        public DateTimeOffset DateTimeOffset { get; set; }
        /// <summary>
        /// timestamptz
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// date
        /// </summary>
        public DateOnly DateOnly { get; set; }
        /// <summary>
        /// interval
        /// </summary>
        public TimeSpan TimeSpan { get; set; }
        /// <summary>
        /// time without time zone
        /// </summary>
        public TimeOnly TimeOnly { get; set; }

    }
}
