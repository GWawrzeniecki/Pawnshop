using System;

namespace PawnShop.Core.Privileges
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PrivilegeAttribute : Attribute
    {
        public PrivilegeAttribute(string privilege)
        {
            Privilege = privilege;
        }

        public string Privilege { get; set; }
    }
}