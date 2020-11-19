using System;
using System.Collections.Generic;

#nullable disable

namespace Pawnshop.DAL.Models
{
    public partial class EndedContract
    {
        public string EndedContractId { get; set; }

        public virtual Contract EndedContractNavigation { get; set; }
    }
}
