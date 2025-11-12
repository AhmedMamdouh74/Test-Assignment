using BAL.DTOs;

namespace BAL.Services
{
    public interface ICountryBlockService
    {
        Task<bool> AddBlockAsync(AddBlockedCountryDto block);
    }
}
