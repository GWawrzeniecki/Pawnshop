using System.Threading.Tasks;
using PawnShop.Business.Models;
using PawnShop.DataAccess.Data;
using PawnShop.Services.DataService.Repositories;

namespace PawnShop.Services.DataService
{
    public class UnitOfWork : IUnitOfWork
    {
        #region private members

        private readonly PawnshopContext _context = new PawnshopContext();
        private GenericRepository<WorkerBoss> _workerBossRepository;
        private GenericRepository<Person> _personRepository;
        private MoneyBalanceRepository _moneyBalanceRepository;
        private GenericRepository<LendingRate> _lendingRateRepository;
        private GenericRepository<ContractState> _contractStateRepository;
        private ContractRepository _contractRepository;
        private ClientRepository _clientRepository;

        #endregion private members

        #region public properties

        public GenericRepository<WorkerBoss> WorkerBossRepository
        {
            get
            {
                this._workerBossRepository ??= new GenericRepository<WorkerBoss>(_context);
                return _workerBossRepository;
            }
        }

        public GenericRepository<Person> PersonRepository
        {
            get
            {
                this._personRepository ??= new GenericRepository<Person>(_context);
                return _personRepository;
            }
        }

        public MoneyBalanceRepository MoneyBalanceRepository
        {
            get
            {
                this._moneyBalanceRepository ??= new MoneyBalanceRepository(_context);
                return _moneyBalanceRepository;
            }
        }

        public ContractRepository ContractRepository
        {
            get
            {
                this._contractRepository ??= new ContractRepository(_context);
                return _contractRepository;
            }
        }

        public GenericRepository<LendingRate> LendingRateRepository
        {
            get
            {
                this._lendingRateRepository ??= new GenericRepository<LendingRate>(_context);
                return _lendingRateRepository;
            }
        }

        public GenericRepository<ContractState> ContractStateRepository
        {
            get
            {
                this._contractStateRepository ??= new GenericRepository<ContractState>(_context);
                return _contractStateRepository;
            }
        }

        public ClientRepository ClientRepository
        {
            get
            {
                this._clientRepository ??= new ClientRepository(_context);
                return _clientRepository;
            }
        }

        #endregion public properties

        #region  public Methods

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}