using PawnShop.EF.Data;
using PawnShop.EF.Models;

namespace PawnShop.Services.DataService
{
    public class UnitOfWork : IUnitOfWork
    {
        #region private members

        private PawnshopContext _context = new PawnshopContext();
        private GenericRepository<WorkerBoss> _workerBossRepository;

        #endregion private members

        #region public properties

        public GenericRepository<WorkerBoss> WorkerBossReepository
        {
            get
            {
                if (this._workerBossRepository == null)
                {
                    this._workerBossRepository = new GenericRepository<WorkerBoss>(_context);
                }
                return _workerBossRepository;
            }
        }

        #endregion public properties
    }
}