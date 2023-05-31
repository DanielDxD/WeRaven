using WeRaven.Api.Data;
using WeRaven.Api.Repositories.Interfaces;

namespace WeRaven.Api.Repositories
{
    public class RepositoryBase : IRepositoryBase
    {
        private readonly AppDbContext _context;
        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();  
        }
    }
}
