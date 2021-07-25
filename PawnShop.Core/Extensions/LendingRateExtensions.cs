using PawnShop.Business.Models;

namespace PawnShop.Core.Extensions
{
    public static class LendingRateExtensions
    {
        public static bool IsDelaySmallerOrEqual(this LendingRate lendingRate, int delay) => delay <= lendingRate.Days;
        public static bool IsDelayBiggerOrEqual(this LendingRate lendingRate, int delay) => delay >= lendingRate.Days;

    }
}