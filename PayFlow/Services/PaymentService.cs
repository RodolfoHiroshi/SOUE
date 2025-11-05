using PayFlow.Models;

namespace PayFlow.Services
{
    public class ProvidersUnavailableException : Exception
    {
        public ProvidersUnavailableException(string message) : base(message) { }
    }

    public class PaymentService
    {
        private readonly FastPayProvider _fast;
        private readonly SecurePayProvider _secure;
        private static int _idCounter = 1;

        public PaymentService(FastPayProvider fast, SecurePayProvider secure)
        {
            _fast = fast;
            _secure = secure;
        }

        private (IPaymentProvider primary, IPaymentProvider secondary) ChooseProviders(decimal amount)
        {
            if (amount < 100m) return (_fast, _secure);
            return (_secure, _fast);
        }

        public async Task<PaymentResponse> ProcessPaymentAsync(decimal amount, string currency)
        {
            var (primary, secondary) = ChooseProviders(amount);

            var tried = new List<(IPaymentProvider provider, (bool success, string externalId, string status) result)>();

            //Tentando serviço Primário
            var primaryResult = await SafePayAsync(primary, amount, currency);
            tried.Add((primary, primaryResult));
            if (primaryResult.success)
                return BuildResponse(primaryResult, primary.Name, amount);

            //Tentando serviço Secundário
            var secondaryResult = await SafePayAsync(secondary, amount, currency);
            tried.Add((secondary, secondaryResult));
            if (secondaryResult.success)
                return BuildResponse(secondaryResult, secondary.Name, amount);

            throw new ProvidersUnavailableException("Ambos os fornecedores indisponíveis, tente novamente mais tarde.");
        }

        private async Task<(bool success, string externalId, string status)> SafePayAsync(IPaymentProvider provider, decimal amount, string currency)
        {
            try
            {
                // Timeout, exceptions ou erros dos provedores serão detectados.
                return await provider.PayAsync(amount, currency);
            }
            catch
            {
                return (false, string.Empty, "failed");
            }
        }

        private PaymentResponse BuildResponse((bool success, string externalId, string status) providerResp, string providerName, decimal grossAmount)
        {
            decimal fee = providerName == "FastPay"
                ? FeeCalculator.CalculateFastPayFee(grossAmount)
                : FeeCalculator.CalculateSecurePayFee(grossAmount);

            decimal net = decimal.Round(grossAmount - fee, 2, MidpointRounding.AwayFromZero);

            return new PaymentResponse
            {
                Id = System.Threading.Interlocked.Increment(ref _idCounter),
                ExternalId = providerResp.externalId ?? string.Empty,
                Status = providerResp.success ? "approved" : "failed",
                Provider = providerName,
                GrossAmount = grossAmount,
                Fee = fee,
                NetAmount = net
            };
        }
    }
}
