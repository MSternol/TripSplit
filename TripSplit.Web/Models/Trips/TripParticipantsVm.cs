using System.ComponentModel.DataAnnotations;

namespace TripSplit.Web.Models.Trips
{
    public sealed class TripParticipantsVm
    {
        public Guid TripId { get; set; }

        [Display(Name = "Kierowca – imię")]
        public string? DriverFirstName { get; set; }
        [Display(Name = "Kierowca – nazwisko")]
        public string? DriverLastName { get; set; }

        public string? P1FirstName { get; set; }
        public string? P1LastName { get; set; }

        public string? P2FirstName { get; set; }
        public string? P2LastName { get; set; }

        public string? P3FirstName { get; set; }
        public string? P3LastName { get; set; }

        public string? P4FirstName { get; set; }
        public string? P4LastName { get; set; }
    }
}
