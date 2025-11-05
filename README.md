# PayFlow – Sistema de Pagamentos

Este projeto implementa um gateway de pagamentos com dois provedores (FastPay e SecurePay), com seleção automática e fallback. Desenvolvido em C# (.NET 7) e conteinerizado com Docker.

## Como executar

### Requisitos
- Docker e Docker Compose instalados
- (Opcional) .NET SDK 8.0 para execução local

### Executar com Docker
```bash
docker compose up --build
