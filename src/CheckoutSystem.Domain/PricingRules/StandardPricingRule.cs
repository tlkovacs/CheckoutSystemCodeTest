using System;

namespace CheckoutSystem.Domain.PricingRules
{
    public class StandardPricingRule : IPricingRule
    {
        public string ProductId { get; }
        private readonly decimal price;

        public StandardPricingRule(string productId, decimal price)
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
        }

        public decimal Calculate(int count)
        {
            if (count < 0)
            {
                throw new ArgumentException($"{nameof(count)} must be a positive number.");
            }
            return count * this.price;
        }
    }
}