using System.Reflection;
using DevextremeAPI.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace IziHardGames.DevExtremeAPI.DAL
{
    public class DevExtremeDbContext(DbContextOptions<DevExtremeDbContext> options) : DbContext(options)
    {
        public DbSet<DevExtremeRecord> Records { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DevExtremeRecord>(x =>
            {
                for (int i = 0; i < 100; i++)
                {
                    x.HasData(new DevExtremeRecord()
                    {
                        Id = i + 1,
                        Groupe = i % 5,
                        ValueAsInt = 500 + i,
                        ValueAsDouble = i + 0.05,
                        ValueAsString = new DateTime(2025, 01, 01).AddDays(i).ToString()
                    });
                }
            });
        }


        public async Task<object> QueueDistinctsColumn<TColumn>(string dataField, string table)
        {
            var mi = typeof(DevExtremeDbContext).GetMethod(nameof(SetFromName), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            ArgumentNullException.ThrowIfNull(mi);
            var prop = typeof(TColumn).GetProperty(dataField, BindingFlags.Public | BindingFlags.Instance);
            ArgumentNullException.ThrowIfNull(prop);
            var type = prop.PropertyType;
            var miGen = mi.MakeGenericMethod(type);
            var task = miGen.Invoke(null, [this.Database, dataField, table]);
            var taskCasted = (task as Task) ?? throw new InvalidCastException();
            await taskCasted;
            var result = (taskCasted as dynamic).Result;
            ArgumentNullException.ThrowIfNull(result);
            return result;
        }

        public static async Task<T[]> SetFromName<T>(DatabaseFacade facade, string dataField, string table)
        {
            using var command = facade.GetDbConnection().CreateCommand();
            var sql = $"SELECT DISTINCT \"{dataField}\" FROM \"{table}\"";
            var q = facade.SqlQueryRaw<T>(sql);
            var result = await q.ToArrayAsync();
            return result;
        }
    }
}
