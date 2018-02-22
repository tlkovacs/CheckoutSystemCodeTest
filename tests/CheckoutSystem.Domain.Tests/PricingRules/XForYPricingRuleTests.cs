using System;
using FluentAssertions;
using Xunit;

namespace CheckoutSystem.Domain.PricingRules.Tests
{
    public class XForYPricingRuleTests
    {
        [Theory]
        [InlineData(1.0, 3, 2, 1, 1.0)]
        [InlineData(1.0, 3, 2, 2, 2.0)]
        [InlineData(1.0, 3, 2, 3, 2.0)]
        [InlineData(1.0, 3, 2, 4, 3.0)]
        [InlineData(1.0, 3, 2, 5, 4.0)]
        [InlineData(1.0, 3, 2, 6, 4.0)]
        [InlineData(1.0, 3, 2, 7, 5.0)]
        public void CalculateTests(decimal price, int x, int y, int count, decimal total)
        {
            var pricingRule = new XForYPricingRule("productId", price, x, y);

            pricingRule.Calculate(count).Should().Be(total);
        }

        [Fact]
        public void Calculate_NegativeCount_Throws()
        {
            var pricingRule = new XForYPricingRule("productId", 1.0m, 3, 2);

            pricingRule.Invoking(pr => pr.Calculate(-1)).Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(null, 1.0, 3, 2)]
        [InlineData("", 1.0, 3, 2)]
        [InlineData("productId", -1.0, 3, 2)]
        [InlineData("productId", 1.0, -3, 2)]
        [InlineData("productId", 1.0, 3, -2)]
        public void Constructor_InvalidParameters_Throws(string productId, decimal price, int x, int y)
        {
            Action act = () => new XForYPricingRule(productId, price, x, y);

            act.Should().Throw<ArgumentException>();
        }
    }
}