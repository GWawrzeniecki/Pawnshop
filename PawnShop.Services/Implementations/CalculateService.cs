using PawnShop.Business.Models;
using PawnShop.Core.SharedVariables;
using PawnShop.Services.Interfaces;
using System;

namespace PawnShop.Services.Implementations
{
    public class CalculateService : ICalculateService
    {
        private readonly IConfigData _configData;

        public CalculateService(IConfigData configData)
        {
            _configData = configData;
        }



        private static decimal GetNetStorageCost(decimal estimatedValue, LendingRate lendingRate) => estimatedValue * lendingRate.Procent / 100;
        private decimal AddVat(decimal netAmount) => (netAmount * _configData.VatPercent / 100) + netAmount;

        private static decimal GetNetRenewCost(decimal estimatedValue, LendingRate lendingRate, int? delay)
        {
            if (delay is null or 0)
            {
                return GetNetStorageCost(estimatedValue, lendingRate);
            }
            else
            {
                return 0;
            }
        }

        public decimal CalculateContractAmount(decimal estimatedValue, LendingRate lendingRate)
        {
            return lendingRate == null
                ? throw new ArgumentNullException(nameof(lendingRate))
                : estimatedValue + decimal.Round(AddVat(GetNetStorageCost(estimatedValue, lendingRate)));
        }

        public decimal CalculateNetStorageCost(decimal estimatedValue, LendingRate lendingRate)
        {
            return lendingRate == null
                ? throw new ArgumentNullException(nameof(lendingRate))
                : GetNetStorageCost(estimatedValue, lendingRate);
        }

        public decimal CalculateNetRenewCost(decimal estimatedValue, LendingRate lendingRate, int? delay)
        {
            return lendingRate == null
                ? throw new ArgumentNullException(nameof(lendingRate))
                : GetNetRenewCost(estimatedValue, lendingRate, delay);
        }

        public decimal CalculateRenewCost(decimal estimatedValue, LendingRate lendingRate, int? delay)
        {
            return lendingRate == null
                ? throw new ArgumentNullException(nameof(lendingRate))
                : decimal.Round(AddVat(CalculateNetRenewCost(estimatedValue, lendingRate, delay)));
        }
    }
}