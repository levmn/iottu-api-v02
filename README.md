# 🛜 Iottu

**Iottu** é um sistema para localização e controle de motos, inspirado em um desafio real prposto pela Mottu. Esta versão foi reestruturada seguindo princípios SOLID, arquitetura em camadas e boas práticas REST, com documentação via Swagger/OpenAPI, e cobertura de testes unitários e de integração.

## 👥 Integrantes
- [RM558948] [Allan Brito Moreira](https://github.com/Allanbm100)
- [RM558868] [Caio Liang](https://github.com/caioliang)
- [RM98276] [Levi Magni](https://github.com/levmn)

## 🧱 Arquitetura e Justificativa
A solução adota uma arquitetura em camadas com separação clara de responsabilidades, orientada a SOLID:

- `Core.Iottu.Application`: Serviços de aplicação (casos de uso). Convertem entidades em DTOs, orquestram repositórios e garantem regras de aplicação. Depende de abstrações do domínio (DIP) e mantém baixo acoplamento (ISP).
- `Core.Iottu.Domain`: Entidades e contratos (Interfaces de Repositórios). Mantém o domínio isolado de detalhes de implementação.
- `Infrastructure.Iottu.Persistence`: Persistência com EF Core (Oracle), Migrations e Repositórios. Implementa interfaces do domínio (DIP) e segue SRP para cada repositório.
- `Shared.Iottu.Contracts`: DTOs e contratos compartilhados, com comentários XML para documentação automatizada do Swagger.
- `Web.Iottu.Api.Catalog`: ASP.NET Core Web API. Expõe endpoints RESTful, versão, documentação e HATEOAS. Controladores finos (Controller → Service → Repository), cumprindo SRP.

Principais práticas adotadas:
- SOLID (SRP, OCP, LSP, ISP, DIP) aplicado aos módulos e dependências.
- REST com CRUD completo, paginação, HATEOAS, códigos de status adequados e exemplos de payloads.
- Swagger/OpenAPI com XML comments (descrições de endpoints, parâmetros e modelos) + exemplos.

### Entidades Principais
- Antena, Moto, Patio, StatusMoto, Tag, Usuario

## 🗂️ Estrutura do Projeto (resumo)
- `Core.Iottu.Application/Services`: serviços (`MotoService`, `TagService`, ...)
- `Core.Iottu.Domain/Entities`: modelos de domínio (Moto, Tag, Antena, Patio)
- `Core.Iottu.Domain/Interfaces`: contratos de repositórios (`IMotoRepository`, etc.)
- `Infrastructure.Iottu.Persistence/Contexts`: `IottuDbContext`
- `Infrastructure.Iottu.Persistence/Repositories`: repositórios EF Core
- `Shared.Iottu.Contracts/DTOs`: DTOs (inclui `PagedResponse<T>` para paginação)
- `tests/Core.Iottu.Api.IntegrationTests`: testes de integração
- `tests/Core.Iottu.Application.Tests`: testes unitários
- `Web.Iottu.Api.Catalog/Controllers`: controladores REST (Antenas, Motos, Patios, Tags)
- `Web.Iottu.Api.Catalog/Helpers`: `HateoasHelper`, filtros do Swagger

## ⚙️ Requisitos e Configuração
### Pré-requisitos
- .NET SDK 9.0
- Acesso a banco Oracle (para migrations/execução)

## 🧱 Migrations (EF Core + Oracle) e Atualização do Banco
Esta seção descreve como criar e aplicar migrations no projeto e como atualizar o banco Oracle do seu usuário.

### 1) Instalar o EF Core
```bash
dotnet tool install --global dotnet-ef
# Se já tiver instalado:
# dotnet tool update --global dotnet-ef
```

### 2) Configurar a conexão
- Faça uma cópia do arquivo `.env.sample` em `Web.Iottu.Api.Catalog` com, e renomeie para `.env`;
- Atualize as variáveis com as suas credenciais:

```bash
DB_USER=seu_usuario
DB_PASSWORD=sua_senha
```

### 3) Criar uma nova migration
Execute na raiz do repositório, apontando o projeto de migrations e o projeto de inicialização da API:

```bash
dotnet ef migrations add Init \
  -p Infrastructure.Iottu.Persistence/Infrastructure.Iottu.Persistence.csproj \
  -s Web.Iottu.Api.Catalog/Web.Iottu.Api.Catalog.csproj \
  -c Infrastructure.Iottu.Persistence.Contexts.IottuDbContext
```

Troque `Init` pelo nome da sua alteração (ex.: `Add_StatusMoto_Seed`, `Alter_Moto_Chassi_Unique`). Os arquivos serão gerados em `Infrastructure.Iottu.Persistence/Migrations`.

### 4) Aplicar migrations no seu banco Oracle
Com as variáveis `DB_USER`/`DB_PASSWORD` configuradas e o Oracle acessível, rode:

```bash
dotnet ef database update \
  -p Infrastructure.Iottu.Persistence/Infrastructure.Iottu.Persistence.csproj \
  -s Web.Iottu.Api.Catalog/Web.Iottu.Api.Catalog.csproj \
  -c Infrastructure.Iottu.Persistence.Contexts.IottuDbContext
```

Isso cria/atualiza as tabelas no schema do usuário Oracle definido em `DB_USER`.

### 5) Reverter a última migration (sem aplicar no banco)
```bash
dotnet ef migrations remove \
  -p Infrastructure.Iottu.Persistence/Infrastructure.Iottu.Persistence.csproj \
  -s Web.Iottu.Api.Catalog/Web.Iottu.Api.Catalog.csproj
```

Para reverter o banco a uma migration anterior específica:
```bash
dotnet ef database update NomeDaMigrationAnterior \
  -p Infrastructure.Iottu.Persistence/Infrastructure.Iottu.Persistence.csproj \
  -s Web.Iottu.Api.Catalog/Web.Iottu.Api.Catalog.csproj \
  -c Infrastructure.Iottu.Persistence.Contexts.IottuDbContext
```

## ▶️ Executando a API
Na raiz do repositório:

```bash
# Restaurar dependências
dotnet restore

# Construir a solução
dotnet build

# Executar a API de Catálogo (Swagger exposto na raiz "/")
dotnet run --project Web.Iottu.Api.Catalog
```

Por padrão (Development), o Swagger ficará disponível em `http://localhost:5102/` (ajuste conforme seu `launchSettings.json`).

## 📜 Documentação (Swagger/OpenAPI)
A API habilita Swagger com:
- Descrições via XML comments (controllers e DTOs)
- Exemplos de payloads via `ExamplesSchemaFilter`
- Parâmetros de paginação documentados via `QueryParameterOperationFilter`

## 📌 Endpoints Principais (CRUD + Paginação + HATEOAS)
Recursos: `antenas`, `motos`, `patios`, `tags`, `usuarios`.

### Listagem paginada
```http
GET /api/v1/motos?page=1&pageSize=10
```
Resposta (exemplo):
```json
{
  "page": 1,
  "pageSize": 10,
  "totalItems": 42,
  "totalPages": 5,
  "items": [ { "data": { "id": "...", "placa": "...", "modelo": "..." }, "_links": [ { "href": "/api/v1/motos/{id}", "rel": "self", "method": "GET" } ] } ],
  "links": [
    { "href": "/api/v1/motos?page=1&pageSize=10", "rel": "self", "method": "GET" },
    { "href": "/api/v1/motos?page=2&pageSize=10", "rel": "next", "method": "GET" }
  ]
}
```

### Buscar por id
```http
GET /api/v1/motos/{id}
```

### Criar
```http
POST /api/v1/motos
Content-Type: application/json

{
  "placa": "HOQ-6915",
  "modelo": "Honda CG 160 | Mottu-E",
  "chassi": "9C2KC1670JR123456",
  "numeroMotor": "KC16E-1234567",
  "statusId": 1,
  "tagId": "e5e0e2ff-749d-41c1-aaa2-e6eb33df66d7",
  "patioId": "40dc9fbe-91ef-4528-bf30-6a9913cabffb"
}
```

### Atualizar
```http
PUT /api/v1/motos/{id}
Content-Type: application/json

{
  "modelo": "Honda CG 160",
  "chassi": "9C2KC1670JR123456",
  "numeroMotor": "KC16E-1234567",
  "statusId": 1,
  "tagId": "e5e0e2ff-749d-41c1-aaa2-e6eb33df66d7",
  "patioId": "40dc9fbe-91ef-4528-bf30-6a9913cabffb"
}
```

### Remover
```http
DELETE /api/v1/motos/{id}
```

Observações:
- Paginação exige `page >= 1` e `pageSize >= 1`. Valores inválidos retornam 400.
- Respostas de lista retornam envelope `PagedResponse<T>` com metadados e HATEOAS de coleção (`self`, `prev`, `next`).
- Cada item possui HATEOAS (`self`, `update`, `delete`).
- A aplicação utiliza versionamento via URL, os endpoints seguem o padrão: `/api/{version}/motos`, `/api/{version}/patios`.

## 🔐 Segurança: Autenticação e Autorização (JWT)
A API utiliza JWT (JSON Web Token) para autenticação e controle de acesso.

### Fluxo de Autenticação
1. Realize uma requisição `POST` para `/api/v1/auth/login`:
```json
{
  "username": "admin",
  "password": "admin123"
}
```

2. Receba o token JWT na resposta:
```json
{
  "accessToken": "<jwt_token>",
  "expiresAt": "2025-11-08T12:00:00Z"
}
```

3. Use o token nas chamadas autenticadas:
```json
Authorization: Bearer <jwt_token>
```

Perfis e autorização:
- `admin`: acesso completo
- `user` (default): acesso restrito a recursos específicos

O Swagger inclui o botão Authorize (cadeado no topo direito) para testar endpoints autenticados.


## 🩺 Health Check
A API expõe um endpoint de monitoramento de saúde:

```http
GET /api/v1/health
```

Este endpoint é utilizado para verificação de **disponibilidade**, **readiness probes** e **monitoramento de banco** (via `IottuDbContext`).

## 🧪 Testes
A solução possui cobertura de testes unitários e de integração com `xUnit`.

### Testes Unitários 
Localizados em `tests/Core.Iottu.Application.Tests`:
- Validam a lógica de negócio dos serviços (MotoService, UsuarioService etc).
- Utilizam mocks de repositórios.

### Testes de Integração
Localizados em `tests/Core.Iottu.Api.IntegrationTests`:
- Usam `WebApplicationFactory<Program>` e banco InMemory.
- Verificam autenticação, CRUD e endpoints da API real.


### Execução dos testes

```bash
dotnet test
```

Para rodar um projeto específico:
```bash
dotnet test tests/Core.Iottu.Api.IntegrationTests/Core.Iottu.Api.IntegrationTests.csproj
```
