using System.Threading.Tasks;

namespace PawnShop.Services.Interfaces
{
    public interface IApiService
    {
        public Task<T> GetResponseInJson<T>(string get) where T : class;
    }
}