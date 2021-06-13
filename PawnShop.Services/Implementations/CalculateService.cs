using System;
using PawnShop.Business.Models;
using PawnShop.Core.SharedVariables;
using PawnShop.Services.Interfaces;

namespace PawnShop.Services.Implementations
{
    public class CalculateService : ICalculateService
    {
        private readonly IConfigData _configData;

        public CalculateService(IConfigData configData)
        {
            _configData = configData;
        }



        private static decimal GetNetStorageCost(decimal estimatedValue, int percent) => estimatedValue * percent / 100;

        private decimal GetVatStorageCost(decimal estimatedValue, int percent)
        {
            var netCost = GetNetStorageCost(estimatedValue, percent);
            var vat = netCost * _configData.VatPercent / 100;
            return netCost + vat;
        }

        public decimal CalculateContractAmount(decimal estimatedValue, LendingRate lendingRate)
        {
            if (lendingRate == null) throw new ArgumentNullException(nameof(lendingRate));
            return estimatedValue + decimal.Round(
                GetVatStorageCost(estimatedValue, lendingRate.Procent));
        }

        public decimal CalculateNetStorageCost(decimal estimatedValue, LendingRate lendingRate)
        {
            if (lendingRate == null) throw new ArgumentNullException(nameof(lendingRate));
            return GetNetStorageCost(estimatedValue, lendingRate.Procent);
        }
    }
}