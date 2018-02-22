using System;
using FluentAssertions;
using Xunit;

namespace CheckoutSystem.Domain.PricingRules.Tests
{
    public class PriceDropOnMinimumPricingRuleTests
    {
        [Theory]
        [InlineData(2.0, 3, 1.0, 1, 2.0)]
        [InlineData(2.0, 3, 1.0, 2, 4.0)]
        [InlineData(2.0, 3, 1.0, 3, 3.0)]
        [InlineData(2.0, 3, 1.0, 4, 4.0)]
        [InlineData(2.0, 3, 1.0, 5, 5.0)]
        [InlineData(2.0, 0, 1.0, 2, 2.0)]
        [InlineData(2.0, 2, 0.0, 1, 2.0)]
        [InlineData(2.0, 2, 0.0, 2, 0.0)]
        public void CalculateTests(decimal price, int minimumCount, decimal priceAfter, int count, decimal total)
        {
            var pricingRule = new PriceDropOnMinimumPricingRule("productId", price, minimumCount, priceAfter);

            pricingRule.Calculate(count).Should().Be(total);
        }

        [Fact]
        public void Calculate_NegativeCount_Throws()
        {
            var pricingRule = new PriceDropOnMinimumPricingRule("productId", 2.0m, 3, 1.0m);

            pricingRule.Invoking(pr => pr.Calculate(-1)).Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(null, 2.0, 2, 1.0)]
        [InlineData("", 2.0, 2, 1.0)]
        [InlineData("productId", -2.0, 2, 1.0)]
        [InlineData("productId", 2.0, -2, 1.0)]
        [InlineData("productId", 2.0, 2, -1.0)]
        public void Constructor_InvalidParameters_Throws(string productId, decimal price, int minimumCount, decimal priceAfter)
        {
            Action act = () => new PriceDropOnMinimumPricingRule(productId, price, minimumCount, priceAfter);

            act.Should().Throw<ArgumentException>();
        }
    }
}