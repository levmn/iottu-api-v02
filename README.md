# 🛜 Iottu

**Iottu** é um sistema para localização e controle de motos, inspirado em um desafio real da Mottu. Esta versão foi reestruturada seguindo princípios SOLID, arquitetura em camadas e boas práticas REST, com documentação via Swagger/OpenAPI.

## 👥 Integrantes
- [RM558948] [Allan Brito Moreira](https://github.com/Allanbm100)
- [RM558868] [Caio Liang](https://github.com/caioliang)
- [RM98276] [Levi Magni](https://github.com/levmn)

## 🧱 Arquitetura e Justificativa
A solução adota uma arquitetura em camadas com separação clara de responsabilidades, orientada a SOLID:

- `Core.Iottu.Domain`: Entidades e contratos (Interfaces de Repositórios). Mantém o domínio isolado de detalhes de implementação.
- `Infrastructure.Iottu.Persistence`: Persistência com EF Core (Oracle), Migrations e Repositórios. Implementa interfaces do domínio (DIP) e segue SRP para cada repositório.
- `Core.Iottu.Application`: Serviços de aplicação (casos de uso). Convertem entidades em DTOs, orquestram repositórios e garantem regras de aplicação. Depende de abstrações do domínio (DIP) e mantém baixo acoplamento (ISP).
- `Shared.Iottu.Contracts`: DTOs e contratos compartilhados, com comentários XML para documentação automatizada do Swagger.
- `Web.Iottu.Api.Catalog`: ASP.NET Core Web API. Expõe endpoints RESTful, versão, documentação e HATEOAS. Controladores finos (Controller → Service → Repository), cumprindo SRP.

Principais práticas adotadas:
- SOLID (SRP, OCP, LSP, ISP, DIP) aplicado aos módulos e dependências.
- REST com CRUD completo, paginação, HATEOAS, códigos de status adequados e exemplos de payloads.
- Swagger/OpenAPI com XML comments (descrições de endpoints, parâmetros e modelos) + exemplos.

### Entidades Principais
- Moto, Tag, Antena, Patio

## 🗂️ Estrutura do Projeto (resumo)
- `Core.Iottu.Domain/Entities`: modelos de domínio (Moto, Tag, Antena, Patio)
- `Core.Iottu.Domain/Interfaces`: contratos de repositórios (`IMotoRepository`, etc.)
- `Infrastructure.Iottu.Persistence/Contexts`: `IottuDbContext`
- `Infrastructure.Iottu.Persistence/Repositories`: repositórios EF Core
- `Core.Iottu.Application/Services`: serviços (`MotoService`, `TagService`, ...)
- `Shared.Iottu.Contracts/DTOs`: DTOs (inclui `PagedResponse<T>` para paginação)
- `Web.Iottu.Api.Catalog/Controllers`: controladores REST (Antenas, Motos, Patios, Tags)
- `Web.Iottu.Api.Catalog/Helpers`: `HateoasHelper`, filtros do Swagger

## ⚙️ Requisitos e Configuração
### Pré-requisitos
- .NET SDK 9.0
- Acesso a banco Oracle (para migrations/execução)

### Variáveis de ambiente
A API usa variáveis de ambiente para a conexão com o Oracle. Configure um arquivo `.env` na raiz do diretório `Web.Iottu.Api.Catalog` com:

```
DB_USER=seu_usuario
DB_PASSWORD=sua_senha
```

E configure `ConnectionStrings:DefaultConnection` em `Web.Iottu.Api.Catalog/appsettings.Development.json` (host, service name, etc.). A conexão é montada em `Program.cs`.

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
Recursos: `motos`, `tags`, `antenas`, `patios`.

### Listagem paginada
```http
GET /api/motos?page=1&pageSize=10
```
Resposta (exemplo):
```json
{
  "page": 1,
  "pageSize": 10,
  "totalItems": 42,
  "totalPages": 5,
  "items": [ { "data": { "id": "...", "placa": "...", "modelo": "..." }, "_links": [ { "href": "/api/motos/{id}", "rel": "self", "method": "GET" } ] } ],
  "links": [
    { "href": "/api/motos?page=1&pageSize=10", "rel": "self", "method": "GET" },
    { "href": "/api/motos?page=2&pageSize=10", "rel": "next", "method": "GET" }
  ]
}
```

### Buscar por id
```http
GET /api/motos/{id}
```

### Criar
```http
POST /api/motos
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
PUT /api/motos/{id}
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
DELETE /api/motos/{id}
```

Observações:
- Paginação exige `page >= 1` e `pageSize >= 1`. Valores inválidos retornam 400.
- Respostas de lista retornam envelope `PagedResponse<T>` com metadados e HATEOAS de coleção (`self`, `prev`, `next`).
- Cada item possui HATEOAS (`self`, `update`, `delete`).

## 🧪 Testes
Para executar os testes (exemplo, ajuste ao seu projeto de testes):

```bash
dotnet test
```

Se houver múltiplos projetos de teste, utilize `--project caminho/do/projeto.csproj`.
