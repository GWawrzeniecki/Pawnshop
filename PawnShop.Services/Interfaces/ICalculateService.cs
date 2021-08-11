using PawnShop.Business.Models;
using System.Collections.Generic;

namespace PawnShop.Services.Interfaces
{
    public interface ICalculateService
    {
        public decimal CalculateContractAmount(decimal estimatedValue, LendingRate lendingRate);
        public decimal CalculateNetStorageCost(decimal estimatedValue, LendingRate lendingRate);
        public decimal CalculateNetRenewCost(decimal estimatedValue, LendingRate lendingRate, int? delay, IList<LendingRate> lendingRates);
        public decimal CalculateRenewCost(decimal estimatedValue, LendingRate lendingRate, int? delay, IList<LendingRate> lendingRates);
        public decimal CalculateBuyBackCost(decimal estimatedValue, LendingRate lendingRate, int? delay, IList<LendingRate> lendingRates);


    }
}