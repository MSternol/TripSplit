using System.ComponentModel.DataAnnotations;

namespace TripSplit.Web.Models.Trips
{
    public sealed class TripEditVm
    {
        public Guid Id { get; set; }

        // pole podglądowe (nagłówek)
        public string StartName { get; set; } = "";
        public string EndName { get; set; } = "";

        [Display(Name = "Cena paliwa (zł/l)")]
        [Range(0, 1000)]
        public decimal FuelPricePerL { get; set; }

        [Display(Name = "Śr. spalanie (l/100 km)")]
        [Range(0, 100)]
        public double AverageConsumptionLper100 { get; set; }

        [Display(Name = "Zużycie (l)")]
        [Range(0, 10000)]
        public double LitersUsed { get; set; }

        [Display(Name = "Dystans (km)")]
        [Range(0, 100000)]
        public double DistanceKm { get; set; }

        [Display(Name = "Parking (zł)")]
        [Range(0, 100000)]
        public decimal ParkingCost { get; set; }

        [Display(Name = "Dodatkowe (zł)")]
        [Range(0, 100000)]
        public decimal ExtraCosts { get; set; }

        [Display(Name = "Liczba osób")]
        [Range(1, 50)]
        public int PeopleCount { get; set; }
    }
}
