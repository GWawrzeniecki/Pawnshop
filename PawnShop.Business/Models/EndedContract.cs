﻿#nullable disable

namespace PawnShop.Business.Models
{
    public partial class EndedContract
    {
        public string EndedContractId { get; set; }

        public virtual Contract EndedContractNavigation { get; set; }
    }
}
