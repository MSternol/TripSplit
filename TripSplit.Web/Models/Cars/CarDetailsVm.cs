using TripSplit.Domain.Enums;

namespace TripSplit.Web.Models.Cars
{
    public sealed class CarDetailsVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public FuelType FuelType { get; set; }
        public double AverageConsumptionLper100 { get; set; }
        public double TankCapacityL { get; set; }

        public double? EstimatedRangeKm =>
            AverageConsumptionLper100 > 0 && TankCapacityL > 0
                ? Math.Round((TankCapacityL / AverageConsumptionLper100) * 100.0, 1)
                : null;

        // OC
        public string? InsuranceCompany { get; set; }
        public string? InsurancePolicyNumber { get; set; }
        public DateTime? InsuranceValidFrom { get; set; }
        public DateTime? InsuranceValidTo { get; set; }

        // Badania
        public DateTime? InspectionValidFrom { get; set; }
        public DateTime? InspectionValidTo { get; set; }

        // Reminders
        public bool RemindersEnabled { get; set; } = true;
        public ReminderLeadTime ReminderLeadTime { get; set; } = ReminderLeadTime.Week;

        public int? InsuranceDaysLeft =>
            InsuranceValidTo.HasValue ? (int?)(InsuranceValidTo.Value.Date - DateTime.UtcNow.Date).Days : null;
        public int? InspectionDaysLeft =>
            InspectionValidTo.HasValue ? (int?)(InspectionValidTo.Value.Date - DateTime.UtcNow.Date).Days : null;

        public bool InsuranceExpired => InsuranceDaysLeft.HasValue && InsuranceDaysLeft.Value < 0;
        public bool InspectionExpired => InspectionDaysLeft.HasValue && InspectionDaysLeft.Value < 0;

        public bool InsuranceDueSoon =>
            InsuranceDaysLeft.HasValue && InsuranceDaysLeft.Value >= 0 && InsuranceDaysLeft.Value <= (int)ReminderLeadTime;
        public bool InspectionDueSoon =>
            InspectionDaysLeft.HasValue && InspectionDaysLeft.Value >= 0 && InspectionDaysLeft.Value <= (int)ReminderLeadTime;
    }
}
