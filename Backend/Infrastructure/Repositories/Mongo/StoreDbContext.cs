using Core.Infraestructure.Repositories.MongoDb;
using MongoDB.Driver;

namespace Infrastructure.Repositories.Mongo
{
    internal class StoreDbContext : DbContext
    {
        public StoreDbContext(string connectionString) : base(connectionString)
        {
            MapTypes();
        }

        public override IMongoCollection<T> GetCollection<T>()
        {
            return null;
        }

        private static void MapTypes()
        {
        }
    }
}
