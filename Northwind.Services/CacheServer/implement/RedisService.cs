using StackExchange.Redis;

namespace Northwind.Services.CacheServer.implement
{
    public class RedisService : IRedisService
    {
        private readonly Lazy<ConnectionMultiplexer> _connection;
        private readonly IDatabase _db;

        public RedisService(string connectionString)
        {
            // 設定重試時間（以毫秒為單位）
            var retryTimeInMilliseconds = TimeSpan.FromSeconds(5).Milliseconds;
            // 將 connectionString 解析為 StackExchange.Redis 的連線選項
            var options = ConfigurationOptions.Parse(connectionString);
            // 設定 Redis 連線時的重試次數
            options.ConnectRetry = 5;
            // 設定重新連線的 retry policy：每次等 5 秒再重試
            options.ReconnectRetryPolicy = new LinearRetry(retryTimeInMilliseconds);
            // 使用 Lazy 包裝連線建立的動作，只有在第一次使用時才會真正連線
            _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options));
            // 取得預設的 Redis 資料庫（通常是 0 unless 指定 defaultDatabase）
            _db = _connection.Value.GetDatabase();
        }

        public async Task SetStringAsync(string key, string value, TimeSpan? expiry = null)
        {
            await _db.StringSetAsync(key, value, expiry);
        }

        public async Task<string?> GetStringAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public async Task<bool> KeyExistsAsync(string key)
        {
            return await _db.KeyExistsAsync(key);
        }

        public async Task<bool> DeleteKeyAsync(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }
    }
}

