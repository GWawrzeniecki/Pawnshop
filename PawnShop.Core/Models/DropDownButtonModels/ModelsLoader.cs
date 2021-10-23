using PawnShop.Core.Enums;
using System.Collections.Generic;

namespace PawnShop.Core.Models.DropDownButtonModels
{
    public static class ModelsLoader
    {
        public static List<DateSearchOption> LoadDateSearchOptions()
        {
            return new List<DateSearchOption>
            {
                new() {Name = "Wyczyść", SearchOption = SearchOptions.Clean},
                new() {Name = "Dzisiaj", SearchOption = SearchOptions.Today},
                new() {Name = "Wczoraj", SearchOption = SearchOptions.Yesterday},
                new() {Name = "Bieżący tydzien", SearchOption = SearchOptions.CurrentWeek},
                new() {Name = "Poprzedni tydzien", SearchOption = SearchOptions.PastWeek},
                new() {Name = "Bieżący miesiąc", SearchOption = SearchOptions.CurrentMonth},
                new() {Name = "Poprzedni miesiąc", SearchOption = SearchOptions.PastMonth},
                new() {Name = "Bieżący kwartał", SearchOption = SearchOptions.CurrentQuarter},
                new() {Name = "Poprzedni kwartał", SearchOption = SearchOptions.PastQuarter},
                new() {Name = "Bieżący rok", SearchOption = SearchOptions.CurrentYear},
                new() {Name = "Poprzedni rok", SearchOption = SearchOptions.PastYear}
            };
        }

        public static List<SearchPriceOption> LoadPriceOptions()
        {
            return new List<SearchPriceOption>
            {
                new() { Option = "Rowne", PriceOption = PriceOption.Equal },
                new() { Option = "Mniejsze", PriceOption = PriceOption.Lower },
                new() { Option = "Wieksze", PriceOption = PriceOption.Higher },

            };
        }
        public static List<RefreshButtonOption> LoadRefreshButtonOptions()
        {
            return new List<RefreshButtonOption>
            {
                new() {Name = "Wyczyść filtr", RefreshOption = RefreshOptions.Clean},
                new() {Name = "Wyczyść filtr i odśwież", RefreshOption = RefreshOptions.CleanAndRefresh}
            };
        }
    }
}