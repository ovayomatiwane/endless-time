using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultantsController (IConsultantsService consultantsService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllConsultants()
        {
            var consultants = await consultantsService.GetAllAsync();
            return Ok(consultants.ToList());
        }
    }
}
