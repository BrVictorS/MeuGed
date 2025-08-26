using MongoDB.Driver;

namespace GedDb.Data
{
    public class MongoDbService
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoDatabase? _database;
        public readonly string _arquivos;
        public MongoDbService(IConfiguration configuration)
        {
            _configuration = configuration;
            var connectionString = _configuration.GetConnectionString("DbConnection");
            _arquivos = _configuration.GetValue<string>("Arquivos");
            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);

        }

        public IMongoDatabase? Database => _database;
    }
}
