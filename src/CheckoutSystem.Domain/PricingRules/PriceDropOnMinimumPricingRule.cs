using System;

namespace CheckoutSystem.Domain.PricingRules
{
    public class PriceDropOnMinimumPricingRule : IPricingRule
    {
        public string ProductId { get; }
        private readonly decimal price;
        private readonly int minimumCount;
        private readonly decimal priceAfter;

        public PriceDropOnMinimumPricingRule(string productId, decimal price, int minimumCount, decimal priceAfter)
        {
            if (string.IsNullOrEmpty(productId))
            {
                throw new ArgumentNullException(nameof(productId));
            }
            this.ProductId = productId;
            if (price < 0.0m)
            {
                throw new ArgumentException($"{nameof(price)} must be a positive number.");
            }
            this.price = price;
            if (minimumCount < 0)
            {
                throw new ArgumentException($"{nameof(minimumCount)} must be a positive number.");
            }
            this.minimumCount = minimumCount;
            if (priceAfter < 0.0m)
            {
                throw new ArgumentException($"{nameof(priceAfter)} must be a positive number.");
            }
            this.priceAfter = priceAfter;
        }

        /// <summary>
        /// price drops to priceAfter if the count is greater than or equal to minimumCount
        /// </summary>
        public decimal Calculate(int count)
        {
            if (count < 0)
            {
                throw new ArgumentException($"{nameof(count)} must be a positive number.");
            }
            if (count >= minimumCount)
            {
                return count * priceAfter;
            }
            return count * price;
        }
    }
}