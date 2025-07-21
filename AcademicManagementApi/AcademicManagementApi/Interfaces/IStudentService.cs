using AcademicManagementApi.Models;

namespace AcademicManagementApi.Interfaces
{
    public interface IStudentService
    {
        Task<StudentViewDto?> GetByIdAsync(int id);
        Task<StudentViewDto?> CreateAsync(StudentCreateDto dto);
        Task<bool> UpdateAsync(int id, StudentUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<PagedResultDto<StudentViewDto>> GetPagedAsync(int pageNumber, int pageSize, string? search = null, string? sortBy = null, bool sortDesc = false);
    }
}
