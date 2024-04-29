using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace StudyQuestionForm.Repo
{
    public class MongoDbRepo
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MongoDbRepo> _logger;
        public MongoDbRepo(IConfiguration config, ILogger<MongoDbRepo> logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(IConfiguration));
            _logger = logger ?? throw new ArgumentNullException(nameof(ILogger<MongoDbRepo>));
        }
        protected IMongoCollection<T> InitialiseCollection<T>(string collectionName)
        {
            var databaseName = _config["MongoDbName"].ToString();

            var connectionString = _config.GetConnectionString("MongoDB");

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(string.Format(databaseName));

            var collection = database.GetCollection<T>(collectionName);

            return collection;
        }
    }
}
