using AcademicManagementApi.Data;
using AcademicManagementApi.Entities;
using AcademicManagementApi.Models;
using AcademicManagementApi.Services;
using AcademicManagementApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AcademicManagementApi.Test
{
    public class StudentServiceTests
    {
        private DbContextOptions<AcademicContext> GetInMemoryDbContextOptions()
        {
            return new DbContextOptionsBuilder<AcademicContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private AcademicContext CreateContext()
        {
            var options = GetInMemoryDbContextOptions();
            var context = new AcademicContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task GetByIdAsync_WhenStudentExists_ShouldReturnStudent()
        {
            using var context = CreateContext();
            var repository = new StudentRepository(context);
            var service = new StudentService(repository);

            var student = new Student
            {
                Name = "João Silva",
                Email = "joao@email.com",
                RA = "2024001",
                CPF = "12345678901"
            };

            context.Students.Add(student);
            await context.SaveChangesAsync();

            var result = await service.GetByIdAsync(student.Id);

            Assert.NotNull(result);
            Assert.Equal("João Silva", result.Name);
            Assert.Equal("joao@email.com", result.Email);
            Assert.Equal("2024001", result.RA);
            Assert.Equal("12345678901", result.CPF);
        }

        [Fact]
        public async Task GetByIdAsync_WhenStudentDoesNotExist_ShouldReturnNull()
        {
            using var context = CreateContext();
            var repository = new StudentRepository(context);
            var service = new StudentService(repository);

            var result = await service.GetByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_WhenValidStudent_ShouldCreateAndReturnStudent()
        {
            using var context = CreateContext();
            var repository = new StudentRepository(context);
            var service = new StudentService(repository);

            var dto = new StudentCreateDto("João Silva", "joao@email.com", "2024001", "12345678901");

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("João Silva", result.Name);
            Assert.Equal("joao@email.com", result.Email);
            Assert.Equal("2024001", result.RA);
            Assert.Equal("12345678901", result.CPF);
            Assert.True(result.Id > 0);

            var savedStudent = await context.Students.FindAsync(result.Id);
            Assert.NotNull(savedStudent);
            Assert.Equal("João Silva", savedStudent.Name);
        }

        [Fact]
        public async Task CreateAsync_WhenRADuplicate_ShouldReturnNull()
        {
            using var context = CreateContext();
            var repository = new StudentRepository(context);
            var service = new StudentService(repository);

            var existingStudent = new Student
            {
                Name = "João Silva",
                Email = "joao@email.com",
                RA = "2024001",
                CPF = "12345678901"
            };

            context.Students.Add(existingStudent);
            await context.SaveChangesAsync();

            var dto = new StudentCreateDto("Maria Santos", "maria@email.com", "2024001", "12345678902");

            var result = await service.CreateAsync(dto);

            Assert.Null(result);

            var students = await context.Students.ToListAsync();
            Assert.Single(students);
        }

        [Fact]
        public async Task UpdateAsync_WhenStudentExists_ShouldUpdateAndReturnTrue()
        {
            using var context = CreateContext();
            var repository = new StudentRepository(context);
            var service = new StudentService(repository);

            var student = new Student
            {
                Name = "João Silva",
                Email = "joao@email.com",
                RA = "2024001",
                CPF = "12345678901"
            };

            context.Students.Add(student);
            await context.SaveChangesAsync();

            var dto = new StudentUpdateDto("João Silva Atualizado", "joao.novo@email.com");

            var result = await service.UpdateAsync(student.Id, dto);

            Assert.True(result);

            var updatedStudent = await context.Students.FindAsync(student.Id);
            Assert.NotNull(updatedStudent);
            Assert.Equal("João Silva Atualizado", updatedStudent.Name);
            Assert.Equal("joao.novo@email.com", updatedStudent.Email);
        }

        [Fact]
        public async Task UpdateAsync_WhenStudentDoesNotExist_ShouldReturnFalse()
        {
            using var context = CreateContext();
            var repository = new StudentRepository(context);
            var service = new StudentService(repository);

            var dto = new StudentUpdateDto("João Silva Atualizado", "joao.novo@email.com");

            var result = await service.UpdateAsync(999, dto);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_WhenStudentExists_ShouldDeleteAndReturnTrue()
        {
            using var context = CreateContext();
            var repository = new StudentRepository(context);
            var service = new StudentService(repository);

            var student = new Student
            {
                Name = "João Silva",
                Email = "joao@email.com",
                RA = "2024001",
                CPF = "12345678901"
            };

            context.Students.Add(student);
            await context.SaveChangesAsync();

            var result = await service.DeleteAsync(student.Id);

            Assert.True(result);

            var deletedStudent = await context.Students.FindAsync(student.Id);
            Assert.Null(deletedStudent);
        }

        [Fact]
        public async Task DeleteAsync_WhenStudentDoesNotExist_ShouldReturnFalse()
        {

            using var context = CreateContext();
            var repository = new StudentRepository(context);
            var service = new StudentService(repository);

            var result = await service.DeleteAsync(999);

            Assert.False(result);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldReturnCorrectPage()
        {
            using var context = CreateContext();
            var repository = new StudentRepository(context);
            var service = new StudentService(repository);

            for (int i = 1; i <= 25; i++)
            {
                context.Students.Add(new Student { Name = $"Aluno {i}", Email = $"aluno{i}@email.com", RA = $"2024{i:D3}", CPF = $"123456789{i:D2}" });
            }
            await context.SaveChangesAsync();

            var result = await service.GetPagedAsync(2, 10);

            Assert.NotNull(result);
            Assert.Equal(2, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(10, result.Items.Count());
            Assert.Contains(result.Items, s => s.Name == "Aluno 11");
            Assert.Contains(result.Items, s => s.Name == "Aluno 20");
        }

        [Fact]
        public async Task GetPagedAsync_WhenNoStudents_ShouldReturnEmpty()
        {
            using var context = CreateContext();
            var repository = new StudentRepository(context);
            var service = new StudentService(repository);

            var result = await service.GetPagedAsync(1, 10);

            Assert.NotNull(result);
            Assert.Empty(result.Items);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
        }

        [Fact]
        public async Task GetPagedAsync_LastPage_ShouldReturnRemainingItems()
        {
            using var context = CreateContext();
            var repository = new StudentRepository(context);
            var service = new StudentService(repository);

            for (int i = 1; i <= 23; i++)
            {
                context.Students.Add(new Student { Name = $"Aluno {i}", Email = $"aluno{i}@email.com", RA = $"2024{i:D3}", CPF = $"123456789{i:D2}" });
            }
            await context.SaveChangesAsync();

            var result = await service.GetPagedAsync(3, 10);

            Assert.NotNull(result);
            Assert.Equal(3, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(3, result.Items.Count());
            Assert.Contains(result.Items, s => s.Name == "Aluno 21");
            Assert.Contains(result.Items, s => s.Name == "Aluno 23");
        }
    }
}
