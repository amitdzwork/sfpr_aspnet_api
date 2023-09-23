using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;

namespace FacilityApi.Models
{
    [BsonIgnoreExtraElements]
    public class Facility
	{        

        [JsonPropertyName("facilityname")]
        public string? FacilityName { get; set; }

        [JsonPropertyName("facilitytype")]
        public string? FacilityType { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("zipcode")]
        public int? ZipCode { get; set; }

        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }


    }
}

