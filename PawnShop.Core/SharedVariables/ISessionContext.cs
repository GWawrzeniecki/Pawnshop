using PawnShop.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PawnShop.Core.SharedVariables
{
    public interface ISessionContext
    {
        public Person LoggedPerson { get; set; }
        public MoneyBalance TodayMoneyBalance { get; set; }
    }
}
