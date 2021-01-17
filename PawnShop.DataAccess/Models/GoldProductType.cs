﻿using System;
using System.Collections.Generic;

#nullable disable

namespace PawnShop.EF.Models
{
    public partial class GoldProductType
    {
        public GoldProductType()
        {
            GoldProducts = new HashSet<GoldProduct>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<GoldProduct> GoldProducts { get; set; }
    }
}
