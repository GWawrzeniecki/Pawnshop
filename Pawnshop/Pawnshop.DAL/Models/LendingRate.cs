﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Pawnshop.DAL.Models
{
    public partial class LendingRate
    {
        public LendingRate()
        {
            Contracts = new HashSet<Contract>();
        }

        public int Id { get; set; }
        public int Procent { get; set; }
        public int Days { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; }
    }
}
