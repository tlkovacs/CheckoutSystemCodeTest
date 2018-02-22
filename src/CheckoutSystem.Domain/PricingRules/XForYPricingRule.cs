using System;

namespace CheckoutSystem.Domain.PricingRules
{
    public class XForYPricingRule : IPricingRule
    {
        public string ProductId { get; }
        private readonly decimal price;
        private readonly int x;
        private readonly int y;

        public XForYPricingRule(string productId, decimal price, int x, int y)
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
            if (x < 0)
            {
                throw new ArgumentException($"{nameof(x)} must be a positive number.");
            }
            this.x = x;
            if (y < 0)
            {
                throw new ArgumentException($"{nameof(y)} must be a positive number.");
            }
            this.y = y;
        }

        /// <summary>
        /// Get x for the price of y.
        /// The remainder is paid in full.
        /// </summary>
        public decimal Calculate(int count)
        {
            if (count < 0)
            {
                throw new ArgumentException($"{nameof(count)} must be a positive number.");
            }
            return ((count - count % x) / x * y + (count % x)) * price;
        }
    }
}