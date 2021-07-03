using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PawnShop.DataAccess.Data
{
    public static class PawnshopContextExtensions
    {
        public static bool IsChanged<T>(this PawnshopContext context) where T : class
        {
            return context
                .ChangeTracker
                .Entries<T>()
                .Any(e => e.State == EntityState.Added || e.State == EntityState.Modified ||
                          e.State == EntityState.Deleted);
        }
    }
}