namespace TripSplit.Web.Models.Shared
{
    public sealed class EmptyStateVm
    {
        public string Title { get; init; } = "Brak danych";
        public string? Subtitle { get; init; }
        public string PrimaryText { get; init; } = "Dodaj";
        public string PrimaryUrl { get; init; } = "#";
        public string? SecondaryText { get; init; }
        public string? SecondaryUrl { get; init; }
    }
}
