using TripSplit.Domain.Enums;

namespace TripSplit.Web.Models.Cars
{
    public sealed class CarListItemVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public FuelType FuelType { get; set; }
        public double AverageConsumptionLper100 { get; set; }
        public double TankCapacityL { get; set; }

        public int? InsuranceDaysLeft { get; set; }
        public int? InspectionDaysLeft { get; set; }
        public DateTime? InsuranceValidTo { get; set; }
        public DateTime? InspectionValidTo { get; set; }
    }
}
