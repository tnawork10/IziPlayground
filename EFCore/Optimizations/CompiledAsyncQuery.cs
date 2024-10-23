using System;
using System.Linq;
using System.Threading.Tasks;
using IziHardGames.Playgrounds.ForEfCore;
using Microsoft.EntityFrameworkCore;

namespace IziHardGames
{
    public class CompiledAsyncQuery
    {
        private static readonly Func<PlaygroundDbContext, Guid, Task<Model0?>> func0 =
        EF.CompileAsyncQuery((PlaygroundDbContext context, Guid id) => context.Models0.Where(x => x.Guid == id).FirstOrDefault());

    }
}