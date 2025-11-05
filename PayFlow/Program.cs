using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PayFlow.Services;
using PayFlow.Models;

var builder = WebApplication.CreateBuilder(args);

// DI
builder.Services.AddSingleton<FastPayProvider>();
builder.Services.AddSingleton<SecurePayProvider>();
builder.Services.AddSingleton<PaymentService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/payments", async (PaymentRequest req, PaymentService paymentService) =>
{
    if (req == null || req.Amount <= 0 || string.IsNullOrWhiteSpace(req.Currency))
        return Results.BadRequest(new { error = "invalid payload" });

    try
    {
        var res = await paymentService.ProcessPaymentAsync(req.Amount, req.Currency);
        return Results.Ok(res);
    }
    catch (ProvidersUnavailableException ex)
    {
        // retorna JSON com corpo e código 502
        return Results.Json(new { error = ex.Message }, statusCode: 502);
    }
    catch (Exception ex)
    {
        // retorna JSON com corpo e código 500
        return Results.Json(new { error = ex.Message }, statusCode: 500);
    }
});


app.Run();
