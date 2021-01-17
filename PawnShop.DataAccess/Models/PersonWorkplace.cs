using System;
using System.Collections.Generic;

#nullable disable

namespace PawnShop.EF.Models
{
    public partial class PersonWorkplace
    {
        public int WorkplaceId { get; set; }
        public int PersonId { get; set; }

        public virtual Person Person { get; set; }
        public virtual WorkPlace Workplace { get; set; }
    }
}
