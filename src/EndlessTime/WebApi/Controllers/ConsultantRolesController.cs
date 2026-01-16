using Common.Dtos.Commands;
using Common.Dtos;
using Common.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultantRolesController(IConsultantRoleService consultantRoleService) : ControllerBase
    {
        [HttpPost("Assign")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto createRate)
        {
            var consultantRole = await consultantRoleService.AssignRoleAsync(createRate);

            return Ok(ApiResponse<ConsultantRoleDto>.Ok(consultantRole));
        }
    }
}
