using System;
using FluentAssertions;
using Xunit;

namespace CheckoutSystem.Domain.PricingRules.Tests
{
    public class StandardPricingRuleTests
    {
        [Theory]
        [InlineData(1.0, 1, 1.0)]
        [InlineData(1.0, 2, 2.0)]
        [InlineData(1.0, 100, 100.0)]
        [InlineData(0.0, 100, 0.0)]
        [InlineData(1.0, 0, 0.0)]
        public void CalculateTests(decimal price, int count, decimal total)
        {
            var pricingRule = new StandardPricingRule("productId", price);

            pricingRule.Calculate(count).Should().Be(total);
        }

        [Fact]
        public void Calculate_NegativeCount_Throws()
        {
            var pricingRule = new StandardPricingRule("productId", 1.0m);

            pricingRule.Invoking(pr => pr.Calculate(-1)).Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(null, 1.0)]
        [InlineData("", 1.0)]
        [InlineData("productId", -1.0)]
        public void Constructor_InvalidParameters_Throws(string productId, decimal price)
        {
            Action act = () => new StandardPricingRule(productId, price);

            act.Should().Throw<ArgumentException>();
        }
    }
}