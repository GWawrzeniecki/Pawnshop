using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Models;
using PawnShop.DataAccess.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Services.DataService.Repositories
{
    public class WorkerBossRepository : GenericRepository<WorkerBoss>
    {
        #region Private members

        private readonly PawnshopContext _context;

        #endregion

        #region Constructor

        public WorkerBossRepository(PawnshopContext context) : base(context)
        {
            _context = context;
        }

        #endregion

        #region Public methods

        public async Task<IList<WorkerBoss>> GetWorkerBosses()
        {
            return await GetWorkerBossAsQueryable()
                .ToListAsync();
        }

        #endregion

        #region PrivateMethods

        private IQueryable<WorkerBoss> GetWorkerBossAsQueryable()
        {
            return _context.WorkerBosses
                .Include(worker => worker.WorkerBossType)
                .Include(worker => worker.WorkerBossNavigation)
                .ThenInclude(person => person.Address)
                .ThenInclude(address => address.City)
                .Include(worker => worker.WorkerBossNavigation)
                .ThenInclude(person => person.Address)
                .ThenInclude(address => address.Country)
                .Include(worker => worker.Privilege)
                .AsQueryable();
        }

        #endregion
    }
}