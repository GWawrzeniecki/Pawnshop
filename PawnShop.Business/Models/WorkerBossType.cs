using System.Collections.Generic;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class WorkerBossType
    {
        public WorkerBossType()
        {
            WorkerBosses = new HashSet<WorkerBoss>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<WorkerBoss> WorkerBosses { get; set; }
    }
}
