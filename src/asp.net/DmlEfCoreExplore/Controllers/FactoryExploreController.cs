using System;
using Bogus;
using DmlEfCoreExplore.Application;
using DmlEfCoreExplore.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DmlEfCoreExplore.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FactoryExploreController : ControllerBase
    {
        private IDbContextFactory<PooledDbContext> factory;
        private string id;
        private Guid guid;
        private DateTime created;
        private SingletonProxy proxy;

        public FactoryExploreController(IDbContextFactory<PooledDbContext> factory, SingletonProxy proxy)
        {
            this.factory = factory;
            this.id = new Faker().Name.FindName();
            this.guid = Guid.NewGuid();
            this.created = DateTime.Now;
            this.proxy = proxy;
        }

        [HttpGet]
        public async Task<IActionResult> GetContextMeta()
        {
            var db = factory.CreateDbContext();
            var es = await db.Entity01s.ToArrayAsync();
            return Ok(new { IdDb = db.Id, GuidDb = db.Guid, DbCreated = db.Created, IdController = id, GuidController = guid, HashCode = this.GetHashCode(), Created = created, HashCodeFactory = factory.GetHashCode() });
        }

        [HttpGet]
        public async Task<IActionResult> GetContextMetaWithDispose()
        {
            await using var db = factory.CreateDbContext();
            var es = await db.Entity01s.ToArrayAsync();
            return Ok(new { IdDb = db.Id, GuidDb = db.Guid, DbCreated = db.Created, IdController = id, GuidController = guid, HashCode = this.GetHashCode(), Created = created, HashCodeFactory = factory.GetHashCode() });
        }

        [HttpGet]
        public async Task<IActionResult> GetContextThroughProxy()
        {
            var db = proxy.Create();
            var es = await db.Entity01s.ToArrayAsync();
            return Ok(new { IdDb = db.Id, GuidDb = db.Guid, DbCreated = db.Created, IdController = id, GuidController = guid, HashCode = this.GetHashCode(), Created = created, HashCodeFactory = factory.GetHashCode() });
        }

        [HttpGet]
        public async Task<IActionResult> GetContextThroughProxyWithDispose()
        {
            await using var db = proxy.Create();
            var es = await db.Entity01s.ToArrayAsync();
            return Ok(new { IdDb = db.Id, GuidDb = db.Guid, DbCreated = db.Created, IdController = id, GuidController = guid, HashCode = this.GetHashCode(), Created = created, HashCodeFactory = factory.GetHashCode() });
        }

        [HttpGet]
        public IActionResult HashCode()
        {
            return Ok(this.GetHashCode());
        }
    }
}
