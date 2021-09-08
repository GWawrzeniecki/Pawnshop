using PawnShop.Business.Models;
using PawnShop.Core.Models.QueryDataModels;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PawnShop.Services.Implementations
{
    public class ClientService : IClientService
    {
        #region PrivateMembers

        private readonly IContainerProvider _containerProvider;

        #endregion

        #region Constructor

        public ClientService(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
        }

        #endregion

        #region PublicMethods

        public async Task<IList<Client>> GetClientBySurname(string surname)
        {
            try
            {
                return await TryToGetClientBySurname(surname);
            }
            catch (Exception e)
            {
                throw new SearchClientsException("Wystąpił problem podczas wyszukiwania klientów po nazwisku.", e);
            }
        }

        public async Task<IList<Client>> GetClientByPesel(string pesel)
        {
            try
            {
                return await TryToGetClientByPesel(pesel);
            }
            catch (Exception e)
            {
                throw new SearchClientsException("Wystąpił problem podczas wyszukiwania klientów po numerze pesel.", e);
            }
        }

        public async Task<IList<Client>> GetClients(int count)
        {
            try
            {
                return await TryToGetClients(count);
            }
            catch (Exception e)
            {
                throw new LoadingClientsException("Wystąpił problem podczas pobierania klientów.", e);
            }
        }

        public async Task<IList<Client>> GetClients(ClientQueryData clientQueryData, int count)
        {
            try
            {
                return await TryToGetClients(clientQueryData, count);
            }
            catch (Exception e)
            {
                throw new SearchClientsException("Wystąpił problem podczas wyszukiwania klientów.", e);
            }
        }

        #endregion

        #region PrivateMethods

        private async Task<IList<Client>> TryToGetClientBySurname(string surname)
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            return await unitOfWork.ClientRepository.GetClientBySurname(surname);
        }

        private async Task<IList<Client>> TryToGetClientByPesel(string pesel)
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            return await unitOfWork.ClientRepository.GetClientByPesel(pesel);
        }

        private async Task<IList<Client>> TryToGetClients(int count)
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            return await unitOfWork.ClientRepository.GetClients(count);
        }

        private async Task<IList<Client>> TryToGetClients(ClientQueryData clientQueryData, int count)
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            return await unitOfWork.ClientRepository.GetClients(clientQueryData, count);
        }

        #endregion
    }
}