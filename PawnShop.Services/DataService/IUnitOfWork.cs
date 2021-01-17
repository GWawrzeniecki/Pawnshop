using PawnShop.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PawnShop.Services.DataService
{
    public interface IUnitOfWork
    {
        public GenericRepository<WorkerBoss> WorkerBossReepository { get; }
    }
}
