using DmlEfCoreExplore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DmlEfCoreExplore.Application
{
    public class SingletonProxy(IDbContextFactory<PooledDbContext> factory)
    {
        public PooledDbContext Create()
        {
            return factory.CreateDbContext();
        }
    }
}
