using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IziHardGames.Playgrounds.ForEfCore.Design
{

    public class DesignDbContext(DbContextOptions<DesignDbContext> opt, ILogger<DesignDbContext> logger) : DbContext(opt)
    {

    }
}
