using PawnShop.Business.Models;
using System;

namespace PawnShop.Core.Models.QueryDataModels
{
    public class ContractQueryData
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string ContractNumber { get; set; }
        public string Client { get; set; }
        public ContractState ContractState { get; set; }
        public string ContractAmount { get; set; }
        public LendingRate LendingRate { get; set; }
    }
}