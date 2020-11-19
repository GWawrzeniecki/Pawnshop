using Pawnshop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pawnshop.DAL.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly PawnshopContext _context = new PawnshopContext();
        private GenericRepository<Client> _clientRepository;
        

        public GenericRepository<Client> ClientRepository
        {
            get
            {

                if (this._clientRepository == null)
                {
                    this._clientRepository = new GenericRepository<Client>(_context);
                }
                return _clientRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;



        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
