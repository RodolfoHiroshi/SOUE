# PayFlow – Sistema de Pagamentos com Provedores Integrados

Este projeto implementa a camada de pagamentos do sistema PayFlow, um gateway que se conecta a múltiplos provedores de forma flexível e transparente. A aplicação foi desenvolvida em C# com .NET 8 e está pronta para execução via Docker ou CLI.

## Objetivo

Criar um sistema capaz de processar pagamentos utilizando dois provedores distintos (FastPay e SecurePay), alternando entre eles automaticamente conforme o valor da transação e disponibilidade.

## Funcionalidades

*   Integração com FastPay e SecurePay
*   Seleção automática de provedor:
    *   Valores abaixo de R$100,00 → FastPay
    *   Valores iguais ou acima de R$100,00 → SecurePay
*   Fallback automático: se o provedor principal estiver indisponível, o sistema tenta o outro
*   Cálculo de taxas por provedor:
    *   FastPay: 3,49% sobre o valor
    *   SecurePay: 2,99% + R$0,40 fixos
*   API RESTful com endpoint POST /payments
*   Resposta inclui valor bruto, taxa e valor líquido

## Exemplo de requisição

```
POST /payments

Content-Type: application/json

{
	"amount": 120.50,
	"currency": "BRL"
}
```

## Exemplo de resposta

```
{
	"id": 2,
	"externalId": "SP-19283",
	"status": "approved",
	"provider": "SecurePay",
	"grossAmount": 120.50,
	"fee": 4.01,
	"netAmount": 116.49 
} 
```

## Como executar

### Requisitos

*   Docker e Docker Compose instalados
*   (Opcional) .NET SDK 8.0 para execução local

### Executando com Docker

```
docker compose up --build

```

A API estará disponível em: [http://localhost:5000](http://localhost:5000)

### Executando localmente

```
dotnet restore

dotnet run 
```

## Testando a API

```
curl -X POST http://localhost:5000/payments 

-H "Content-Type: application/json" -d '{"amount":120.50,"currency":"BRL"}' 
```

## Estrutura do projeto

```
PayFlow/

├── PayFlow.csproj 
├── Program.cs 
├── Models/ 
├── Services/ 
├── Mocks/ 
├── Dockerfile 
├── docker-compose.yml 
├── .gitignore 
└── README.md 
```

## Observações

*   Os provedores estão simulados via mocks, mas a estrutura permite substituição fácil por chamadas reais via HttpClient.
*   O projeto está preparado para extensão com novos provedores, persistência de dados e testes automatizados.
