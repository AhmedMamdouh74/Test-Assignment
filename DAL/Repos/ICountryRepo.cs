using Models;

namespace DAL.Repos
{
    public interface ICountryRepo
    {
        Task<bool> AddBlockAsync(CountryBlock block);
        Task<CountryBlock?> GetBlockAsync(string countryCode);
    }
}
