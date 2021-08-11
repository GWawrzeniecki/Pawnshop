using PawnShop.Business.Models;
using PawnShop.Core.Extensions;
using PawnShop.Core.SharedVariables;
using PawnShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PawnShop.Services.Implementations
{
    public class CalculateService : ICalculateService
    {
        #region PrivateMembers

        private readonly IConfigData _configData;

        #endregion

        #region Constructor

        public CalculateService(IConfigData configData)
        {
            _configData = configData;
        }

        #endregion

        #region PublicMethods

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

        public decimal CalculateNetRenewCost(decimal estimatedValue, LendingRate lendingRate, int? delay, IList<LendingRate> lendingRates)
        {
            return lendingRate == null
                ? throw new ArgumentNullException(nameof(lendingRate))
                : GetNetRenewCost(estimatedValue, lendingRate, delay, lendingRates);
        }

        public decimal CalculateRenewCost(decimal estimatedValue, LendingRate lendingRate, int? delay, IList<LendingRate> lendingRates)
        {
            return lendingRate == null
                ? throw new ArgumentNullException(nameof(lendingRate))
                : decimal.Round(AddVat(CalculateNetRenewCost(estimatedValue, lendingRate, delay, lendingRates)));
        }

        public decimal CalculateBuyBackCost(decimal estimatedValue, LendingRate lendingRate, int? delay, IList<LendingRate> lendingRates)
        {
            return lendingRate == null
                ? throw new ArgumentNullException(nameof(lendingRate))
                : decimal.Round(AddVat(CalculateNetRenewCost(estimatedValue, lendingRate, delay, lendingRates))) + estimatedValue;
        }

        #endregion

        #region PrivateMethods

        private decimal GetNetStorageCost(decimal estimatedValue, LendingRate lendingRate) => estimatedValue * lendingRate.Procent / 100;
        private decimal AddVat(decimal netAmount) => (netAmount * _configData.VatPercent / 100) + netAmount;

        private decimal GetNetRenewCost(decimal estimatedValue, LendingRate lendingRate, int? delay, IList<LendingRate> lendingRates)
        {
            if (delay is null or 0)
            {
                return GetNetStorageCost(estimatedValue, lendingRate);
            }

            var netStorageCost = GetNetStorageCost(estimatedValue, lendingRate);
            var delayedLendingRates = GetDelayedLendingRates(delay.Value, lendingRates);

            //var delayNetStorageCost = 0;
            foreach (var lR in delayedLendingRates)
            {
                netStorageCost += netStorageCost * lR.Procent / 100;
            }

            return netStorageCost;
        }

        private IEnumerable<LendingRate> GetDelayedLendingRates(int delay, IList<LendingRate> lendingRates)
        {
            if (lendingRates.Any(l => l.Days == delay))
                return new[] { lendingRates.First(l => l.Days == delay) };

            lendingRates = lendingRates
                .OrderByDescending(l => l.Days)
                .ToList();

            var lendingRatesCount = lendingRates.Count;
            var subtractedLendingRates = new List<LendingRate>();
            var highestDay = lendingRates.Max(l => l.Days);
            var lendingRatesIterator = 0;

            for (; lendingRatesIterator < lendingRatesCount && delay > highestDay; lendingRatesIterator++)
            {
                while (lendingRates[lendingRatesIterator].IsDelayBiggerOrEqual(delay))
                {
                    delay -= lendingRates[lendingRatesIterator].Days;
                    subtractedLendingRates.Add(lendingRates[lendingRatesIterator]);
                }
            }

            if (delay > 0)
                subtractedLendingRates.AddRange(lendingRates.Where(l => l.Days == lendingRates.Where(l2 => l2.Days >= delay)
                   .Min(l3 => l3.Days)));

            return subtractedLendingRates;
        }

        #endregion



    }
}