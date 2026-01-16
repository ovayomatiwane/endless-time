using Common.Dtos;
using Common.Dtos.Commands;
using Common.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultantsController (IConsultantsService consultantsService) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllConsultants()
        {
            var consultants = await consultantsService.GetAllAsync();

            return Ok(ApiResponse<List<ConsultantDto>>.Ok(consultants));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateConsultantDto createConsultant)
        {
            var rate = await consultantsService.CreateConsultantAsync(createConsultant);

            return Ok(ApiResponse<ConsultantDto>.Ok(rate));
        }
    }
}
