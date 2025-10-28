namespace TripSplit.Web.Models.Trips
{
    public sealed class TripDetailsVm
    {
        public Guid Id { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public string StartName { get; set; } = string.Empty;
        public string EndName { get; set; } = string.Empty;

        public double DistanceKm { get; set; }
        public decimal FuelPricePerL { get; set; }
        public double AverageConsumptionLper100 { get; set; }
        public double LitersUsed { get; set; }
        public int PeopleCount { get; set; }

        public decimal ParkingCost { get; set; }
        public decimal ExtraCosts { get; set; }

        public decimal FuelCostTotal { get; set; }
        public decimal TripCostTotal { get; set; }
        public decimal CostPerPerson { get; set; }

        public string? CarName { get; set; }


    }
}
