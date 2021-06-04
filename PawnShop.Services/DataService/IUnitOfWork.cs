using PawnShop.Business.Models;
using PawnShop.Services.DataService.Repositories;
using System.Threading.Tasks;

namespace PawnShop.Services.DataService
{
    public interface IUnitOfWork
    {
        public void SaveChanges();
        public Task SaveChangesAsync();
        public GenericRepository<WorkerBoss> WorkerBossRepository { get; }
        public GenericRepository<Person> PersonRepository { get; }

        public MoneyBalanceRepository MoneyBalanceRepository { get; }
        public ContractRepository ContractRepository { get; }
        public GenericRepository<LendingRate> LendingRateRepository { get; }
        public GenericRepository<ContractState> ContractStateRepository { get; }
        public ClientRepository ClientRepository { get; }
        public GenericRepository<ContractItem> ContractItemRepository { get; }
        public GenericRepository<ContractItemCategory> ContractItemCategoryRepository { get; }
        public GenericRepository<UnitMeasure> UnitMeasureRepository { get; }
        public GenericRepository<ContractItemState> ContractItemStateRepository { get; }
    }
}