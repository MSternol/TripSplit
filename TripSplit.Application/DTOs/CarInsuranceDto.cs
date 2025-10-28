using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Application.DTOs
{
    public sealed class CarInsuranceDto
    {
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public string? Company { get; set; }
        public string? PolicyNumber { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
