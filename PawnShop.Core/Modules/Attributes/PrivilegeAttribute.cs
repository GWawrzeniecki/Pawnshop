using System;

namespace PawnShop.Core.Attributes
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