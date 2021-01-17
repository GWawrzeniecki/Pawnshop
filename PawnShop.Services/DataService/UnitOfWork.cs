using PawnShop.EF.Models;
using PawnShop.EF.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PawnShop.Services.DataService
{
    public class UnitOfWork : IUnitOfWork
    {
        #region private members
        private PawnshopContext _context = new PawnshopContext();
        private GenericRepository<WorkerBoss> _workerBossRepository;
        #endregion

        #region public properties
        public GenericRepository<WorkerBoss> WorkerBossReepository
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
        #endregion
    }
}
