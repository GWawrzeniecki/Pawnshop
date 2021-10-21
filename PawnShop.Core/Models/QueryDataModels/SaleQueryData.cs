using System;
using Microsoft.VisualBasic;
using PawnShop.Business.Models;
using PawnShop.Core.Models.DropDownButtonModels;

namespace PawnShop.Core.Models.QueryDataModels
{
    public class SaleQueryData
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string ItemName { get; set; }
        public string Client { get; set; }
        public ContractItemCategory ContractItemCategory { get; set; }
        public string SalePrice { get; set; }
        public string ContractNumber { get; set; }
        public SearchPriceOption PriceOption { get; set; }

    }
}