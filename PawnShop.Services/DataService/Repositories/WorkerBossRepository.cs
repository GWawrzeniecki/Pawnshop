using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Dtos;
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

        public async Task<WorkerBossLoginDto> GetWorkerBossLogin(string login)
        {
            return await _context
                .WorkerBosses
                .Include(w => w.Privilege)
                .Include(w => w.WorkerBossNavigation)
                .Select(w => new WorkerBossLoginDto()
                {
                    WorkerBossNavigation = new Person()
                    {
                        FirstName = w.WorkerBossNavigation.FirstName,
                        LastName = w.WorkerBossNavigation.LastName,
                        PersonId = w.WorkerBossNavigation.PersonId
                    },
                    WorkerBossId = w.WorkerBossId,
                    WorkerBossTypeId = w.WorkerBossTypeId,
                    Login = w.Login,
                    Hash = w.Hash,
                    Privilege = w.Privilege,
                    PrivilegeId = w.PrivilegeId
                })
                .FirstOrDefaultAsync(w => w.Login.Equals(login));
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