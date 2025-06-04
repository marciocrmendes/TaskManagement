# Sistema TaskManagement

## Sobre o Projeto
O TaskManagement é um sistema de gerenciamento de tarefas e projetos construído com tecnologias modernas. Ele permite criar projetos, adicionar tarefas, acompanhar histórico de mudanças e gerar relatórios.

## Observação
O endpoint Auth foi criado apenas para brincar com as policies e identificar o usuário, sem a necessidade de criar o CRUD do usuário, conforme pedido no teste: https://meteor-ocelot-f0d.notion.site/NET-C-5281edbec2e4480d98552e5ca0242c5b 

## Tecnologias Utilizadas

### Arquitetura e Padrões de Design
1. **Arquitetura em Camadas (Clean Architecture)**:
   - API: Interface de comunicação externa via REST
   - Application: Casos de uso e orquestração da lógica de negócio
   - Domain: Regras de negócio e entidades principais
   - CrossCutting: Elementos compartilhados entre camadas
   - Infra: Acesso a dados e implementações concretas

2. **Domain-Driven Design (DDD)**:
   - Entidades com identidade (Entity\<Guid>)
   - Agregados (Pastas Aggregates no Domain)
   - Domínios delimitados (bounded contexts)
   - Value Objects (DTOs)
   - Separação clara entre Domain (regras de negócio) e Application (casos de uso)

3. **Padrão CQRS**:
   - Implementado na camada Application
   - Separação clara entre comandos (escrita) e queries (leitura)
   - Organização por Features: Authentication, Projects, Tasks, TaskComments, Reports
   - Commands, Queries e Handlers específicos para cada operação

4. **Padrão Repository**: Interfaces e implementações concretas para acesso aos dados

5. **Padrão Unit of Work**: Para controle transacional

6. **Event-Driven Architecture**: Uso de eventos de domínio para comunicação entre componentes

7. **Padrão Mediator**: Implementado via MediatR na camada Application para desacoplamento entre os componentes

### Frameworks e Bibliotecas
1. **.NET 8**: Framework principal
2. **ASP.NET Core**: Para APIs com Minimal APIs
3. **Entity Framework Core 8.0.15**: ORM para acesso a dados
4. **PostgreSQL**: Banco de dados
5. **MediatR**: Implementação do padrão mediator
6. **FluentValidation**: Para validação de entrada de dados
7. **Swagger/OpenAPI**: Documentação da API
8. **Docker**: Suporte a containerização
9. **JWT**: Autenticação baseada em tokens

## Como Executar o Projeto

### Pré-requisitos
- .NET 8 SDK
- Docker e Docker Compose
- PostgreSQL (se executar fora do Docker)

### Usando Docker (localhost:8080)
```bash
docker-compose up -d
```

### Sem Docker
1. Configure a string de conexão do PostgreSQL no arquivo `appsettings.json`
2. Execute as migrações:
```bash
dotnet ef database update --project src/TaskManagement.Infra --startup-project src/TaskManagement.API
```
3. Execute o projeto:
```bash
dotnet run --project src/TaskManagement.API
```

## Executando Testes
O projeto inclui testes unitários abrangentes que cobrem os handlers das diferentes features:

```bash
# Executar todos os testes
dotnet test

# Executar testes com mais detalhes
dotnet test --verbosity normal
```

Os testes estão organizados por agregados e features, cobrindo:
- Handlers de Projects (Create, Delete, GetAll, GetTasksById)
- Handlers de Tasks (Create, Delete, Update, GetById)
- Handlers de TaskComments (Create)
- Handlers de Authentication (CreateAccessToken)
- Handlers de Reports (UsersAverageCompletedTasks)

## Funcionalidades Principais
- Criação e gerenciamento de projetos
- Criação e gerenciamento de tarefas
- Histórico de alterações nas tarefas
- Autenticação e autorização de usuários
- Relatórios de desempenho

## Arquitetura DDD Implementada

Este projeto foi refatorado para seguir os princípios do Domain-Driven Design (DDD) com uma arquitetura em camadas bem definida:

### Camada Domain
- Contém apenas as entidades de domínio, agregados e eventos
- Livre de dependências externas
- Responsável pelas regras de negócio fundamentais

### Camada Application
- Implementa todos os casos de uso através do padrão CQRS
- Organizada por Features (Authentication, Projects, Tasks, TaskComments, Reports)
- Contém Commands, Queries, Handlers e Validações
- Orquestra as operações sem conter lógica de negócio

### Camada Infrastructure
- Implementa interfaces definidas no Domain
- Acesso a dados via Entity Framework
- Configurações de persistência

### Camada API
- Endpoints REST usando Minimal APIs
- Autenticação e autorização
- Documentação via Swagger

## Estrutura do Projeto
- **TaskManagement.API**: Camada de apresentação e endpoints REST
- **TaskManagement.Application**: Casos de uso, Commands, Queries e Handlers (CQRS)
- **TaskManagement.Domain**: Entidades de domínio, agregados e eventos
- **TaskManagement.CrossCutting**: Componentes compartilhados
- **TaskManagement.Infra**: Implementação de persistência e acesso a dados
- **TaskManagement.UnitTests**: Testes unitários

### Organização da Camada Application
A camada Application está organizada por Features seguindo o padrão CQRS:
- **Features/Authentication**: Commands e Handlers para autenticação
- **Features/Projects**: Commands, Queries e Handlers para projetos
- **Features/Tasks**: Commands, Queries e Handlers para tarefas
- **Features/TaskComments**: Commands e Handlers para comentários
- **Features/Reports**: Queries e Handlers para relatórios
- **Common**: Classes base e extensões compartilhadas

## Oportunidades de Melhoria

### Arquitetura e Design
1. **Completar implementação de TaskComments**
2. **Padronizar nomenclatura** (ex: "Avarege" → "Average")
3. **Corrigir bug no Soft Delete**
4. **Implementar Cache**
5. **Parametrizar limite de tarefas por projeto**
6. **Implementar validações mais robustas na camada Application**
7. **Adicionar mais eventos de domínio para auditoria**

### Segurança
1. **Reforçar autenticação com refresh tokens**
2. **Implementar Rate Limiting**
3. **Auditar eventos de segurança**

### Performance
1. **Otimizar queries com paginação**
2. **Considerar uso de Dapper para queries complexas**

### DevOps
1. **Implementar pipelines CI/CD**
2. **Adicionar ferramentas de monitoramento**
3. **Implementar Health Checks**

### Documentação e Testes
1. **Melhorar documentação da API no Swagger**
2. **Aumentar cobertura de testes unitários**
3. **Implementar testes de integração e E2E**
