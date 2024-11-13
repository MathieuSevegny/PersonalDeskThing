using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PersonalDeskThing.App.Core.Models.Options;

namespace PersonalDeskThing.App.Core.Cache
{
    class CacheService
    {
        const string FileName = "cache.json";
        Dictionary<string, object> _cache = [];
        CacheOptions _options;
        string _filePath => Path.Combine(_options.DirectoryPath, FileName);
        public CacheService(IOptions<CacheOptions> options)
        {
            _options = options.Value;
            Directory.CreateDirectory(_options.DirectoryPath);
            Load();
        }
        public bool Load()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    _cache = [];
                    return true;
                }
                string? data = File.ReadAllText(_filePath);
                _cache = JsonConvert.DeserializeObject<Dictionary<string, object>>(data) ?? [];
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public async Task<bool> Save()
        {
            try
            {
                string data = JsonConvert.SerializeObject(_cache);
                await File.WriteAllTextAsync(_filePath, data).ConfigureAwait(true);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public Task<object?> GetValue(string key)
        {
            if (!_cache.TryGetValue(key, out object? value))
            {
                return Task.FromResult<object?>(null);
            }
            return Task.FromResult<object?>(value);
        }

        public async Task SetValue(string key, object value)
        {
            _cache[key] = value;

            await Save().ConfigureAwait(false);
        }
    }
}
