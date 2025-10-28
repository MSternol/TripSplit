using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Common.Text;

namespace TripSplit.Tests.Application.Common
{
    public class TextNormalizerTests
    {
        [Theory]
        [InlineData("Żółć  ", "zołc")]
        [InlineData("  Łukasz", "łukasz")]
        [InlineData("KOWALSKI", "kowalski")]
        public void Norm_Removes_Diacritics_And_Lowers_Except_LStroke(string input, string expected)
            => TextNormalizer.Norm(input).Should().Be(expected);

        [Theory]
        [InlineData("Żaneta", "zaneta", true)]
        [InlineData("Kowalski", "KOWALSKI", true)]
        [InlineData("Nowak", "Nowicka", false)]
        public void EqualsLoose_Works(string a, string b, bool eq)
            => TextNormalizer.EqualsLoose(a, b).Should().Be(eq);
    }
}
