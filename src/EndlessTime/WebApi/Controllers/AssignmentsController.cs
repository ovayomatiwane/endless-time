using Common.Dtos.Commands;
using Common.Dtos;
using Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController(IAssignmentsService assignmentsService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await assignmentsService.GetAssignmentsAsync();

            return Ok(ApiResponse<List<AssignmentDto>>.Ok(result));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateAssignmentDto createAssignment)
        {
            var result = await assignmentsService.CreateAssignmentAsync(createAssignment);

            return Ok(ApiResponse<AssignmentDto>.Ok(result));
        }

        [HttpGet("Unassigned")]
        public async Task<IActionResult> GetUnassigned()
        {
            var result = await assignmentsService.GetUnassignedAsync();

            return Ok(ApiResponse<List<AssignmentDto>>.Ok(result));
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var result = await assignmentsService.GetByIdAsync(id);

            return Ok(ApiResponse<AssignmentDto>.Ok(result));
        }
    }
}
