using PawnShop.Core.Enums;
using System;

namespace PawnShop.Core.Models.DropDownButtonModels
{
    public class DateSearchOption
    {
        public string Name { get; set; }
        public SearchOptions SearchOption { get; set; }
        public Action<DateTime?, DateTime?> FillPropertiesAction { get; set; }
    }
}