using AcademicManagementApi.Entities;
using AcademicManagementApi.Interfaces;
using AcademicManagementApi.Models;

namespace AcademicManagementApi.Services
{
    public class StudentService(IStudentRepository repository) : IStudentService
    {
        private readonly IStudentRepository _repository = repository;


        public async Task<StudentViewDto?> GetByIdAsync(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
            {
                return null;
            }

            return new StudentViewDto(student.Id, student.Name, student.Email, student.RA, student.CPF);
        }

        public async Task<StudentViewDto?> CreateAsync(StudentCreateDto dto)
        {
            if (await _repository.ExistsByRAAsync(dto.RA))
            {
                return null;
            }

            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                RA = dto.RA,
                CPF = dto.CPF
            };

            var createdStudent = await _repository.CreateAsync(student);
            return new StudentViewDto(createdStudent.Id, createdStudent.Name, createdStudent.Email, createdStudent.RA, createdStudent.CPF);
        }

        public async Task<bool> UpdateAsync(int id, StudentUpdateDto dto)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
            {
                return false;
            }

            student.Name = dto.Name;
            student.Email = dto.Email;

            await _repository.UpdateAsync(student);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<PagedResultDto<StudentViewDto>> GetPagedAsync(int pageNumber, int pageSize, string? search = null, string? sortBy = null, bool sortDesc = false)
        {
            var students = await _repository.GetPagedAsync(pageNumber, pageSize, search, sortBy, sortDesc);
            var totalItems = await _repository.CountAsync(search);
            var items = students.Select(student => new StudentViewDto(student.Id, student.Name, student.Email, student.RA, student.CPF));
            return new PagedResultDto<StudentViewDto>(
                items,
                totalItems,
                pageNumber,
                pageSize
            );
        }
    }
}
