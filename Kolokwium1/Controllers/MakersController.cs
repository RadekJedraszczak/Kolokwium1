using Kolokwium1.DTOs;
using Kolokwium1.Services;
using Kolokwium1.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MakersController : ControllerBase
    {
        private readonly IDbService _dbService;

        public MakersController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [Route("makers/{makersid}")]
        [HttpGet]
        public async Task<IActionResult> GetById(int makersId)
        {
            try
            {
                var result = await _dbService.GetMakersDetailsAsync(makersId);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
    }
}
