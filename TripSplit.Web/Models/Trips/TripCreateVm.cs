using System.ComponentModel.DataAnnotations;

namespace TripSplit.Web.Models.Trips
{
    public sealed class TripCreateVm
    {
        [Required, Display(Name = "Punkt startowy")]
        public string StartName { get; set; } = string.Empty;

        [Required, Display(Name = "Punkt końcowy")]
        public string EndName { get; set; } = string.Empty;

        [Display(Name = "Start: szer.")]
        public double? StartLat { get; set; }

        [Display(Name = "Start: dł.")]
        public double? StartLon { get; set; }

        [Display(Name = "Koniec: szer.")]
        public double? EndLat { get; set; }

        [Display(Name = "Koniec: dł.")]
        public double? EndLon { get; set; }

        [Range(0, 100000), Display(Name = "Dystans [km]")]
        public double? DistanceKm { get; set; }

        [Range(0, 1000), DataType(DataType.Currency), Display(Name = "Cena paliwa [PLN/l]")]
        public decimal FuelPricePerL { get; set; }

        [Range(0, 50), Display(Name = "Śr. spalanie [l/100km]")]
        public double AverageConsumptionLper100 { get; set; }

        [Range(0, 2000), Display(Name = "Zużycie [l] (opcjonalnie)")]
        public double? LitersUsed { get; set; }

        [Range(1, 20), Display(Name = "Liczba osób")]
        public int PeopleCount { get; set; } = 1;

        [Range(0, 100000), DataType(DataType.Currency), Display(Name = "Parking [PLN]")]
        public decimal ParkingCost { get; set; }

        [Range(0, 100000), DataType(DataType.Currency), Display(Name = "Dodatkowe koszty [PLN]")]
        public decimal ExtraCosts { get; set; }

        [Display(Name = "Samochód (opcjonalnie)")]
        public Guid? CarId { get; set; }

    }
}
