# Microondas Benner

Uma API REST para simular e controlar o funcionamento de um microondas, desenvolvida como desafio Coodesh.

## Sobre o Projeto

Sistema que fornece uma interface para gerenciar o estado de um microondas, incluindo funcionalidades como iniciar, pausar, aquecer com diferentes níveis de potência e executar programas de aquecimento predefinidos.

## Tecnologias Utilizadas

- **Linguagem**: C#
- **Framework**: .NET 9.0 / ASP.NET Core
- **Arquitetura**: Clean Architecture (Domain, Infrastructure, API)
- **Padrões**: Dependency Injection, Repository Pattern
- **Autenticação**: Bearer Token
- **CORS**: Habilitado para requisições cross-origin

## Como Instalar

### Pré-requisitos

- .NET 9.0 SDK ou superior
- Git

### Passos de Instalação

1. Clone o repositório:
```bash
git clone https://github.com/seu-usuario/MicroondasBenner.git
cd MicroondasBenner
```

2. Restaure as dependências:
```bash
dotnet restore
```

3. Compile o projeto:
```bash
dotnet build
```

## Como Usar

### Executar a API

1. Acesse a pasta do projeto da API:
```bash
cd src/Microondas.Api
```

2. Execute o projeto:
```bash
dotnet run
```

A API será disponibilizada em `http://localhost:5149`

### Endpoints Disponíveis

A documentação dos endpoints está disponível via OpenAPI (Swagger) em:
```
http://localhost:5149/openapi/v1.json
```

### Exemplos de Uso

Você pode utilizar o arquivo `Microondas.Api.http` incluído no projeto para fazer requisições de teste.

## Rodando os Testes

```bash
cd tests/Microondas.Domain.Tests
dotnet test
```

## Estrutura do Projeto

```
src/
├── Microondas.Api/        # Camada de apresentação (Controllers, Middleware)
├── Microondas.Domain/     # Camada de domínio (Entities, Interfaces, DTOs)
├── Microondas.Infrastructure/  # Camada de infraestrutura (Services, Repositories)
└── Microondas.UI/         # Interface Blazor (opcional)
```

---

>  This is a challenge by [Coodesh](https://coodesh.com/)

