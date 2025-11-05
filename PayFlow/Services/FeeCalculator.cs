namespace PayFlow.Services
{
    public static class FeeCalculator
    {
        public static decimal CalculateFastPayFee(decimal amount) =>
            decimal.Round(amount * 0.0349m, 2, MidpointRounding.AwayFromZero);

        public static decimal CalculateSecurePayFee(decimal amount) =>
            decimal.Round(amount * 0.0299m + 0.40m, 2, MidpointRounding.AwayFromZero);
    }
}
