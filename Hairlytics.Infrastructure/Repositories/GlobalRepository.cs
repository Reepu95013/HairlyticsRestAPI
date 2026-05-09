using Hairlytics.Domain.Interfaces;
using Hairlytics.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Infrastructure.Repositories
{
    public class GlobalRepository : IGlobalRepository
    {
        private readonly ApplicationDbContext _context;
        public GlobalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveDbContextAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
