using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Models;
using PawnShop.DataAccess.Data;
using PawnShop.Services.DataService.Repositories;
using Prism.Ioc;
using System;
using System.Threading.Tasks;

namespace PawnShop.Services.DataService
{
    public class UnitOfWork : IUnitOfWork
    {

        #region private members

        private readonly IContainerProvider _containerProvider;
        public readonly PawnshopContext _context;
        private WorkerBossRepository _workerBossRepository;
        private GenericRepository<Person> _personRepository;
        private MoneyBalanceRepository _moneyBalanceRepository;
        private GenericRepository<LendingRate> _lendingRateRepository;
        private GenericRepository<ContractState> _contractStateRepository;
        private ContractRepository _contractRepository;
        private ClientRepository _clientRepository;
        private ContractItemRepository _contractItemRepository;
        private GenericRepository<ContractItemCategory> _contractItemCategoryRepository;
        private GenericRepository<UnitMeasure> _unitMeasureRepository;
        private GenericRepository<ContractItemState> _contractItemStateRepository;
        private GenericRepository<PaymentType> _paymentTypeRepository;
        private bool _disposed;
        private GenericRepository<Country> _countryRepository;
        private GenericRepository<City> _cityRepository;
        private GenericRepository<WorkerBossType> _workerBossTypeRepository;
        private SaleRepository _saleRepository;
        private DealDocumentRepository _dealDocumentRepository;

        #endregion private members

        #region Constructors

        public UnitOfWork(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
            _context = new();
        }

        public UnitOfWork(IContainerProvider containerProvider, DbContextOptions<PawnshopContext> options)
        {
            _containerProvider = containerProvider;
            _context = new PawnshopContext(options);
        }

        #endregion

        #region public properties

        public WorkerBossRepository WorkerBossRepository
        {
            get
            {
                _workerBossRepository ??= new WorkerBossRepository(_context);
                return _workerBossRepository;
            }
        }

        public GenericRepository<Person> PersonRepository
        {
            get
            {
                _personRepository ??= new GenericRepository<Person>(_context);
                return _personRepository;
            }
        }

        public MoneyBalanceRepository MoneyBalanceRepository
        {
            get
            {
                _moneyBalanceRepository ??= new MoneyBalanceRepository(_context);
                return _moneyBalanceRepository;
            }
        }

        public ContractRepository ContractRepository
        {
            get
            {
                _contractRepository ??= new ContractRepository(_context, _containerProvider);
                return _contractRepository;
            }
        }

        public GenericRepository<LendingRate> LendingRateRepository
        {
            get
            {
                _lendingRateRepository ??= new GenericRepository<LendingRate>(_context);
                return _lendingRateRepository;
            }
        }

        public GenericRepository<ContractState> ContractStateRepository
        {
            get
            {
                _contractStateRepository ??= new GenericRepository<ContractState>(_context);
                return _contractStateRepository;
            }
        }

        public ClientRepository ClientRepository
        {
            get
            {
                _clientRepository ??= new ClientRepository(_context);
                return _clientRepository;
            }
        }

        public ContractItemRepository ContractItemRepository
        {
            get
            {
                _contractItemRepository ??= new ContractItemRepository(_context);
                return _contractItemRepository;
            }
        }

        public GenericRepository<ContractItemCategory> ContractItemCategoryRepository
        {
            get
            {
                _contractItemCategoryRepository ??= new GenericRepository<ContractItemCategory>(_context);
                return _contractItemCategoryRepository;
            }
        }

        public GenericRepository<UnitMeasure> UnitMeasureRepository
        {
            get
            {
                _unitMeasureRepository ??= new GenericRepository<UnitMeasure>(_context);
                return _unitMeasureRepository;
            }
        }

        public GenericRepository<ContractItemState> ContractItemStateRepository
        {
            get
            {
                _contractItemStateRepository ??= new GenericRepository<ContractItemState>(_context);
                return _contractItemStateRepository;
            }
        }

        public GenericRepository<PaymentType> PaymentTypeRepository
        {
            get
            {
                _paymentTypeRepository ??= new GenericRepository<PaymentType>(_context);
                return _paymentTypeRepository;
            }
        }

        public GenericRepository<Country> CountryRepository
        {
            get
            {
                _countryRepository ??= new GenericRepository<Country>(_context);
                return _countryRepository;
            }
        }

        public GenericRepository<City> CityRepository
        {
            get
            {
                _cityRepository ??= new GenericRepository<City>(_context);
                return _cityRepository;
            }
        }

        public GenericRepository<WorkerBossType> WorkerBossTypeRepository
        {
            get
            {
                _workerBossTypeRepository ??= new GenericRepository<WorkerBossType>(_context);
                return _workerBossTypeRepository;
            }
        }

        public SaleRepository SaleRepository
        {
            get
            {
                _saleRepository ??= new SaleRepository(_context);
                return _saleRepository;
            }
        }

        public DealDocumentRepository DealDocumentRepository
        {
            get
            {
                _dealDocumentRepository ??= new DealDocumentRepository(_context);
                return _dealDocumentRepository;
            }
        }

        #endregion public properties

        #region public Methods

        public void SaveChanges()
        {

            _context.SaveChanges();

        }

        public async Task SaveChangesAsync()
        {

            await _context.SaveChangesAsync();

        }

        #endregion

        #region PrivateMethod



        #endregion

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
