using System.ComponentModel.DataAnnotations;

namespace TripSplit.Web.Models.Cars
{
    public sealed class CarInspectionVm
    {
        [Display(Name = "Badanie: od")]
        [DataType(DataType.Date)]
        public DateTime? InspectionValidFrom { get; set; }

        [Display(Name = "Badanie: do")]
        [DataType(DataType.Date)]
        public DateTime? InspectionValidTo { get; set; }
    }
}
