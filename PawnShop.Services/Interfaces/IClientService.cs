using PawnShop.Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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