﻿using System;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class WorkerBossContractItem
    {
        public int WorkerBossId { get; set; }
        public int ContractItemId { get; set; }
        public DateTime DateOfIssue { get; set; }
        public decimal ProposedPrice { get; set; }

        public virtual ContractItem ContractItem { get; set; }
        public virtual WorkerBoss WorkerBoss { get; set; }
    }
}
