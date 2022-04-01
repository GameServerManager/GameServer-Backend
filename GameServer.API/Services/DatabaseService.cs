using GameServer.API.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace GameServer.API.Services
{
    public class DatabaseService : IDatabaseService
    {
        public IMongoCollection<User> UserCollection { get; private set; }
        private readonly MongoClient _dbClient;
        private readonly string _connectionString;
        public DatabaseService()
        {
            DataProviderSettings settings = new DataProviderSettings()
            {
                UserName = "root",
                Password = "example"
            };
            _connectionString = $"mongodb://{settings.UserName}:{settings.Password}@{settings.Host}:{settings.Port}/";
            _dbClient = new MongoClient(_connectionString);
            Connect();
        }

        public void Connect()
        {
            InitUserDatabase();
        }

        private void InitUserDatabase()
        {
            BsonClassMap.RegisterClassMap<User>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Username));
            });

            string serverDatabaseName = "User";
            string serverCollectionName = "UserEntitys";

            var db = _dbClient.GetDatabase(serverDatabaseName);
            UserCollection = db.GetCollection<User>(serverCollectionName);
        }

        public async Task<User> GetUser(string id)
        {
            var filter = Builders<User>.Filter.Eq(server => server.Username, id);

            var user = await UserCollection.FindAsync(filter);
            return await user.FirstAsync();
        }

        public async Task SaveNewUser(User user)
        {
            await UserCollection.InsertOneAsync(user);
        }
    }
}
