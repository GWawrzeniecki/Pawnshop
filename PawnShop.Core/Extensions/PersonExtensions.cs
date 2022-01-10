using PawnShop.Business.Models;

namespace PawnShop.Core.Extensions
{
    public static class PersonExtensions
    {
        public static string GetFullName(this Person person)
        {
            return $"{person.FirstName} {person.LastName}";
        }
    }
}