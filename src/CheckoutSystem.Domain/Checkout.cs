using System;
using System.Collections.Generic;
using System.Linq;
using CheckoutSystem.Domain.PricingRules;

namespace CheckoutSystem.Domain
{
    public class Checkout
    {
        private readonly IPricingRule[] pricingRules;
        private readonly List<string> productIds = new List<string>();

        public Checkout(params IPricingRule[] pricingRules)
        {
            this.pricingRules = pricingRules;
        }
        
        public void Add(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                throw new ArgumentNullException(nameof(productId));
            }
            this.productIds.Add(productId);
        }

        public decimal Total()
        {
            var total = 0.0m;

            // get each productId and count how many items there are
            foreach (var g in this.productIds.GroupBy(i => i).Select(g => new { ProductId = g.Key, Count = g.Count() }))
            {
                // Verify there's at least one pricing rule for the productId
                if (pricingRules.All(x => x.ProductId != g.ProductId))
                {
                    throw new InvalidOperationException($"There are no pricing rules associated with '{g.ProductId}'.");
                }

                // Calculate subtotals from each pricing rule and get the cheapest
                total += this.pricingRules
                    .Where(pr => pr.ProductId == g.ProductId)
                    .Select(pr => pr.Calculate(g.Count))
                    .OrderBy(t => t)
                    .FirstOrDefault();
            }

            return total;
        }
    }
}