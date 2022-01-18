using System;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class Person
    {
        public int PersonId { get; set; }
        public int AddressId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public virtual Address Address { get; set; }
        public virtual Client Client { get; set; }
        public virtual WorkerBoss WorkerBoss { get; set; }

    }
}
