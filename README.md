# Marco AspNetCore API CQS

Exemplo de API ASP.NET Core aplicando o padrão **CQS (Command Query Separation)** com **MediatR** e acesso a dados com **Dapper**.

## 📌 Visão geral

Este projeto demonstra como separar operações de leitura (queries) de operações de comando (commands), mantendo responsabilidades bem definidas por camada:

- **WebApi**: exposição dos endpoints HTTP e mapeamento de DTOs.
- **Application**: comandos e handlers de aplicação.
- **Infra.Data.Dapper**: queries e handlers de leitura com Dapper.
- **Domain**: entidades e regras de domínio.

> Atualmente, o handler de query retorna um objeto fake para fins de exemplo, mas o código já contém o esqueleto para consulta real em SQL Server com Dapper.

## 🧱 Arquitetura e projetos

A solution possui 4 projetos principais:

- `Marco.AspNetCore.Cqs.WebApi` (`net10.0`)
- `Marco.AspNetCore.Cqs.Application` (`net10.0`)
- `Marco.AspNetCore.Cqs.Infra.Data.Dapper` (`net10.0`)
- `Marco.AspNetCore.Cqs.Domain` (`net10.0`)

Fluxo principal de consulta por CPF:

1. Controller recebe o request.
2. Endpoint via **Command** envia `ConsultarPessoaFisicaPorCpfCommand` para o MediatR.
3. O command handler delega para a query `ConsultarPessoaFisicaPorCpfQuery`.
4. Query handler executa a leitura (atualmente mock/fake) e retorna `PessoaFisica`.

## ✅ Pré-requisitos

- **.NET SDK 10** (compatível com `net10.0`) 
- SQL Server (opcional no estado atual, obrigatório se você ativar a query real no handler)

> Observação: o projeto foi migrado para .NET 10; use SDK/runtime 10.0 para build e execução.

## ⚙️ Configuração

Arquivo principal de configuração:

- `src/Marco.AspNetCore.Cqs.WebApi/appsettings.json`

Configurar a connection string em `SqlServerReadOnlySettings:DefaultConnection`:

```json
"SqlServerReadOnlySettings": {
  "DefaultConnection": "Data Source=SEU_SERVIDOR,1433;Initial Catalog=SEU_BANCO;User Id=SEU_USUARIO;Password=SUA_SENHA;Application Name=MarcoAspNetCoreCqsWebApi"
}
```

## 🚀 Como executar

### Opção 1: via CLI

```bash
cd src/Marco.AspNetCore.Cqs.WebApi
dotnet restore
dotnet run
```

Por padrão, o profile do projeto usa:

- `http://localhost:5000`
- Swagger em `/swagger`

### Opção 2: via Visual Studio

1. Abra `src/MarcoAspNetCoreCqs.sln`.
2. Defina `Marco.AspNetCore.Cqs.WebApi` como startup project.
3. Execute com IIS Express ou perfil do projeto.

## 📚 Endpoints principais

Controller: `PessoasFisicasController` (versão `1.0`).

### 1) Consultar via Command

```http
GET /ConsultarViaCommand/{cpf}
```

Exemplo:

```bash
curl "http://localhost:5000/ConsultarViaCommand/11144477735"
```

### 2) Consultar via Query

```http
GET /ConsultarViaQuery/{cpf}
```

Exemplo:

```bash
curl "http://localhost:5000/ConsultarViaQuery/11144477735"
```

### Respostas esperadas

- `200 OK`: retorna dados da pessoa física.
- `404 Not Found`: quando não houver registro.
- `400 Bad Request`: para erros de validação/exceções de domínio.
- `500 Internal Server Error`: erro inesperado.

Exemplo de payload de sucesso:

```json
{
  "cpf": "111.444.777-35",
  "nome": "Maria da Silva",
  "dataNascimento": "1990-01-01T00:00:00"
}
```

## 🧪 Testes e validação rápida

A solution possui o projeto `Marco.AspNetCore.Cqs.UnitTests` para testes unitários com xUnit + FluentAssertions.

Para executar:

```bash
cd src
dotnet test MarcoAspNetCoreCqs.sln
```

Validação manual adicional:

1. Suba a API.
2. Acesse `http://localhost:5000/swagger`.
3. Execute os endpoints de consulta com um CPF válido.

## 🔌 Como ligar uma consulta real no SQL Server

No arquivo `ConsultarPessoaFisicaPorCpfQueryHandler`, já existe trecho comentado para Dapper. Para ativar:

1. Descomente a criação de parâmetros e SQL.
2. Substitua `YourTable` e colunas pelo seu schema real.
3. Garanta que a connection string está correta no `appsettings.json`.
4. Remova/ajuste o retorno fake (`Maria da Silva`).

## 🛠️ Stack e dependências principais

- ASP.NET Core Web API
- MediatR
- Dapper
- AutoMapper
- Swagger (Swashbuckle)

## 📝 Observações importantes

- Este repositório é **didático** para demonstrar CQS.
- Há uma separação clara entre intenção de comando e consulta.
- O endpoint de command, no exemplo atual, também resulta em leitura (encadeando query), intencionalmente para demonstrar o fluxo via MediatR.
