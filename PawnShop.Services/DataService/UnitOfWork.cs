using PawnShop.DataAccess.Data;
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
        private GenericRepository<LendingRate> _lendingRateRepository;
        private GenericRepository<ContractState> _contractStateRepository;
        private ContractRepository _contractRepository;

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

        public ContractRepository ContractRepository
        {
            get
            {
                if (this._contractRepository == null)
                {
                    this._contractRepository = new ContractRepository(_context);
                }
                return _contractRepository;
            }
        }

        public GenericRepository<LendingRate> LendingRateRepository
        {
            get
            {
                if (this._lendingRateRepository == null)
                {
                    this._lendingRateRepository = new GenericRepository<LendingRate>(_context);
                }
                return _lendingRateRepository;
            }
        }

        public GenericRepository<ContractState> ContractStateRepository
        {
            get
            {
                if (this._contractStateRepository == null)
                {
                    this._contractStateRepository = new GenericRepository<ContractState>(_context);
                }
                return _contractStateRepository;
            }
        }

        #endregion public properties
    }
}