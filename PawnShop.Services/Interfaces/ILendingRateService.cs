using PawnShop.Business.Models;
using System.Threading.Tasks;

namespace PawnShop.Services.Interfaces
{
    public interface ILendingRateService
    {
        public Task AddLendingRate(int days, int percentage);
        public Task EditLendingRate(LendingRate lendingRate);
        public Task DeleteLendingRate(LendingRate lendingRate);
    }
}