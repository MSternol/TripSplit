using System;
using TripSplit.Domain.Enums;

namespace TripSplit.Application.DTOs
{
    public sealed class CarDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public FuelType FuelType { get; set; }
        public double AverageConsumptionLper100 { get; set; }
        public double TankCapacityL { get; set; }

        public string? InsuranceCompany { get; set; }
        public string? InsurancePolicyNumber { get; set; }
        public DateTime? InsuranceValidFrom { get; set; }
        public DateTime? InsuranceValidTo { get; set; }

        public DateTime? InspectionValidFrom { get; set; }
        public DateTime? InspectionValidTo { get; set; }

        public bool RemindersEnabled { get; set; }
        public ReminderLeadTime ReminderLeadTime { get; set; }

        public int? InsuranceDaysLeft { get; set; }
        public int? InspectionDaysLeft { get; set; }
        public bool InsuranceExpired { get; set; }
        public bool InspectionExpired { get; set; }
    }
}
