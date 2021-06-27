using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.DataAccess.Data;
using PawnShop.Services.DataService.Repositories;
using System.Threading.Tasks;

namespace PawnShop.Services.DataService
{
    public class UnitOfWork : IUnitOfWork
    {


        #region private members
        private readonly IMapper _mapper;
        private readonly PawnshopContext _context = new PawnshopContext();
        private GenericRepository<WorkerBoss> _workerBossRepository;
        private GenericRepository<Person> _personRepository;
        private MoneyBalanceRepository _moneyBalanceRepository;
        private GenericRepository<LendingRate> _lendingRateRepository;
        private GenericRepository<ContractState> _contractStateRepository;
        private ContractRepository _contractRepository;
        private ClientRepository _clientRepository;
        private GenericRepository<ContractItem> _contractItemRepository;
        private GenericRepository<ContractItemCategory> _contractItemCategoryRepository;
        private GenericRepository<UnitMeasure> _unitMeasureRepository;
        private GenericRepository<ContractItemState> _contractItemStateRepository;
        private GenericRepository<PaymentType> _paymentTypeRepository;

        #endregion private members

        public UnitOfWork(IMapper mapper)
        {
            _mapper = mapper;
        }

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
                this._contractRepository ??= new ContractRepository(_context, this, _mapper);
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

        public GenericRepository<ContractItem> ContractItemRepository
        {
            get
            {
                this._contractItemRepository ??= new GenericRepository<ContractItem>(_context);
                return _contractItemRepository;
            }
        }

        public GenericRepository<ContractItemCategory> ContractItemCategoryRepository
        {
            get
            {
                this._contractItemCategoryRepository ??= new GenericRepository<ContractItemCategory>(_context);
                return _contractItemCategoryRepository;
            }
        }

        public GenericRepository<UnitMeasure> UnitMeasureRepository
        {
            get
            {
                this._unitMeasureRepository ??= new GenericRepository<UnitMeasure>(_context);
                return _unitMeasureRepository;
            }
        }

        public GenericRepository<ContractItemState> ContractItemStateRepository
        {
            get
            {
                this._contractItemStateRepository ??= new GenericRepository<ContractItemState>(_context);
                return _contractItemStateRepository;
            }
        }

        public GenericRepository<PaymentType> PaymentTypeRepository
        {
            get
            {
                this._paymentTypeRepository ??= new GenericRepository<PaymentType>(_context);
                return _paymentTypeRepository;
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