using PawnShop.Business.Data;
using PawnShop.Business.Models;
using PawnShop.Services.DataService.Repositories;

namespace PawnShop.Services.DataService
{
    public class UnitOfWork : IUnitOfWork
    {
        #region private members

        private PawnshopContext _context = new PawnshopContext();
        private GenericRepository<WorkerBoss> _workerBossRepository;
        private GenericRepository<Person> _personRepository;
        private MoneyBalanceRepository _moneyBalanceRepository;

        #endregion private members

        #region public properties

        public GenericRepository<WorkerBoss> WorkerBossRepository
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

        public GenericRepository<Person> PersonRepository
        {
            get
            {
                if (this._personRepository == null)
                {
                    this._personRepository = new GenericRepository<Person>(_context);
                }
                return _personRepository;
            }
        }

        public MoneyBalanceRepository MoneyBalanceRepository
        {
            get
            {
                if (this._moneyBalanceRepository == null)
                {
                    this._moneyBalanceRepository = new MoneyBalanceRepository(_context);
                }
                return _moneyBalanceRepository;
            }
        }

        #endregion public properties
    }
}