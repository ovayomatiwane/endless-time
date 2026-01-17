using Common.Dtos;
using Common.Dtos.Commands;
using Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultantAssignmentsController(IConsultantAssignmentService consultantAssignmentService) : ControllerBase
    {
        [HttpPost("Assign")]
        public async Task<IActionResult> AssignTask([FromBody] AssignTaskDto assignTask)
        {
            var result = await consultantAssignmentService.AssignTaskAsync(assignTask);

            return Ok(ApiResponse<ConsultantAssignmentDto>.Ok(result));
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var result = await consultantAssignmentService.GetByIdAsync(id);

            return Ok(ApiResponse<ConsultantAssignmentDto>.Ok(result));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await consultantAssignmentService.GetConsultantAssignmentsAsync();

            return Ok(ApiResponse<List<ConsultantAssignmentDto>>.Ok(result));
        }
    }
}
