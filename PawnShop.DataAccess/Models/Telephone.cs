using System;
using System.Collections.Generic;

#nullable disable

namespace PawnShop.EF.Models
{
    public partial class Telephone
    {
        public int ContractitemId { get; set; }
        public string Brand { get; set; }
        public string Procesor { get; set; }
        public string Ram { get; set; }
        public string MassStorage { get; set; }
        public decimal ScreenSize { get; set; }
        public string DescriptionKit { get; set; }

        public virtual ContractItem Contractitem { get; set; }
    }
}
