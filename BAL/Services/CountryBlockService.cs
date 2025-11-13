using BAL.DTOs;
using DAL.Repos;
using Microsoft.Extensions.Logging;
using Models;

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
        public async Task<bool> AddBlockAsync(AddBlockedCountryDto blockDto)
        {
            var countryCode = blockDto.CountryCode?.Trim().ToUpperInvariant() ?? "";
            if (string.IsNullOrEmpty(countryCode) || countryCode.Length != 2)
            {
                logger.LogWarning("Invalid country code provided: {CountryCode}", blockDto.CountryCode);
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
                CountryName = "",
                BlockedUntilUtc = null // Set to null for permanent blockDto
            };
            var created = await repo.AddBlockAsync(countryBlock);
            if (!created)
            {

                logger.LogError("Failed to blockDto country code {CountryCode}.", countryCode);
                return false;
            }
            logger.LogInformation("Added permanent blockDto for {Country}", countryCode);
            return true;


        }

        public async Task<PagedResult<CountryBlock>> GetAllAsync(int page, int pageSize, string? search)
        {
            var blockedCountries = await repo.GetAllAsync();
            if (!String.IsNullOrWhiteSpace(search))
            {

                var term = search.Trim().ToUpperInvariant();
                blockedCountries = blockedCountries
                    .Where(c => c.CountryCode.ToUpperInvariant().Contains(term)
                             || (!string.IsNullOrEmpty(c.CountryName) && c.CountryName.ToUpperInvariant().Contains(term)));
            }
            var totalCount = blockedCountries.Count();
            var Items = blockedCountries
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<CountryBlock>
            {
                Items = Items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<bool> RemoveBlockAsync(RemoveBlockedCountryDto removeCountryDto)
        {
            var countryCode = removeCountryDto.CountryCode?.Trim().ToUpperInvariant() ?? "";
            if (string.IsNullOrEmpty(countryCode) || countryCode.Length != 2)
            {
                logger.LogWarning("Invalid country code provided: {CountryCode}", countryCode);
                return false;
            }
            var existing = await repo.GetBlockAsync(countryCode);
            if (existing == null)
            {
                logger.LogInformation("Country code {CountryCode} is not blocked.", countryCode);
                return false;
            }
            var removed = await repo.RemoveBlockedAsync(countryCode);
            if (!removed)
            {
                logger.LogError("Failed to remove blockDto for country code {CountryCode}.", countryCode);
                return false;
            }
            logger.LogInformation("Removed blockDto for {Country}", countryCode);
            return true;


        }
    }
}
