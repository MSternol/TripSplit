using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Infrastructure.Identity
{
    public sealed class AppUser : IdentityUser<Guid>
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        public string DisplayName =>
            string.Join(" ", new[] { FirstName, LastName }.Where(s => !string.IsNullOrWhiteSpace(s))).Trim();

        public override string ToString() => DisplayName.Length > 0 ? DisplayName : base.ToString() ?? UserName ?? Id.ToString();
    }
}
