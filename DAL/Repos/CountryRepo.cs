using Models;
using System.Collections.Concurrent;

namespace DAL.Repos
{
    public class CountryRepo : ICountryRepo
    {
        private readonly ConcurrentDictionary<string, CountryBlock> blocks = new(StringComparer.OrdinalIgnoreCase);
        public Task<bool> AddBlockAsync(CountryBlock block)
        {
            var added = blocks.TryAdd(block.CountryCode.ToUpperInvariant(), block);
            return Task.FromResult(added);
        }

        public Task<bool> RemoveBlockedAsync(string countryCode)
        {
            var removed = blocks.TryRemove(countryCode.ToUpperInvariant(), out _);
            return Task.FromResult(removed);
        }

        public Task<CountryBlock?> GetBlockAsync(string countryCode)
        {
            countryCode = countryCode.ToUpperInvariant();
            blocks.TryGetValue(countryCode, out var block);
            return Task.FromResult(block);
        }

        public Task<IEnumerable<CountryBlock>> GetAllAsync()
        {
           return Task.FromResult(blocks.Values.AsEnumerable());
        }
    }
}
