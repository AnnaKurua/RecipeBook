using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace RecipeBook
{
    public class MongoUserRepo : IUserRepo
    {
        private readonly IMongoCollection<User> _collection;

        public MongoUserRepo()
        {
            var client = new MongoClient(AppSettings.Instance.MongoConnectionString);
            var database = client.GetDatabase(AppSettings.Instance.MongoDatabaseName);
            _collection = database.GetCollection<User>("users");
        }

        public User? GetUser(Guid id) =>
            _collection.Find(u => u.Id == id).FirstOrDefault();

        public List<User> GetAll() =>
            _collection.Find(Builders<User>.Filter.Empty).ToList();

        // Case-insensitive email search — done in the database this time
        // instead of LINQ in RAM like JsonUserRepo
        public User? GetByEmail(string email)
        {
            // RegularExpression with "i" flag = case insensitive
            // equivalent of StringComparison.OrdinalIgnoreCase in JsonUserRepo
            var filter = Builders<User>.Filter.Regex(
                u => u.Email,
                new MongoDB.Bson.BsonRegularExpression(email, "i")
            );
            return _collection.Find(filter).FirstOrDefault();
        }

        public void Add(User user)
        {
            if (user == null) return;
            // InsertOne: always inserts a new document, never updates
            // mirrors the intent of Add() in JsonUserRepo — no upsert
            _collection.InsertOne(user);
        }

        public void Delete(Guid id) =>
            _collection.DeleteOne(u => u.Id == id);

        public void UpdateUser(User user)
        {
            if (user == null) return;
            // ReplaceOne WITHOUT IsUpsert — update only, never insert
            // if user not found, silently does nothing
            // mirrors the if (index >= 0) behaviour in JsonUserRepo
            _collection.ReplaceOne(u => u.Id == user.Id, user);
        }
    }
}