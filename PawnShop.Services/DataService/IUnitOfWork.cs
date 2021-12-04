using PawnShop.Business.Models;
using PawnShop.Services.DataService.Repositories;
using System;
using System.Threading.Tasks;

namespace PawnShop.Services.DataService
{
    public interface IUnitOfWork : IDisposable
    {
        public void SaveChanges();
        public Task SaveChangesAsync();
        public WorkerBossRepository WorkerBossRepository { get; }
        public GenericRepository<Person> PersonRepository { get; }

        public MoneyBalanceRepository MoneyBalanceRepository { get; }
        public ContractRepository ContractRepository { get; }
        public GenericRepository<LendingRate> LendingRateRepository { get; }
        public GenericRepository<ContractState> ContractStateRepository { get; }
        public ClientRepository ClientRepository { get; }
        public ContractItemRepository ContractItemRepository { get; }
        public GenericRepository<ContractItemCategory> ContractItemCategoryRepository { get; }
        public GenericRepository<UnitMeasure> UnitMeasureRepository { get; }
        public GenericRepository<ContractItemState> ContractItemStateRepository { get; }
        public GenericRepository<PaymentType> PaymentTypeRepository { get; }
        public GenericRepository<Country> CountryRepository { get; }
        public GenericRepository<City> CityRepository { get; }
        public GenericRepository<WorkerBossType> WorkerBossTypeRepository { get; }
        public SaleRepository SaleRepository { get; }
        public DealDocumentRepository DealDocumentRepository { get; }
    }
}