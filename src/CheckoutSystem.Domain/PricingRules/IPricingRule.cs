namespace CheckoutSystem.Domain.PricingRules
{
    public interface IPricingRule
    {
        string ProductId { get; }
        decimal Calculate(int count);
    }
}