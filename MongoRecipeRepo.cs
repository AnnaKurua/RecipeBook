using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace RecipeBook
{
    public class MongoRecipeRepo : IRecipeRepo
    {
        // IMongoCollection<Recipe> is the MongoDB equivalent of List<Recipe>
        // except it lives on the database server, not in RAM
        private readonly IMongoCollection<Recipe> _collection;

        public MongoRecipeRepo()
        {
            // MongoClient = the connection to the MongoDB server
            // equivalent of opening a file in JsonFileStore
            var client = new MongoClient(AppSettings.Instance.MongoConnectionString);

            // GetDatabase: gets (or creates) the "RecipeBook" database
            var database = client.GetDatabase(AppSettings.Instance.MongoDatabaseName);

            // GetCollection: gets (or creates) the "recipes" collection
            // a collection is MongoDB's equivalent of a table
            // if "recipes" doesn't exist yet, MongoDB creates it on first write
            _collection = database.GetCollection<Recipe>("recipes");
        }

        // Find with an empty filter = "give me everything"
        // ToList() executes the query and returns results
        public List<Recipe> GetAll() =>
            _collection.Find(Builders<Recipe>.Filter.Empty).ToList();

        // Find where Id matches — returns null if nothing found
        // same result as _recipes.FirstOrDefault(r => r.Id == id)
        public Recipe? GetById(Guid id) =>
            _collection.Find(r => r.Id == id).FirstOrDefault();

        public void Save(Recipe recipe)
        {
            if (recipe == null) return;

            // ReplaceOne: find document with matching Id and replace it entirely
            // IsUpsert = true: if no match found, INSERT instead (this is the upsert)
            // equivalent of the FindIndex/Add logic in JsonRecipeRepo
            _collection.ReplaceOne(
                r => r.Id == recipe.Id,
                recipe,
                new ReplaceOptions { IsUpsert = true }
            );
        }

        public void Delete(Guid id)
        {
            // DeleteOne: finds the first document matching the filter and removes it
            // equivalent of _recipes.RemoveAll(r => r.Id == id)
            _collection.DeleteOne(r => r.Id == id);
        }
        // Notice: no Persist() anywhere — MongoDB writes are automatic
    }
}