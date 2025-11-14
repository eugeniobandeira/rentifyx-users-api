# Testes de Integração - Arquitetura Otimizada

## 🎯 Visão Geral

Este projeto utiliza uma arquitetura otimizada de testes de integração usando **xUnit Collection Fixtures** para maximizar performance no CI/CD.

## 🏗️ Arquitetura

### Componentes Principais

```
┌─────────────────────────────────────────────┐
│  IntegrationTestFixture (Collection)        │
│  • 1 Container DynamoDB compartilhado       │
│  • 1 WebApplicationFactory reutilizável     │
│  • Inicializado UMA VEZ para todos testes  │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│  IntegrationTestCollection                  │
│  • Marca testes para compartilhar fixture   │
│  • [Collection("Integration Tests")]        │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│  IntegrationTestBase                        │
│  • Classe base para testes                  │
│  • Recebe fixture via construtor            │
│  • Cria HttpClient por teste                │
│  • Limpa dados após cada teste              │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│  UsersApiIntegrationTests                   │
│  • Testes concretos                         │
│  • 18 testes compartilhando mesma infra     │
└─────────────────────────────────────────────┘
```

## ⚡ Performance

### Antes (Abordagem Antiga)
- **Container por teste:** 18 testes = 18 containers criados/destruídos
- **Tempo médio:** ~8-15 minutos
- **Recursos:** Alto consumo de CPU/memória

### Depois (Abordagem Otimizada)
- **Container único:** 18 testes = 1 container compartilhado
- **Tempo médio:** ~2-4 minutos (**~70% mais rápido**)
- **Recursos:** Consumo reduzido drasticamente

## 🔒 Isolamento de Testes

Apesar de compartilhar o container, **cada teste permanece isolado**:

1. **HttpClient novo** por teste
2. **Cleanup de dados** após cada teste
3. **Testes NÃO rodam em paralelo** (mesma collection)
4. **Estado limpo** garantido entre execuções

## 📝 Como Escrever Novos Testes

### 1. Herdar de IntegrationTestBase

```csharp
[Collection("Integration Tests")] // IMPORTANTE: Adicione este atributo
public class MinhaNovaClasseDeTestesTests : IntegrationTestBase
{
    public MinhaNovaClasseDeTestesTests(IntegrationTestFixture fixture)
        : base(fixture)
    {
        // Construtor necessário para injetar a fixture
    }

    [Fact]
    public async Task MeuTeste_DevePassar()
    {
        // Use HttpClient já configurado
        var response = await HttpClient.GetAsync("/api/endpoint");

        // Assertions...
    }
}
```

### 2. Atributos Importantes

- **`[Collection("Integration Tests")]`**: OBRIGATÓRIO para compartilhar a fixture
- **`[Fact]`**: Testes individuais
- **`[Theory]`**: Testes parametrizados

## 🐳 Testcontainers no CI/CD

### Configurações Otimizadas

O pipeline GitHub Actions inclui:

1. **Pre-pull de imagens:** DynamoDB baixado antes dos testes
2. **Variáveis de ambiente otimizadas:**
   - `TESTCONTAINERS_RYUK_DISABLED: false` (cleanup automático)
   - `DOCKER_HOST: unix:///var/run/docker.sock`
   - `CI: true` (detecção de ambiente CI)

3. **Logging detalhado:** Verbosidade aumentada para diagnóstico
4. **Diagnóstico em falhas:** Logs de containers e resultados

## 🧪 Executando Localmente

```bash
# Todos os testes de integração
dotnet test tests/Integration/Integration.Test/Integration.Test.csproj

# Com verbosidade detalhada
dotnet test tests/Integration/Integration.Test/Integration.Test.csproj --verbosity detailed

# Teste específico
dotnet test --filter "FullyQualifiedName~UsersApiIntegrationTests.CreateUser_WithValidData"
```

## 🔍 Troubleshooting

### Container não inicia
- Verifique se Docker está rodando: `docker ps`
- Verifique logs: Os testes mostram logs detalhados no console

### Testes falhando por timeout
- Aumente timeout no `IntegrationTestFixture` (linha ~75)
- Verifique recursos disponíveis no CI

### Dados não são limpos entre testes
- Verifique se `CleanupDatabaseAsync()` está sendo chamado
- Logs devem mostrar: `🧹 [Test] Database cleaned after test`

## 📊 Métricas de Qualidade

- **18 testes de integração** cobrindo endpoints principais
- **Coverage:** Enviado automaticamente ao SonarCloud
- **Execução:** Paralela com testes unitários no pipeline

## 🔗 Referências

- [xUnit Collection Fixtures](https://xunit.net/docs/shared-context#collection-fixture)
- [Testcontainers for .NET](https://dotnet.testcontainers.org/)
- [WebApplicationFactory](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests)

---

**Última atualização:** 2025
**Mantido por:** DevOps Team
