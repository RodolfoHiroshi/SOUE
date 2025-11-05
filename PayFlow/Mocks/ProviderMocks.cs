using PayFlow.Models;

namespace PayFlow.Mocks
{
    public static class ProviderMocks
    {
        private static readonly Random _rnd = new();

        public static Task<FastPayResponse?> CallFastPayAsync(object payload)
        {
            // Simula indisponibilidade aleatória (10% fail)
            if (_rnd.NextDouble() < 0.1) return Task.FromResult<FastPayResponse?>(null);

            var id = $"FP-{_rnd.Next(100000, 999999)}";
            var resp = new FastPayResponse(id, "approved", "Pagamento aprovado");
            return Task.FromResult<FastPayResponse?>(resp);
        }

        public static Task<SecurePayResponse?> CallSecurePayAsync(object payload)
        {
            if (_rnd.NextDouble() < 0.1) return Task.FromResult<SecurePayResponse?>(null);

            var id = $"SP-{_rnd.Next(10000, 99999)}";
            var resp = new SecurePayResponse(id, "success");
            return Task.FromResult<SecurePayResponse?>(resp);
        }
    }
}
