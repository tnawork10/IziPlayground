using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IziHardGames.Playgrounds.ForEfCore
{
    public class PlaygroundDbContext : DbContext
    {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        public DbSet<Model0> Models0 { get; set; }
        public DbSet<Model1> Models1 { get; set; }
        public DbSet<Model2> Models2 { get; set; }
        public DbSet<ModelA> ModelsA { get; set; }
        public DbSet<ModelB> ModelsB { get; set; }

        public static int counter;

        //public PlaygroundDbContext(DbContextOptions<PlaygroundDbContext> options) : base(options)
        //{

        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseNpgsql("server=127.0.0.1;uid=ngoc;pwd=ngoc;database=playground_ef_core");
            optionsBuilder
                .UseNpgsql("server=127.0.0.1;uid=postgres;pwd=postgres;database=playground_ef_core")
                .UseLoggerFactory(MyLoggerFactory)
                .EnableSensitiveDataLogging();
        }


        public static Model0 CreateModel0()
        {
            return new Model0()
            {
                UniqString = Guid.NewGuid().ToString(),
                Guid = Guid.NewGuid(),

                Model1s = new List<Model1>()
                {
                    CreateModel1(),
                    CreateModel1(),
                    CreateModel1(),
                    CreateModel1(),
                    CreateModel1(),
                },
            };
        }

        public static Model1 CreateModel1()
        {
            return new Model1()
            {
                Model2s = new List<Model2>()
                    {
                        CreateModel2(),
                        CreateModel2(),
                        CreateModel2(),
                        CreateModel2(),
                        CreateModel2(),
                    }
            };
        }
        public static Model2 CreateModel2()
        {
            return new Model2()
            {
                ModelA = new ModelA()
                {

                },
                ModelB = new ModelB()
                {
                    Name = DateTime.Now.ToString(),
                    KeyCounter = (++counter).ToString(),
                },
            };
        }

        public static async Task RecreateAsync()
        {
            using var context = new PlaygroundDbContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            context.Models0.Add(CreateModel0());
            await context.SaveChangesAsync();
        }
    }

    [Index(nameof(UniqString), IsUnique = true)]
    public class Model0
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string UniqString { get; set; } = string.Empty;
        public string String { get; set; } = string.Empty;
        public int Model1Id { get; set; }
        public ICollection<Model1> Model1s { get; set; }
        //public ICollection<ModelB> ModelBs { get; }
    }

    public class Model1
    {
        public int Id { get; set; }
        public long ValueAsLong { get; set; }
        public ICollection<Model2> Model2s { get; set; }
        public Model0 Model0 { get; set; }
    }

    public class Model2
    {
        public int Id { get; set; }
        public ModelA ModelA { get; set; }
        public ModelB ModelB { get; set; }
        public Model1 Model1 { get; set; }
    }

    public class ModelA
    {
        public int Id { get; set; }
    }

    [Index(nameof(KeyCounter), IsUnique = true)]
    public class ModelB
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string KeyCounter { get; set; }
    }
}
