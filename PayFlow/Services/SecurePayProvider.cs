using PayFlow.Models;
using PayFlow.Mocks;

namespace PayFlow.Services
{
    public class SecurePayProvider : IPaymentProvider
    {
        public string Name => "SecurePay";

        public async Task<(bool success, string externalId, string status)> PayAsync(decimal amount, string currency)
        {
            var payload = new
            {
                amount_cents = (int)decimal.Round(amount * 100, 0),
                currency_code = currency,
                client_reference = $"ORD-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}"
            };

            var resp = await ProviderMocks.CallSecurePayAsync(payload);

            if (resp is null) return (false, string.Empty, "failed");

            return (resp.result == "success", resp.transaction_id, resp.result);
        }
    }
}
