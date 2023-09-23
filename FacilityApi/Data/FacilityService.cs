using System;
using MongoDB.Bson;
using MongoDB.Driver;
using FacilityApi.Models;
using System.Text.Json.Serialization;
using System.Net;
using System.Reflection.Emit;
using MongoDB.Driver.Core.Operations;
using Microsoft.Extensions.Options;

namespace FacilityApi.Data
{

    public class FacilityService
    {
        private readonly IMongoCollection<BsonDocument> _facilitiesCollection;

        public FacilityService(
            IOptions<FacilityDatabaseSettings> facilityDatabaseSettings)
        {
            var mongoClient = new MongoClient(
            facilityDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                facilityDatabaseSettings.Value.DatabaseName);

            _facilitiesCollection = mongoDatabase.GetCollection<BsonDocument>(
                facilityDatabaseSettings.Value.CollectionName);
        }

        public async Task<List<Facility>> GetAsync()
        {
            var documents = await _facilitiesCollection.Find(_ => true).ToListAsync();
            var facilities = new List<Facility>();           

            foreach (var document in documents)
            {
                facilities.Add(ConvertBsonToFacility(document));
            }
            return facilities;
        }           

        public async Task<List<Facility>> GetFacilityByTypeAsync(string facilityType)
        {
            var facilityCursor = await _facilitiesCollection.FindAsync(
                Builders<BsonDocument>.Filter.Eq("facilitytype", facilityType));

            var documents = facilityCursor.ToListAsync();

            var facilities = new List<Facility>();

            if (documents == null) return facilities;

            foreach (var document in await documents)
            {
                facilities.Add(ConvertBsonToFacility(document));
            }

            return facilities;
        }

        public async Task<TransactionResult> AddFacitity(Facility facility)
        {            
            //Check if facility already exists

            var filter = Builders<BsonDocument>.Filter.Eq("facilityname", facility.FacilityName);
            var facilityCursor = await _facilitiesCollection.CountDocumentsAsync(filter);

            if (((int)facilityCursor) > 0)
            {
                return TransactionResult.Conflict;
            }

            var document = new BsonDocument
            {
                {"facilityname",facility.FacilityName },
                {"facilitytype", facility.FacilityType },
                {"address", facility.Address },
                {"city", facility.City },
                {"state", facility.State },
                {"zipcode", facility.ZipCode },
                {"latitude", facility.Latitude },
                {"longitude", facility.Longitude }
            };
            try
            {
                await _facilitiesCollection.InsertOneAsync(document);
                if (document["_id"].IsObjectId)
                {
                    return TransactionResult.Success;
                }
                return TransactionResult.BadRequest;
            }
            catch (Exception)
            {
                return TransactionResult.ServerError;
            }
        }

        public async Task<TransactionResult> UpdateFacility(string facilityName, Facility facility)
        {          
            var filter = Builders<BsonDocument>.Filter.Eq("facilityname", facilityName);
            var update = Builders<BsonDocument>.Update
                .Set("facilityname", facility.FacilityName)
                .Set("facilitytype", facility.FacilityType)
                .Set("address", facility.Address)
                .Set("city", facility.City)
                .Set("state", facility.State)
                .Set("zipcode", facility.ZipCode)
                .Set("latitude", facility.Latitude)
                .Set("longitude", facility.Longitude);

            var result = await _facilitiesCollection.UpdateOneAsync(filter, update);

            if (result.MatchedCount == 0)
            {
                return TransactionResult.NotFound;
            }
            if (result.ModifiedCount > 0)
            {
                return TransactionResult.Success;
            }
            return TransactionResult.ServerError;
        }

        public async Task<bool> DeleteFacilityByName(string facilityName)
        {            
            var result = await _facilitiesCollection.DeleteOneAsync(
                Builders<BsonDocument>.Filter.Eq("facilityname", facilityName));
            return result.DeletedCount > 0;

        }

        private Facility ConvertBsonToFacility(BsonDocument document)
        {         
            return new Facility
            {
                FacilityName = document["facilityname"].AsString,
                FacilityType = document["facilitytype"].AsString,
                Address = document["address"].AsString,
                City = document["city"].AsString,
                State = document["state"].AsString,
                ZipCode = document["zipcode"].AsInt32,
                Latitude = document["latitude"].AsDouble,
                Longitude = document["longitude"].AsDouble
            };
        }

    }
}

