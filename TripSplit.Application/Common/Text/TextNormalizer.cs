using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Application.Common.Text
{
    public static class TextNormalizer
    {
        public static string Norm(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;

            var raw = s.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(raw.Length);
            foreach (var ch in raw)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public static bool EqualsLoose(string? a, string? b)
            => Norm(a) == Norm(b);
    }
}
