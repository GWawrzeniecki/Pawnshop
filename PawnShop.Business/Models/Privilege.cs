using System.Collections.Generic;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class Privilege
    {
        public Privilege()
        {
            WorkerBosses = new HashSet<WorkerBoss>();
        }

        public int Id { get; set; }
        public bool PawnShopTabs { get; set; }
        public bool SettingsTab { get; set; }
        public bool WorkersTab { get; set; }

        public virtual ICollection<WorkerBoss> WorkerBosses { get; set; }
    }
}
