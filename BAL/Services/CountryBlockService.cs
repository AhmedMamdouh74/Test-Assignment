using BAL.DTOs;
using DAL.Repos;
using Microsoft.Extensions.Logging;

namespace BAL.Services
{
    public class CountryBlockService : ICountryBlockService
    {
        private readonly ICountryRepo repo;
        private readonly ILogger<CountryBlockService> logger;
        public CountryBlockService(ICountryRepo _repo, ILogger<CountryBlockService> _logger)
        {
            repo = _repo;
            logger = _logger;
        }
        public async Task<bool> AddBlockAsync(AddBlockedCountryDto block)
        {
            var countryCode = block.CountryCode?.Trim().ToUpperInvariant() ?? "";
            if (string.IsNullOrEmpty(countryCode) || countryCode.Length != 2)
            {
                logger.LogWarning("Invalid country code provided: {CountryCode}", block.CountryCode);
                return false;
            }
            var existing = await repo.GetBlockAsync(countryCode);
            if (existing != null)
            {
                logger.LogInformation("Country code {CountryCode} is already blocked.", countryCode);
                return false;
            }
            var countryBlock = new Models.CountryBlock
            {
                CountryCode = countryCode,
                CountryName = "", // Optionally, you could look up the country name here
                BlockedUntilUtc = null // Set to null for permanent block
            };
            var created = await repo.AddBlockAsync(countryBlock);
            if (!created)
            {

                logger.LogError("Failed to block country code {CountryCode}.", countryCode);
                return false;
            }
            logger.LogInformation("Added permanent block for {Country}", countryCode);
            return true;


        }
    }
}
