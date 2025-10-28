using System.ComponentModel.DataAnnotations;

namespace TripSplit.Web.Models.Cars
{
    public sealed class CarInsuranceVm
    {
        [Display(Name = "Ubezpieczyciel (OC)")]
        public string? InsuranceCompany { get; set; }

        [Display(Name = "Numer polisy OC")]
        public string? InsurancePolicyNumber { get; set; }

        [Display(Name = "OC: od")]
        [DataType(DataType.Date)]
        public DateTime? InsuranceValidFrom { get; set; }

        [Display(Name = "OC: do")]
        [DataType(DataType.Date)]
        public DateTime? InsuranceValidTo { get; set; }
    }
}
