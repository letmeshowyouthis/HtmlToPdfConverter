using HtmlToPdfConverter.DAL.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace HtmlToPdfConverter.DAL.Managers
{
    /// <summary>
    /// Redis-based sessions manager.
    /// </summary>
    public class RedisSessionManager : ISessionManager
    {
        private readonly IDatabase _database;

        public RedisSessionManager(ConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<Session?> FindAsync(Guid id)
        {
            var value = await _database.StringGetAsync($"{RecordKeyPrefix}:{id}");
            return !value.HasValue 
                ? default 
                : JsonConvert.DeserializeObject<Session>(value);
        }

        public async Task<bool> AddOrUpdateAsync(Session session)
        {
            var value = ComputeValue(session);

            return await _database.StringSetAsync(ComputeKey(session), value);
        }

        public async Task RemoveAsync(Session session)
        {
            await _database.KeyDeleteAsync(ComputeKey(session));
        }

        private const string RecordKeyPrefix = "session";
        private static string ComputeKey(Session session) => $"{RecordKeyPrefix}:{session.Id}";

        private static string ComputeValue(Session session) => JsonConvert.SerializeObject(session);
    }
}
