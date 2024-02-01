using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;

public class CosmosDbStorageService
{
    private readonly string _databaseName;
    private readonly string _containerName;
    private readonly CosmosClient _cosmosClient;

    public CosmosDbStorageService(IConfiguration configuration)
    {
        _databaseName = configuration["CosmosDb:DatabaseName"];
        _containerName = configuration["CosmosDb:ContainerName"];
        _cosmosClient = new CosmosClient(new Uri($"https://{configuration["CosmosDb:Account"]}.documents.azure.com:443/"), configuration["CosmosDb:PrimaryKey"], new ConnectionPolicy
        {
            ConnectionMode = Microsoft.Azure.Cosmos.ConnectionMode.Direct,
            ConnectionProtocol = Protocol.Tcp,
            MaxPoolSize = 100,
        });
    }

    public async Task SaveContextAsync(SKContext context)
    {
        if (context == null || context.ConversationId == null)
        {
            return;
        }

        var container = _cosmosClient.GetContainer(_databaseName, _containerName);
        var item = new
        {
            id = context.ConversationId,
            content = context.ToString(),
        };

        await container.CreateItemAsync(item, new PartitionKey(context.ConversationId));
    }

    public SKContext GetContextOrNull(string conversationId)
    {
        if (!Guid.TryParse(conversationId, out Guid parsedId))
        {
            return null;
        }

        var container = _cosmosClient.GetContainer(_databaseName, _containerName);
        var itemResponse = container.ReadItemAsync<dynamic>(parsedId.ToString(), new PartitionKey(parsedId)).Result;

        if (itemResponse != null && itemResponse.StatusCode == HttpStatusCode.OK)
        {
            return SKContext.Deserialize((string)((Newtonsoft.Json.Linq.JObject)itemResponse.Resource).content);
        }

        return null;
    }
}