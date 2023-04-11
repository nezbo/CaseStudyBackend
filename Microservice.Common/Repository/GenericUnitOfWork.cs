using Microservice.Common.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Common.Repository
{
    public class GenericUnitOfWork : IGenericUnitOfWork
    {
        private readonly IServiceProvider _services;
        private readonly IBaseDbContext _context;

        public GenericUnitOfWork(IServiceProvider services, IBaseDbContext context)
        {
            _services = services;
            _context = context;
        }

        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }

        public IGenericRepository<T> GetRepository<T>()
        {
            return _services.GetService<IGenericRepository<T>>();
        }
    }
}
