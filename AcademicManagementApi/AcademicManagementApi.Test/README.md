# Testes Unitários - AcademicManagementApi

Este projeto contém testes unitários para o `StudentService` usando xUnit e Entity Framework Core In-Memory Database.

## Estrutura dos Testes

### StudentServiceTests.cs
Testes usando banco de dados em memória (In-Memory Database):
- `GetAllAsync_ShouldReturnAllStudents()` - Testa se retorna todos os estudantes
- `GetAllAsync_WhenNoStudents_ShouldReturnEmptyList()` - Testa quando não há estudantes
- `GetByIdAsync_WhenStudentExists_ShouldReturnStudent()` - Testa busca por ID existente
- `GetByIdAsync_WhenStudentDoesNotExist_ShouldReturnNull()` - Testa busca por ID inexistente
- `CreateAsync_WhenValidStudent_ShouldCreateAndReturnStudent()` - Testa criação de estudante válido
- `CreateAsync_WhenRADuplicate_ShouldReturnNull()` - Testa criação com RA duplicado
- `UpdateAsync_WhenStudentExists_ShouldUpdateAndReturnTrue()` - Testa atualização de estudante existente
- `UpdateAsync_WhenStudentDoesNotExist_ShouldReturnFalse()` - Testa atualização de estudante inexistente
- `DeleteAsync_WhenStudentExists_ShouldDeleteAndReturnTrue()` - Testa exclusão de estudante existente
- `DeleteAsync_WhenStudentDoesNotExist_ShouldReturnFalse()` - Testa exclusão de estudante inexistente

## Como Executar os Testes

### Executar todos os testes:
```bash
dotnet test
```

### Executar testes de um arquivo específico:
```bash
dotnet test --filter "FullyQualifiedName~StudentServiceTests"
```

### Executar um teste específico:
```bash
dotnet test --filter "FullyQualifiedName~GetAllAsync_ShouldReturnAllStudents"
```

### Executar com detalhes:
```bash
dotnet test --verbosity normal
```

### Executar com cobertura de código (requer coverlet):
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Padrões de Teste Utilizados

### 1. Padrão AAA (Arrange, Act, Assert)
```csharp
[Fact]
public async Task TestMethod()
{
    // Arrange - Preparar dados e dependências
    using var context = CreateContext();
    var repository = new StudentRepository(context);
    var service = new StudentService(repository);
    
    // Act - Executar a ação que está sendo testada
    var result = await service.GetAllAsync();
    
    // Assert - Verificar se o resultado é o esperado
    Assert.NotNull(result);
    Assert.Empty(result);
}
```

## Dependências

- `xUnit` - Framework de testes
- `Microsoft.EntityFrameworkCore.InMemory` - Banco em memória para testes
