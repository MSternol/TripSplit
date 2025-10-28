using System.ComponentModel.DataAnnotations;

namespace TripSplit.Web.Models.Tools
{
    public sealed class AverageConsumptionVm
    {
        [Range(0.1, 100000), Display(Name = "Dystans [km]")]
        public double DistanceKm { get; set; }

        [Range(0, 2000), Display(Name = "Zużycie [l]")]
        public double LitersUsed { get; set; }

        [Display(Name = "Śr. spalanie [l/100km]")]
        public double? Result { get; set; }
    }
}
