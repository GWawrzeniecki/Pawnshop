using System;

namespace PawnShop.Services.DataService.Dtos
{
    public class SaleChartDto
    {
        public decimal SoldPriceSum { get; set; }
        public DateTime SaleDate { get; set; }
    }
}