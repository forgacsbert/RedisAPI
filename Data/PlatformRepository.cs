using System.Text.Json;
using RedisAPI.Models;
using StackExchange.Redis;

namespace RedisAPI.Data
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly IConnectionMultiplexer _redis;

        public PlatformRepository(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public void CreatePlatform(Platform platform)
        {
            ArgumentNullException.ThrowIfNull(platform);

            var db = _redis.GetDatabase();
            var value = JsonSerializer.Serialize(platform);

            db.HashSet("hashplatform",
            [
                new HashEntry(platform.Id, value),
            ]);
        }

        public void DeletePlatform(string id)
        {
            var db = _redis.GetDatabase();

            db.HashDelete("hashplatform", id);
        }

        public Platform? GetPlatform(string id)
        {
            var db = _redis.GetDatabase();
            var value = db.HashGet("hashplatform", id);

            return value.HasValue ? JsonSerializer.Deserialize<Platform>(value) : null;
        }

        public IEnumerable<Platform?> GetPlatforms()
        {
            var db = _redis.GetDatabase();

            var values = db.HashGetAll("hashplatform");

            return values.Select(value => JsonSerializer.Deserialize<Platform>(value.Value));
        }
    }
}