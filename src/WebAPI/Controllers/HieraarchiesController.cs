//using IziHardGames.Playgrounds.ForEfCore;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace IziPlayGround.Server.Controllers
//{

//    [ApiController]
//    [Route("[controller]")]
//    public class HieraarchiesController : ControllerBase
//    {
//        private PlaygroundSelfHierarchyDbContext context;
//        public Guid guid = new Guid("87349c99-fc36-44da-8d3d-59793a9fd65f");
//        public Guid guid2 = new Guid("108b4196-1c79-4367-968e-7e12b33871ae");
//        public Guid single = new Guid("f8513cb0-d569-410e-9d55-9a6c6d556113");
//        public Guid ltree = new Guid("{9761EE9B-4E1F-4C76-83FE-215703DCD7D0}");

//        public HieraarchiesController(PlaygroundSelfHierarchyDbContext dbContext)
//        {
//            this.context = dbContext;
//        }

//        [HttpPost("InitDataBase")]
//        public async Task<IActionResult> InitDataBase()
//        {
//            await context.Database.EnsureDeletedAsync();
//            await context.Database.EnsureCreatedAsync();
//            return Ok();
//        }


//        [HttpPost("PopulateDbContext")]
//        public async Task<IActionResult> Populate()
//        {
//            await PopulateHieraarchy();
//            await PopulateLTree();
//            await context.SaveChangesAsync();
//            return Ok(new { Hierarchy = await context.Hierarchies.ToArrayAsync(), LTree = await context.LTrees.ToArrayAsync() });
//        }

//        private async Task PopulateHieraarchy()
//        {
//            var parent = new EntityHierarchy()
//            {
//                Guid = guid,
//                Childs = new List<EntityHierarchy>()
//                {
//                    new EntityHierarchy()
//                    {
//                        Guid = guid2,
//                        Childs = new List<EntityHierarchy>()
//                        {
//                            new EntityHierarchy()
//                            {
//                                ParentGuid = guid2,
//                            },
//                            new EntityHierarchy()
//                            {
//                                ParentGuid = guid2,
//                            },
//                            new EntityHierarchy() {
//                                ParentGuid = guid2,
//                            },
//                            new EntityHierarchy() {
//                                ParentGuid = guid2,
//                            }
//                        }
//                    },
//                    new EntityHierarchy() {
//                                ParentGuid = guid,
//                            },
//                    new EntityHierarchy(){
//                                ParentGuid = guid,
//                            },
//                    new EntityHierarchy(){
//                                ParentGuid = guid,
//                            },
//                }
//            };
//            var e = context.Add(parent);
//            var e2 = context.Add(new EntityHierarchy() { Guid = single });
//        }

//        private async Task PopulateLTree()
//        {
//            var root = new EntityHierarchyLTree()
//            {
//                Guid = ltree,
//                Path = $"{ltree}",
//            };
//            var childs = new List<EntityHierarchyLTree>();

//            childs.Add(new EntityHierarchyLTree()
//            {
//                Guid = Guid.NewGuid(),
//                Parent = root,
//            });
//            childs.Add(new EntityHierarchyLTree()
//            {
//                Guid = Guid.NewGuid(),
//                Parent = root,
//                Childs = new EntityHierarchyLTree[]
//                {
//                    new EntityHierarchyLTree()
//                    {
//                        Guid = Guid.NewGuid(),
//                        Childs = new EntityHierarchyLTree[]
//                        {
//                            new EntityHierarchyLTree()
//                            {
//                                Guid = Guid.NewGuid(),
//                            }
//                        }
//                    }
//                }
//            });

//            root.Childs = childs;
//            BuildLTreePath(root);
//            context.LTrees.Add(root);
//        }

//        private void BuildLTreePath(EntityHierarchyLTree item)
//        {
//            var childs = item.Childs;
//            if (childs != null)
//            {
//                foreach (var child in childs)
//                {
//                    child.Path = new LTree($"{item.Path}.{child.Guid}");
//                    BuildLTreePath(child);
//                }
//            }
//        }


//        [HttpGet("GetChilds")]
//        public async Task<IActionResult> GetChilds()
//        {
//            var v = await context.Hierarchies.Where(x => x.Guid == guid).Include(x => x.Childs).ThenInclude(x => x.Childs).ToArrayAsync();
//            return Ok(v);
//        }

//        [HttpPost("Save")]
//        public async Task<IActionResult> Save()
//        {
//            var v = await context.Hierarchies.FindAsync(guid);
//            context.Hierarchies.Update(v);
//            await context.SaveChangesAsync();
//            return Ok(v);
//        }

//        [HttpPost("GetSpan")]
//        public async Task<IActionResult> GetSpan()
//        {
//            return Ok((new int[] { 0, 1, 2, 3, 5, 6 }));
//        }

//        [HttpPost("GetLTree")]
//        public async Task<IActionResult> GetLTree()
//        {
//            var q = context.LTrees.Include(x => x.Childs).Where(x => x.Guid == ltree).ToArrayAsync();
//            return Ok(await q);
//        }

//        [HttpPost("GetLTreeRoot")]
//        public async Task<IActionResult> GetLTreeRoot()
//        {
//            var q = context.LTrees.Where(x => x.Guid == ltree);

//            return NoContent();

//            //var q = q.Select(c => c.Path.IsDescendantOf(c.Path));
//            //var root = await
//            //return Ok(root);
//            //return Ok(await q);
//        }

//        [HttpPost("GetLTreeAll")]
//        public async Task<IActionResult> GetLTreeAll()
//        {
//            // получится одноуровневый список без вложений
//            var q =  context.LTrees.AsNoTracking().Where(x => x.Path.IsDescendantOf(context.LTrees.First(x => x.Guid == ltree).Path)).ToArrayAsync();
//            var flat = await q;
//            // получится постренный с вложениями список
//            var q2 = context.LTrees.Where(x => x.Path.IsDescendantOf(context.LTrees.First(x => x.Guid == ltree).Path)).ToArrayAsync();
//            return Ok(await q2);
//        }
//    }
//}
