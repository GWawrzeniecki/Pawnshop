using System;
using System.Collections.Generic;
using System.Text;

namespace PawnShop.Core.Taskbar
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class UserControlAttribute : Attribute
    {
        public Type Type { get; private set; }
        public UserControlAttribute(Type userControlType)
        {
            Type = userControlType;
        }
    }
}
