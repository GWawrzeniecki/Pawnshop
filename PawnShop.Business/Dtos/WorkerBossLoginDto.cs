using PawnShop.Business.Models;

namespace PawnShop.Business.Dtos
{
    public class WorkerBossLoginDto
    {
        public WorkerBossLoginDto()
        {
        }

        public int WorkerBossId { get; set; }
        public int WorkerBossTypeId { get; set; }
        public int PrivilegeId { get; set; }
        public string Login { get; set; }
        public string Hash { get; set; }

        public virtual Privilege Privilege { get; set; }
        public virtual Person WorkerBossNavigation { get; set; }

    }
}