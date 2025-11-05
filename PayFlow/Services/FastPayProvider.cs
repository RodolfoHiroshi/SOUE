using PayFlow.Models;
using PayFlow.Mocks;

namespace PayFlow.Services
{
    public class FastPayProvider : IPaymentProvider
    {
        public string Name => "FastPay";

        public async Task<(bool success, string externalId, string status)> PayAsync(decimal amount, string currency)
        {
            //Constrói payload no formato do FastPay
            var payload = new
            {
                transaction_amount = amount,
                currency = currency,
                payer = new { email = "cliente@teste.com" },
                installments = 1,
                description = "Compra via FastPay"
            };

            //MOCK: substitua por HttpClient para chamada real
            var resp = await ProviderMocks.CallFastPayAsync(payload);

            if (resp is null) return (false, string.Empty, "failed");

            return (resp.status == "approved", resp.id, resp.status);
        }
    }
}
