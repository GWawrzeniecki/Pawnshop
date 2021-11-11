using PawnShop.Core.Enums;
using PawnShop.Core.Extensions;
using System;
using System.Collections.Generic;

namespace PawnShop.Core.Models.DropDownButtonModels
{
    public static class ModelsLoader
    {
        public static List<DateSearchOption> LoadDateSearchOptions(Action<DateTime?, DateTime?> fillPropertiesAction)
        {
            return new List<DateSearchOption>
            {
                new() {Name = "Wyczyść", SearchOption = SearchOptions.Clean, FillPropertiesAction = fillPropertiesAction},
                new() {Name = "Dzisiaj", SearchOption = SearchOptions.Today, FillPropertiesAction = fillPropertiesAction},
                new() {Name = "Wczoraj", SearchOption = SearchOptions.Yesterday, FillPropertiesAction = fillPropertiesAction},
                new() {Name = "Bieżący tydzien", SearchOption = SearchOptions.CurrentWeek, FillPropertiesAction = fillPropertiesAction},
                new() {Name = "Poprzedni tydzien", SearchOption = SearchOptions.PastWeek, FillPropertiesAction = fillPropertiesAction},
                new() {Name = "Bieżący miesiąc", SearchOption = SearchOptions.CurrentMonth, FillPropertiesAction = fillPropertiesAction},
                new() {Name = "Poprzedni miesiąc", SearchOption = SearchOptions.PastMonth, FillPropertiesAction = fillPropertiesAction},
                new() {Name = "Bieżący kwartał", SearchOption = SearchOptions.CurrentQuarter, FillPropertiesAction = fillPropertiesAction},
                new() {Name = "Poprzedni kwartał", SearchOption = SearchOptions.PastQuarter, FillPropertiesAction = fillPropertiesAction},
                new() {Name = "Bieżący rok", SearchOption = SearchOptions.CurrentYear, FillPropertiesAction = fillPropertiesAction},
                new() {Name = "Poprzedni rok", SearchOption = SearchOptions.PastYear, FillPropertiesAction = fillPropertiesAction}
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

        public static void SetSearchOption(DateSearchOption dateSearchOption)
        {
            switch (dateSearchOption.SearchOption)
            {
                case SearchOptions.Clean:
                    dateSearchOption.FillPropertiesAction(null, null);
                    break;
                case SearchOptions.Today:
                    dateSearchOption.FillPropertiesAction(DateTime.Today, DateTime.Today);
                    break;
                case SearchOptions.Yesterday:
                    dateSearchOption.FillPropertiesAction(DateTime.Today.Yesterday(), DateTime.Today.Yesterday());
                    break;
                case SearchOptions.CurrentWeek:
                    dateSearchOption.FillPropertiesAction(DateTime.Today.Monday(), DateTime.Today.Sunday());
                    break;
                case SearchOptions.PastWeek:
                    dateSearchOption.FillPropertiesAction(DateTime.Today.PastMonday(), DateTime.Today.PastSunday());
                    break;
                case SearchOptions.CurrentMonth:
                    dateSearchOption.FillPropertiesAction(DateTime.Today.BeginningOfCurrentMonth(), DateTime.Today.EndOfCurrentMonth());
                    break;
                case SearchOptions.PastMonth:
                    dateSearchOption.FillPropertiesAction(DateTime.Today.BeginningOfPastMonth(), DateTime.Today.EndOfPastMonth());
                    break;
                case SearchOptions.CurrentQuarter:
                    dateSearchOption.FillPropertiesAction(DateTime.Today.BeginningOfCurrentQuarter(), DateTime.Today.EndOfCurrentQuarter());
                    break;
                case SearchOptions.PastQuarter:
                    dateSearchOption.FillPropertiesAction(DateTime.Today.BeginningOfPastQuarter(), DateTime.Today.EndOfPastQuarter());
                    break;
                case SearchOptions.CurrentYear:
                    dateSearchOption.FillPropertiesAction(DateTime.Today.BeginningOfCurrentYear(), DateTime.Today.EndOfCurrentYear());
                    break;
                case SearchOptions.PastYear:
                    dateSearchOption.FillPropertiesAction(DateTime.Today.BeginningOfPastYear(), DateTime.Today.EndOfPastYear());
                    break;
                default:
                    throw new ArgumentException(nameof(dateSearchOption));
            }
        }
    }
}