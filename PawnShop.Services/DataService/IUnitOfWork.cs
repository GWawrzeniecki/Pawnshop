using PawnShop.Business.Models;
using PawnShop.Services.DataService.Repositories;

namespace PawnShop.Services.DataService
{
    public interface IUnitOfWork
    {
        public GenericRepository<WorkerBoss> WorkerBossRepository { get; }
        public GenericRepository<Person> PersonRepository { get; }

        public MoneyBalanceRepository MoneyBalanceRepository { get; }

    }
}