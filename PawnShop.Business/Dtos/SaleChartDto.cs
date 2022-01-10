using System;

namespace PawnShop.Business.Dtos
{
    public class SaleChartDto
    {
        public decimal SoldPriceSum { get; set; }
        public DateTime SaleDate { get; set; }
    }
}