using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IziPlayGround.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevController : ControllerBase
    {
        private PlaygroundSelfHierarchyDbContext context;
        public Guid guid = new Guid("87349c99-fc36-44da-8d3d-59793a9fd65f");
        public Guid guid2 = new Guid("108b4196-1c79-4367-968e-7e12b33871ae");
        public Guid single = new Guid("f8513cb0-d569-410e-9d55-9a6c6d556113");

        public DevController(PlaygroundSelfHierarchyDbContext dbContext)
        {
            this.context = dbContext;
        }

        [HttpPost("InitDataBase")]
        public async Task<IActionResult> InitDataBase()
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            return Ok();
        }


        [HttpPost("Populate")]
        public async Task<IActionResult> Populate()
        {
            var parent = new EntityHierarchy()
            {
                Guid = guid,
                Childs = new List<EntityHierarchy>()
                {
                    new EntityHierarchy()
                    {
                        Guid = guid2,
                        Childs = new List<EntityHierarchy>()
                        {
                            new EntityHierarchy()
                            {
                                ParentGuid = guid2,
                            },
                            new EntityHierarchy()
                            {
                                ParentGuid = guid2,
                            },
                            new EntityHierarchy() {
                                ParentGuid = guid2,
                            },
                            new EntityHierarchy() {
                                ParentGuid = guid2,
                            }
                        }
                    },
                    new EntityHierarchy() {
                                ParentGuid = guid,
                            },
                    new EntityHierarchy(){
                                ParentGuid = guid,
                            },
                    new EntityHierarchy(){
                                ParentGuid = guid,
                            },
                }
            };
            var e = context.Add(parent);
            var e2 = context.Add(new EntityHierarchy() { Guid = single });
            await context.SaveChangesAsync();
            return Ok(e.Entity);
        }

        [HttpGet("GetChilds")]
        public async Task<IActionResult> GetChilds()
        {
            var v = await context.Hierarchies.Where(x => x.Guid == guid).Include(x => x.Childs).ThenInclude(x => x.Childs).ToArrayAsync();
            return Ok(v);
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save()
        {
            var v = await context.Hierarchies.FindAsync(guid);
            context.Hierarchies.Add(v);
            await context.SaveChangesAsync();
            return Ok(v);
        }
        [HttpPost("GetSpan")]

        public async Task<IActionResult> GetSpan()
        {
            return Ok((new int[] { 0, 1, 2, 3, 5, 6 }));
        }
    }
}
