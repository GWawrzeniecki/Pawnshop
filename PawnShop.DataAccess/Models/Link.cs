﻿using System;
using System.Collections.Generic;

#nullable disable

namespace PawnShop.EF.Models
{
    public partial class Link
    {
        public int LinkId { get; set; }
        public string Link1 { get; set; }
        public int SaleId { get; set; }

        public virtual Sale Sale { get; set; }
    }
}
