using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace RecipeBook
{
    public static class MongoConfig
    {
        private static bool _configured = false;
        private static readonly object _lock = new object();

        public static void Configure()
        {
            lock (_lock)
            {
                if (_configured) return; // only run once

                // Tell the driver: serialize all Guids as strings (standard format)
                BsonSerializer.RegisterSerializer(
                    new GuidSerializer(GuidRepresentation.Standard)
                );

                _configured = true;
            }
        }
    }
}