using PawnShop.Business.Models;

namespace PawnShop.Services.Interfaces
{
    public interface ICalculateService
    {
        public decimal CalculateContractAmount(decimal estimatedValue, LendingRate lendingRate);
        public decimal CalculateNetStorageCost(decimal estimatedValue, LendingRate lendingRate);
    }
}