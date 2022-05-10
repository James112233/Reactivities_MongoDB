using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models
{
    public class Activities : IMongoCommon
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public bool IsCancelled { get; set; } = false;
        public bool IsDelete { get; set; } = false;
    }
}