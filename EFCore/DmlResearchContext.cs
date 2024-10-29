using System;
using Microsoft.EntityFrameworkCore;

namespace IziHardGames.Playgrounds.ForEfCore
{
    public class DmlResearchContext : DbContext
    {
        public DbSet<DmlEntity> Entities { get; set; }
        public DmlResearchContext(DbContextOptions<DmlResearchContext> options) : base(options)
        {

        }
    }

    public class DmlEntity
    {
        public Guid Id { get; set; }
        public string TextField { get; set; } = "Some";
        public double DoubleField { get; set; } 
        public int IntField { get; set; } 
    }
}
