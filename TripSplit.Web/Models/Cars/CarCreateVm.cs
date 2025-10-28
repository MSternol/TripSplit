using System.ComponentModel.DataAnnotations;
using TripSplit.Domain.Enums;

namespace TripSplit.Web.Models.Cars
{
    public sealed class CarCreateVm
    {
        [Required, MaxLength(200), Display(Name = "Nazwa auta")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Rodzaj paliwa")]
        public FuelType FuelType { get; set; } = FuelType.Petrol;

        [Range(0, 50), Display(Name = "Śr. spalanie [l/100km]")]
        public double AverageConsumptionLper100 { get; set; }

        [Range(0, 200), Display(Name = "Pojemność baku [l]")]
        public double TankCapacityL { get; set; }

        [Display(Name = "Dodaj dane OC")]
        public bool IncludeInsurance { get; set; } = false;

        [Display(Name = "Dodaj badanie techniczne")]
        public bool IncludeInspection { get; set; } = false;

        public CarInsuranceVm? Insurance { get; set; }
        public CarInspectionVm? Inspection { get; set; }

    }
}
