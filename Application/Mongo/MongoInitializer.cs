using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Conventions;

namespace Application.Mongo
{
    public class MongoInitializer : IDatabaseInitializer
    {
        private bool _initialized;
        private readonly bool _seed;
        private IMongoDatabase _database;

        public MongoInitializer(IMongoDatabase database, IOptions<MongoOption> options)
        {
            _database = database;
            _seed = options.Value.Seed;
        }

        public async Task InitializeAsync()
        {
            if (_initialized)
            {
                return;
            }

            RegisterConventions();
            _initialized = true;
            if (!_seed)
            {
                return;
            }
        }

        private void RegisterConventions()
        {
            ConventionRegistry.Register("reactivitiesConvention", new MongoConvention(), x => true);
        }

        private class MongoConvention : IConventionPack
        {
            public IEnumerable<IConvention> Conventions => new List<IConvention>
            {
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(MongoDB.Bson.BsonType.String),
                new CamelCaseElementNameConvention()
            };
        }
    }
}