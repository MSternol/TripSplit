using System.ComponentModel.DataAnnotations;

namespace TripSplit.Web.Models.Tools
{
    public sealed class FullTankCostVm
    {
        [Range(0, 200), Display(Name = "Pojemność baku [l]")]
        public double TankCapacityL { get; set; }

        [Range(0, 1000), DataType(DataType.Currency), Display(Name = "Cena paliwa [PLN/l]")]
        public decimal FuelPricePerL { get; set; }

        [Display(Name = "Koszt pełnego baku [PLN]")]
        public decimal? Result { get; set; }
    }
}
