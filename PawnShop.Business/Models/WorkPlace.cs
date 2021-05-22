using System;
using System.Collections.Generic;
using PawnShop.Business.Models;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class WorkPlace
    {
        public WorkPlace()
        {
            PersonWorkplaces = new HashSet<PersonWorkplace>();
        }

        public int WorkplaceId { get; set; }
        public int AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual ICollection<PersonWorkplace> PersonWorkplaces { get; set; }
    }
}
