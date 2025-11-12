using BAL.DTOs;
using BAL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Test_Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryBlockService countryBlockService;
        public CountriesController(ICountryBlockService _countryBlockService)
        {
            countryBlockService = _countryBlockService;
        }
        [HttpPost("block")]
        public async Task<IActionResult> BlockCountry([FromBody] AddBlockedCountryDto blockDto)
        {
            if (blockDto == null || string.IsNullOrWhiteSpace(blockDto.CountryCode))
            {
                return BadRequest("Country code is required.");
            }
            var result = await countryBlockService.AddBlockAsync(blockDto);
            if (!result)
            {
                return Conflict("Country is already blocked or invalid country code.");
            }
            return Ok("Country blocked successfully.");
        }
    }
}
