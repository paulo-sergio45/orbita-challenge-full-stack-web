using AcademicManagementApi.Entities;

namespace AcademicManagementApi.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task<Student?> GetByRAAsync(string ra);
        Task<Student> CreateAsync(Student student);
        Task<Student> UpdateAsync(Student student);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByRAAsync(string ra);
        Task<IEnumerable<Student>> GetPagedAsync(int pageNumber, int pageSize, string? search = null, string? sortBy = null, bool sortDesc = false);
        Task<int> CountAsync(string? search = null);
    }
}