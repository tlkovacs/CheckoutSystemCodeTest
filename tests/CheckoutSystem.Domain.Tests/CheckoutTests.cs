using System;
using CheckoutSystem.Domain.PricingRules;
using FluentAssertions;
using Xunit;

namespace CheckoutSystem.Domain.Tests
{
    public class CheckoutTests
    {
        [Fact]
        public void NoItemsAndNoPricingRules_Returns_Zero()
        {
            var checkout = new Checkout();

            checkout.Total().Should().Be(0.0m);
        }

        [Fact]
        public void NoPricingRules_Throws()
        {
            var checkout = new Checkout();

            checkout.Add("productId");

            checkout.Invoking(pr => pr.Total()).Should()
                .Throw<InvalidOperationException>()
                .WithMessage("There are no pricing rules associated with 'productId'.");
        }

        [Fact]
        public void NoMatchingPricingRules_Throws()
        {
            var checkout = new Checkout(new StandardPricingRule("productId_A", 1.0m));

            checkout.Add("productId_B");

            checkout.Invoking(pr => pr.Total()).Should()
                .Throw<InvalidOperationException>()
                .WithMessage("There are no pricing rules associated with 'productId_B'.");
        }

        [Fact]
        public void NoItems_Returns_Zero()
        {
            var checkout = new Checkout(new StandardPricingRule("productId", 1.0m));

            checkout.Total().Should().Be(0.0m);
        }

        [Fact]
        public void LowestPricingRuleWins()
        {
            var checkout = new Checkout(new StandardPricingRule("productId", 1.0m), new StandardPricingRule("productId", 2.0m));

            checkout.Add("productId");
            checkout.Add("productId");

            checkout.Total().Should().Be(2.0m);
        }

        [Theory]
        [InlineData("default", new[] { "classic" }, 269.99)]
        [InlineData("default", new[] { "standout" }, 322.99)]
        [InlineData("default", new[] { "premium" }, 394.99)]
        [InlineData("default", new[] { "classic", "standout", "premium" }, 987.97)]
        [InlineData("Unilever", new[] { "classic", "classic", "classic", "premium" }, 934.97)]
        [InlineData("Apple", new[] { "standout", "standout", "standout", "premium" }, 1294.96)]
        [InlineData("Nike", new[] { "premium", "premium", "premium", "premium" }, 1519.96)]
        [InlineData("Ford", new[] { "premium", "premium", "premium" }, 1169.97)]
        [InlineData("Ford", new[] { "standout", "standout", "standout", "standout" }, 1239.96)]
        [InlineData("Ford", new[] { "classic", "classic", "classic", "classic", "classic" }, 1079.96)]
        public void ExampleScenarios(string customer, string[] products, decimal total)
        {
            var checkout = new Checkout(GetPricingRulesForCustomer(customer));

            foreach (var product in products)
            {
                checkout.Add(product);
            }

            checkout.Total().Should().Be(total);
        }

        private static IPricingRule[] GetPricingRulesForCustomer(string customer)
        {
            switch (customer)
            {
                case "Unilever":
                    return new IPricingRule[]
                    {
                        new XForYPricingRule("classic", 269.99m, 3, 2), // Gets a for 3 for 2 deal on Classic Ads
                        new StandardPricingRule("standout", 322.99m),
                        new StandardPricingRule("premium", 394.99m)
                    };
                case "Apple":
                    return new IPricingRule[]
                    {
                        new StandardPricingRule("classic", 269.99m),
                        new PriceDropOnMinimumPricingRule("standout", 322.99m, 0, 299.99m), // Gets a discount on Standout Ads where the price drops to $299.99 per ad
                        new StandardPricingRule("premium", 394.99m)
                    };
                case "Nike":
                    return new IPricingRule[]
                    {
                        new StandardPricingRule("classic", 269.99m),
                        new StandardPricingRule("standout", 322.99m),
                        new PriceDropOnMinimumPricingRule("premium", 394.99m, 4, 379.99m) // Gets a discount on Premium Ads when 4 or more are purchased. The price drops to $379.99 per ad
                    };
                case "Ford":
                    return new IPricingRule[]
                    {
                        new XForYPricingRule("classic", 269.99m, 5, 4), // Gets a 5 for 4 deal on Classic Ads
                        new PriceDropOnMinimumPricingRule("standout", 322.99m, 0, 309.99m), // Gets a discount on Standout Ads where the price drops to $309.99 per ad
                        new PriceDropOnMinimumPricingRule("premium", 394.99m, 3, 389.99m) // Gets a discount on Premium Ads when 3 or more are purchased. The price drops to $389.99 per ad
                    };
                default:
                    return new IPricingRule[]
                    {
                        new StandardPricingRule("classic", 269.99m),
                        new StandardPricingRule("standout", 322.99m),
                        new StandardPricingRule("premium", 394.99m)
                    };
            }
        }
    }
}