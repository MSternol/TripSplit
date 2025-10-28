using System.ComponentModel.DataAnnotations;
using TripSplit.Domain.Enums;

namespace TripSplit.Web.Models.Cars
{
    public sealed class CarEditVm
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Nazwa auta jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa może mieć maksymalnie 100 znaków.")]
        [Display(Name = "Nazwa pojazdu")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Wybierz rodzaj paliwa.")]
        [Display(Name = "Rodzaj paliwa")]
        public FuelType FuelType { get; set; }

        [Range(0, 100, ErrorMessage = "Podaj wartość od 0 do 100.")]
        [Display(Name = "Średnie spalanie (l/100 km)")]
        public double AverageConsumptionLper100 { get; set; }

        [Range(0, 200, ErrorMessage = "Podaj wartość od 0 do 200.")]
        [Display(Name = "Pojemność baku (l)")]
        public double TankCapacityL { get; set; }

        [Display(Name = "Edytuj dane OC")]
        public bool IncludeInsurance { get; set; } = false;

        [Display(Name = "Edytuj badanie techniczne")]
        public bool IncludeInspection { get; set; } = false;

        public CarInsuranceVm? Insurance { get; set; }
        public CarInspectionVm? Inspection { get; set; }

    }
}
