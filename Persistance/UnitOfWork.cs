using System.Threading.Tasks;
using vega.Core;
using Vega.Core;
using Vega.Persistance;

namespace vega.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VegaDbContext _context;

        public UnitOfWork(VegaDbContext context)
        {
            _context = context;
        }
        
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}