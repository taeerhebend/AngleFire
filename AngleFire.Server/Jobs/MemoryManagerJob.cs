using Microsoft.SemanticKernel;

namespace AngleFire.Server.Jobs
{
    public class MemoryManagerJob
    {
        private readonly RedisMemoryManager redisMemoryManager;
        private readonly MongoDbMemoryManager mongoDbMemoryManager;

        public MemoryManagerJob(RedisMemoryManager redisMemoryManager, MongoDbMemoryManager mongoDbMemoryManager)
        {
            this.redisMemoryManager = redisMemoryManager;
            this.mongoDbMemoryManager = mongoDbMemoryManager;
        }

        public async Task Run()
        {
            // Handle requests to keep chat memory of Redis and MongoDb
            await Task.WhenAll(
                redisMemoryManager.HandleRequests(),
                mongoDbMemoryManager.HandleRequests()
            );
        }
    }
}
