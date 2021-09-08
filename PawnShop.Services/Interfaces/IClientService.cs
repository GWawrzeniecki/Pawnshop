using PawnShop.Business.Models;
using PawnShop.Core.Models.QueryDataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PawnShop.Services.Interfaces
{
    public interface IClientService
    {
        Task<IList<Client>> GetClientBySurname(string surname);
        Task<IList<Client>> GetClientByPesel(string pesel);
        Task<IList<Client>> GetClients(ClientQueryData clientQueryData, int count);
        Task<IList<Client>> GetClients(int count);

    }
}