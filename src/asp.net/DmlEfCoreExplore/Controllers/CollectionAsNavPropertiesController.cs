using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DmlEfCoreExplore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollectionAsNavPropertiesController(DmlResearchContext context) : ControllerBase
    {
        [HttpGet(nameof(Init))]
        public async Task<IActionResult> Init()
        {
            var parent = new DmlCollectionNavPropParent();

            context.CollectionNavChilds.Add(new DmlCollectionNavPropChild()
            {
                Parent = parent,
            });
            context.CollectionNavChilds.Add(new DmlCollectionNavPropChild()
            {
                Parent = parent,
            });
            context.CollectionNavChilds.Add(new DmlCollectionNavPropChild()
            {
                Parent = parent,
            });
            context.CollectionNavChilds.Add(new DmlCollectionNavPropChild()
            {
                Parent = parent,
            });
            await context.SaveChangesAsync();
            return Ok(await context.CollectionNavParents.ToArrayAsync());
        }
        [HttpGet(nameof(GetAllParents))]
        public async Task<IActionResult> GetAllParents()
        {
            return Ok(await context.CollectionNavParents.Include(x => x.Childs).ToArrayAsync());
        }


        [HttpGet(nameof(UpdateChildsWithAdd))]
        public async Task<IActionResult> UpdateChildsWithAdd()
        {
            var first = await context.CollectionNavParents.FirstAsync();
            // добавояет к существуюим чилдам еще 2
            first.Childs = new List<DmlCollectionNavPropChild>
            {
                new DmlCollectionNavPropChild()
                {
                    Parent = first,
                },
                new DmlCollectionNavPropChild()
                {
                    Parent = first,
                },
            };
            await context.SaveChangesAsync();
            return Ok(await context.CollectionNavParents.Include(x => x.Childs).ToArrayAsync());
        }

        [HttpGet(nameof(UpdateChildsWithReplace))]
        public async Task<IActionResult> UpdateChildsWithReplace()
        {
            //var first = await context.CollectionNavParents.Include(x => x.Childs).FirstAsync();
            var first = await context.CollectionNavParents.FirstAsync();

            // deletes all exited childs and add what in list when included,
            first.Childs = new List<DmlCollectionNavPropChild>
            {
                new DmlCollectionNavPropChild()
                {
                    Parent = first,
                },
                new DmlCollectionNavPropChild()
                {
                    Parent = first,
                },
            };
            // добавояет к существуюим чилдам еще 2
            await context.SaveChangesAsync();
            return Ok(await context.CollectionNavParents.Include(x => x.Childs).ToArrayAsync());
        }

        [HttpGet(nameof(ClearChilds))]
        public async Task<IActionResult> ClearChilds()
        {
            // with child ON DELETE CASCADE
            var first = await context.CollectionNavParents.Include(x => x.Childs).FirstAsync();
            //var first = await context.CollectionNavParents.FirstAsync();  
            //first.Childs = new List<DmlCollectionNavPropChild>();   // not working when not include, works when include() 
            //first.Childs = null;   // works, when include, not working when not include()
            //first.Childs.Clear();   // works when include()
            // childs deleted from table in all cases
            await context.SaveChangesAsync();
            return Ok(await context.CollectionNavParents.Include(x => x.Childs).ToArrayAsync());
        }


        [HttpGet(nameof(ClearChildsConditional))]
        public async Task<IActionResult> ClearChildsConditional()
        {
            var first = await context.CollectionNavParents.FirstOrDefaultAsync();
            var count = await context.CollectionNavParents.SelectMany(x => x.Childs).Where(x => (x.Id % 2) != 0).ExecuteDeleteAsync();
            return Ok(await context.CollectionNavParents.Include(x => x.Childs).Where(x => x.Id == first.Id).ToArrayAsync());
        }
    }
}
