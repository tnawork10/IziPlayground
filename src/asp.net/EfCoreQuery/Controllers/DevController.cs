using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EfCoreQuery.Controllers
{
    /// <summary>
    /// Some controller with comments
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DevController : ControllerBase
    {
        private QueryDbContext context;
        /// <summary>
        /// some action with comments
        /// </summary>
        /// <param name="context"></param>
        public DevController(QueryDbContext context)
        {
            this.context = context;
        }
    }
}
