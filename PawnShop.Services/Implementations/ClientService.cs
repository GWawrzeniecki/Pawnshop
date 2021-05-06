using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using PawnShop.Business.Models;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;

namespace PawnShop.Services.Implementations
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
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


        public async Task<Client> CreateClient(Client client)
        {
            try
            {
                return await TryToCreateClient(client);
            }
            catch (Exception e)
            {
                throw new CreateClientException("Wystąpił problem podczas dodawania nowego klienta.", e);
            }
        }

        public async Task<Client> UpdateClient(Client client)
        {
            try
            {
                return await TryToCreateClient(client);
            }
            catch (Exception e)
            {
                throw new CreateClientException("Wystąpił problem podczas aktualizacji danych klienta.", e);
            }
        }

        private async Task<IList<Client>> TryToGetClientBySurname(string surname)
        {
            return await _unitOfWork.ClientRepository.GetClientBySurname(surname);
        }

        private async Task<IList<Client>> TryToGetClientByPesel(string pesel)
        {
            return await _unitOfWork.ClientRepository.GetClientByPesel(pesel);
        }

        private async Task<Client> TryToCreateClient(Client client)
        {
            return await _unitOfWork.ClientRepository.CreateClientAsync(client);
        }

        private async Task<Client> TryToUpdateClient(Client client)
        {
            return await _unitOfWork.ClientRepository.UpdateClientAsync(client);
        }


    }
}