using AcademicManagementApi.Interfaces;
using AcademicManagementApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AcademicManagementApi.Controllers.v1
{
    [ApiController]
    [Route("v1/[controller]")]
    public class StudentsController(IStudentService service, ILogger<StudentsController> logger) : ControllerBase
    {
        private readonly IStudentService _service = service;
        private readonly ILogger<StudentsController> _logger = logger;

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResultDto<StudentViewDto>>> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool sortDesc = false)
        {
            try
            {
                var result = await _service.GetPagedAsync(pageNumber, pageSize, search, sortBy, sortDesc);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged students");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentViewDto>> GetById(int id)
        {
            try
            {
                var student = await _service.GetByIdAsync(id);
                if (student == null)
                {
                    return NotFound();
                }
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student by id: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<StudentViewDto>> Create(StudentCreateDto dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                if (created == null)
                {
                    return BadRequest("Invalid data or RA already exists.");
                }
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating student with RA: {RA}", dto.RA);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, StudentUpdateDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                if (!updated)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
