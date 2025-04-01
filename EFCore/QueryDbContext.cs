using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IziHardGames.Playgrounds.ForEfCore
{

    public class QueryDbContext : DbContext
    {
        public DbSet<EntQueryOne> Ones { get; set; }
        public DbSet<EntQueryToOne> ToOnes { get; set; }
        public DbSet<EntQueryToMany> Manies { get; set; }
        public DbSet<EntityWithValue> EntityWithValues { get; set; }
        public DbSet<CompositeKeyJoin> CompositeKeyJoins { get; set; }
        public DbSet<CompositeKeyJoinKeys> CompositeKeyJoinsKeys { get; set; }
        public DbSet<EntityPkSimple> EntityPkSimples { get; set; }
        public DbSet<EntityWithCompositeUniqIndex> EntityWithCompositeUniqIndices { get; set; }

        public QueryDbContext(DbContextOptions<QueryDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EntQueryOne>().HasOne(x => x.One).WithOne(x => x.One).HasForeignKey<EntQueryToOne>(b => b.OneId);
            modelBuilder.Entity<EntQueryOne>().HasMany(x => x.Many).WithOne(x => x.One);
            modelBuilder.Entity<CompositeKeyJoin>().HasKey(x => new { x.IdPart1, x.IdPart2 });
            //modelBuilder.Entity<CompositeKeyJoin>().HasKey(x => new KeyTyped(x.IdPart1, x.IdPart2));
            modelBuilder.Entity<EntityWithCompositeUniqIndex>().HasIndex(x => new { x.UniqIndex1, x.UniqIndex2 }).IsUnique();
        }

        public async Task PopulateCompositeKeyJoins()
        {
            for (int i = 1; i < 100; i++)
            {
                for (int j = 1; j < 100; j++)
                {
                    var e = new CompositeKeyJoin()
                    {
                        IdPart1 = i,
                        IdPart2 = j,
                        SomeRandomValue = Guid.NewGuid(),
                        Value = i + j
                    };

                    var e2 = new CompositeKeyJoinKeys()
                    {
                        Id = Guid.NewGuid(),
                        IdPart1 = i,
                        IdPart2 = j,
                    };
                    this.CompositeKeyJoins.Add(e);
                    this.CompositeKeyJoinsKeys.Add(e2);
                }
            }
            await this.SaveChangesAsync();
        }

        public async Task PopulateEntityPkSimples()
        {
            for (int i = 1; i < 1000; i++)
            {
                this.EntityPkSimples.Add(new EntityPkSimple()
                {
                    Id = i,
                    ValueAsInt = 5000 + i,
                    ValuesAsDouble = double.MaxValue / (i),
                });
            }
            await this.SaveChangesAsync();
        }
        public async Task PopulateEntityWithCompositeUniqIndex()
        {
            for (int i = 1; i < 100; i++)
            {
                for (int j = 1; j < 100; j++)
                {
                    this.EntityWithCompositeUniqIndices.Add(new EntityWithCompositeUniqIndex()
                    {
                        Id = Guid.NewGuid(),
                        UniqIndex1 = i,
                        UniqIndex2 = j,
                        ValueAsDouble = double.MaxValue / (i),
                    });
                }
            }
            await this.SaveChangesAsync();
        }

        public static readonly Func<QueryDbContext, IEnumerable<KeyTyped>, Task<List<EntityWithCompositeUniqIndex>>> CompiledV1 =
          EF.CompileAsyncQuery((QueryDbContext context, IEnumerable<KeyTyped> keys) =>
              context.EntityWithCompositeUniqIndices.Where(o => keys.Any(x => x == o.Index)).ToList());
        //context.EntityWithCompositeUniqIndices.Join(keys, x => new { x.UniqIndex1, x.UniqIndex2 }, y => new { UniqIndex1 = y.KeyIdPart1, UniqIndex2 = y.KeyIdPart2 }, (x, y) => x).ToList());
    }



    public class EntityPkSimple
    {
        [Key]
        public int Id { get; set; }
        public int ValueAsInt { get; set; }
        public double ValuesAsDouble { get; set; }
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

    public class EntityWithCompositeUniqIndex
    {
        public Guid Id { get; set; }
        public int UniqIndex1 { get; set; }
        public int UniqIndex2 { get; set; }
        public double ValueAsDouble { get; set; }
        public KeyTyped Index => new KeyTyped(UniqIndex1, UniqIndex2);
    }
    public class CompositeKeyJoin
    {
        //public object Key => new { IdPart1, IdPart2 };
        //public KeyTyped KeyTyped => new KeyTyped(IdPart1, IdPart2);
        public int IdPart1 { get; set; }
        public int IdPart2 { get; set; }
        public double Value { get; set; }
        public Guid SomeRandomValue { get; set; }
    }
    public class CompositeKeyJoinKeys
    {
        public Guid Id { get; set; }
        public int IdPart1 { get; set; }
        public int IdPart2 { get; set; }
    }

    public struct KeyTyped
    {
        public KeyTyped(int idPart1, int idPart2)
        {
            KeyIdPart1 = idPart1;
            KeyIdPart2 = idPart2;
        }
        public int KeyIdPart1 { get; set; }
        public int KeyIdPart2 { get; set; }


        public static bool operator ==(KeyTyped p1, KeyTyped p2)
        {
            return p1.KeyIdPart1 == p2.KeyIdPart1 && p1.KeyIdPart2 == p2.KeyIdPart2;
        }
        public static bool operator !=(KeyTyped p1, KeyTyped p2)
        {
            return p1.KeyIdPart1 != p2.KeyIdPart1 && p1.KeyIdPart2 != p2.KeyIdPart2;
        }
    }

    public class TypeCompositeKeyObj
    {
        public int IdPart1 { get; set; }
        public int IdPart2 { get; set; }
    }
}
