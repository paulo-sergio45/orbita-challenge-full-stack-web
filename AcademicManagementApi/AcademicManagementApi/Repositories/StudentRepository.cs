using AcademicManagementApi.Data;
using AcademicManagementApi.Entities;
using AcademicManagementApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AcademicManagementApi.Repositories
{
    public class StudentRepository(AcademicContext context) : IStudentRepository
    {
        private readonly AcademicContext _context = context;

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student?> GetByRAAsync(string ra)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.RA == ra);
        }

        public async Task<Student> CreateAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return false;
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Students.AnyAsync(s => s.Id == id);
        }

        public async Task<bool> ExistsByRAAsync(string ra)
        {
            return await _context.Students.AnyAsync(s => s.RA == ra);
        }

        public async Task<IEnumerable<Student>> GetPagedAsync(int pageNumber, int pageSize, string? search = null, string? sortBy = null, bool sortDesc = false)
        {
            var query = _context.Students.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var lower = search.ToLower();
                query = query.Where(s =>
                    s.Name.ToLower().Contains(lower) ||
                    s.Email.ToLower().Contains(lower) ||
                    s.RA.ToLower().Contains(lower) ||
                    s.CPF.ToLower().Contains(lower)
                );
            }
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy == "name")
                    query = sortDesc ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name);
                else if (sortBy == "email")
                    query = sortDesc ? query.OrderByDescending(s => s.Email) : query.OrderBy(s => s.Email);
                else if (sortBy == "ra")
                    query = sortDesc ? query.OrderByDescending(s => s.RA) : query.OrderBy(s => s.RA);
                else if (sortBy == "cpf")
                    query = sortDesc ? query.OrderByDescending(s => s.CPF) : query.OrderBy(s => s.CPF);
                else
                    query = query.OrderBy(s => s.Id);
            }
            else
            {
                query = query.OrderBy(s => s.Id);
            }
            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAsync(string? search = null)
        {
            var query = _context.Students.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var lower = search.ToLower();
                query = query.Where(s =>
                    s.Name.ToLower().Contains(lower) ||
                    s.Email.ToLower().Contains(lower) ||
                    s.RA.ToLower().Contains(lower) ||
                    s.CPF.ToLower().Contains(lower)
                );
            }
            return await query.CountAsync();
        }
    }
}