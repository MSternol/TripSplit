namespace TripSplit.Web.Models.Trips
{
    public sealed class TripListItemVm
    {
        public Guid Id { get; set; }
        public string StartName { get; set; } = string.Empty;
        public string EndName { get; set; } = string.Empty;
        public double DistanceKm { get; set; }
        public decimal TripCostTotal { get; set; }
        public decimal CostPerPerson { get; set; }
        public DateTime StartedAt { get; set; }
    }
}
