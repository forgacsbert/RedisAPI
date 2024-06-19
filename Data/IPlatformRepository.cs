using RedisAPI.Models;

namespace RedisAPI.Data
{
    public interface IPlatformRepository
    {
        void CreatePlatform(Platform platform);

        void DeletePlatform(string id);

        Platform? GetPlatform(string id);

        IEnumerable<Platform?> GetPlatforms();
    }
}