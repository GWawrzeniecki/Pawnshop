﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Pawnshop.DAL.Models
{
    public partial class Accountant
    {
        public int AccountantId { get; set; }

        public virtual Person AccountantNavigation { get; set; }
    }
}
