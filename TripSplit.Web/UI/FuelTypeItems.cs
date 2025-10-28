using Microsoft.AspNetCore.Mvc.Rendering;
using TripSplit.Domain.Enums;

namespace TripSplit.Web.UI
{
    public static class FuelTypeItems
    {
        private static readonly IReadOnlyDictionary<FuelType, string> _labels = new Dictionary<FuelType, string>
        {
            [FuelType.Unknown] = "Nieznane",
            [FuelType.Petrol] = "Benzyna",
            [FuelType.Diesel] = "Diesel",
            [FuelType.LPG] = "LPG",
            [FuelType.CNG] = "CNG",
            [FuelType.Electric] = "Elektryczny"
        };

        public static IEnumerable<SelectListItem> All(FuelType? selected = null) =>
            _labels.Select(kv => new SelectListItem(kv.Value, ((int)kv.Key).ToString(), selected == kv.Key));

        public static string Label(FuelType fuel) => _labels.TryGetValue(fuel, out var l) ? l : fuel.ToString();
    }
}
