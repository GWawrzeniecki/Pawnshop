using System.Collections.Generic;
using System.Threading.Tasks;
using PawnShop.Business.Models;

namespace PawnShop.Services.Interfaces
{
    public interface IClientService
    {
        Task<IList<Client>> GetClientBySurname(string surname);

        Task<IList<Client>> GetClientByPesel(string pesel);

        Task<Client> CreateClient(Client client);
        Task<Client> UpdateClient(Client client);

    }
}