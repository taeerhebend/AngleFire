using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;
using System.Threading.Tasks;

public class ChatHubService : Hub
{
    private readonly IConnectionMultiplexer _redisConnection;

    public ChatHubService(IConnectionMultiplexer redisConnection)
    {
        _redisConnection = redisConnection;
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
        await StoreMessageInRedis(user, message);
    }

    private async Task StoreMessageInRedis(string user, string message)
    {
        var db = _redisConnection.GetDatabase();
        await db.ListRightPushAsync("ChatMessages", $"{user}: {message}");
    }

    public async Task GetMessages()
    {
        var db = _redisConnection.GetDatabase();
        var messages = await db.ListRangeAsync("ChatMessages");
        foreach (var message in messages)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", message);
        }
    }

    public override async Task OnConnectedAsync()
    {
        await GetMessages();
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(System.Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public async Task ClearMessages()
    {
        var db = _redisConnection.GetDatabase();
        await db.KeyDeleteAsync("ChatMessages");
        await Clients.All.SendAsync("MessagesCleared");
    }

    public async Task GetMemory()
    {
        var db = _redisConnection.GetDatabase();
        var memory = await db.ExecuteAsync("MEMORY", "USAGE");
        await Clients.Caller.SendAsync("ReceiveMemory", memory);
    }

    public async Task GetClients()
    {
        var clients = await _redisConnection.GetServer(_redisConnection.GetEndPoints()[0]).ClientListAsync();
        await Clients.Caller.SendAsync("ReceiveClients", clients);
    }

    public async Task GetKeys()
    {
        var db = _redisConnection.GetDatabase();
        var keys = await db.ExecuteAsync("KEYS", "*");
        await Clients.Caller.SendAsync("ReceiveKeys", keys);
    }

    public async Task GetKey(string key)
    {
        var db = _redisConnection.GetDatabase();
        var value = await db.StringGetAsync(key);
        await Clients.Caller.SendAsync("ReceiveKey", value);
    }
}
