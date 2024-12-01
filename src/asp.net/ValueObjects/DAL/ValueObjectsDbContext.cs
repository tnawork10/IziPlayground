using Microsoft.EntityFrameworkCore;

namespace ValueObjects.DAL
{
    public class ValueObjectsDbContext : DbContext
    {
        public DbSet<EntityValutObjects> EntityValutObjects { get; set; }

        public ValueObjectsDbContext(DbContextOptions<ValueObjectsDbContext> options) : base(options)
        {

        }
    }

    public class EntityValutObjects
    {
        public int Id { get; set; }
        public VoGuid VoGuid { get; set; }
    }

    public struct VoGuid
    {
        public Guid Guid { get; set; }
    }
}