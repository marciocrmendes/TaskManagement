# Sistema TaskManagement

## Sobre o Projeto
O TaskManagement é um sistema de gerenciamento de tarefas e projetos construído com tecnologias modernas. Ele permite criar projetos, adicionar tarefas, acompanhar histórico de mudanças e gerar relatórios.

## Observação
O endpoint Auth foi criado apenas para brincar com as policies e identificar o usuário, sem a necessidade de criar o CRUD do usuário, conforme pedido no teste: https://meteor-ocelot-f0d.notion.site/NET-C-5281edbec2e4480d98552e5ca0242c5b 

## Tecnologias Utilizadas

### Arquitetura e Padrões de Design
1. **Arquitetura em Camadas (Clean Architecture)**:
   - API: Interface de comunicação externa via REST
   - CrossCutting: Elementos compartilhados entre camadas
   - Domain: Regras de negócio e entidades principais
   - Infra: Acesso a dados e implementações concretas

2. **Domain-Driven Design (DDD)**:
   - Entidades com identidade (Entity\<Guid>)
   - Agregados (Pastas Aggregates)
   - Domínios delimitados (bounded contexts)
   - Value Objects (DTOs)

3. **Padrão CQRS**:
   - Separação clara entre comandos (escrita) e queries (leitura)
   - Uso de Commands e Queries em pastas separadas
   - Handlers específicos para cada operação

4. **Padrão Repository**: Interfaces e implementações concretas para acesso aos dados

5. **Padrão Unit of Work**: Para controle transacional

6. **Event-Driven Architecture**: Uso de eventos de domínio para comunicação entre componentes

7. **Padrão Mediator**: Implementado via MediatR para desacoplamento entre os componentes

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

## Funcionalidades Principais
- Criação e gerenciamento de projetos
- Criação e gerenciamento de tarefas
- Histórico de alterações nas tarefas
- Autenticação e autorização de usuários
- Relatórios de desempenho

## Estrutura do Projeto
- **TaskManagement.API**: Camada de apresentação e endpoints
- **TaskManagement.CrossCutting**: Componentes compartilhados
- **TaskManagement.Domain**: Entidades e lógica de negócio
- **TaskManagement.Infra**: Implementação de persistência
- **TaskManagement.UnitTests**: Testes unitários

## Oportunidades de Melhoria

### Arquitetura e Design
1. **Completar implementação de TaskComments**
2. **Padronizar nomenclatura** (ex: "Avarege" → "Average")
3. **Corrigir bug no Soft Delete**
4. **Implementar Cache**
5. **Parametrizar limite de tarefas por projeto**

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
