using PawnShop.DataAccess.Models;

namespace PawnShop.Services.DataService
{
    public interface IUnitOfWork
    {
        public GenericRepository<WorkerBoss> WorkerBossReepository { get; }
    }
}