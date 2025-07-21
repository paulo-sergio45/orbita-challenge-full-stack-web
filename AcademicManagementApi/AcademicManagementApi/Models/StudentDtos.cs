namespace AcademicManagementApi.Models
{
    public record StudentCreateDto(string Name, string Email, string RA, string CPF);
    public record StudentUpdateDto(string Name, string Email);
    public record StudentViewDto(int Id, string Name, string Email, string RA, string CPF);
    public record PagedResultDto<T>(IEnumerable<T> Items, int TotalItems, int PageNumber, int PageSize);
}
