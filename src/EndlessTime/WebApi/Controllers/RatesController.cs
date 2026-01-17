using Common.Dtos;
using Common.Dtos.Commands;
using Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatesController(IRatesService ratesService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await ratesService.GetRatesAsync();

            return Ok(ApiResponse<List<RateDto>>.Ok(result));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateRateDto createRate)
        {
            var rate = await ratesService.CreateRateAsync(createRate);

            return Ok(ApiResponse<RateDto>.Ok(rate));
        }

        [HttpGet("Active")]
        public async Task<IActionResult> GetActive()
        {
            var rate = await ratesService.GetActiveAsync();

            return Ok(ApiResponse<List<RateDto>>.Ok(rate));
        }
    }
}
