using System;
using System.Collections.Generic;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class WorkerBoss
    {
        public WorkerBoss()
        {
            Contracts = new HashSet<Contract>();
        }

        public int WorkerBossId { get; set; }
        public int WorkerBossTypeId { get; set; }
        public int PrivilegeId { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? DatePhysicalCheckUp { get; set; }
        public int? Salary { get; set; }
        public int? GrantedBonus { get; set; }
        public string Pesel { get; set; }
        public string Login { get; set; }
        public string Hash { get; set; }

        public virtual Privilege Privilege { get; set; }
        public virtual Person WorkerBossNavigation { get; set; }
        public virtual WorkerBossType WorkerBossType { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
    }
}
