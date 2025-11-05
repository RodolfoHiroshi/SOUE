using PayFlow.Models;

namespace PayFlow.Services
{
    public interface IPaymentProvider
    {
        string Name { get; }
        Task<(bool success, string externalId, string status)> PayAsync(decimal amount, string currency);
    }
}
