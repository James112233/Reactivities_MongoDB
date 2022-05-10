using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Mongo
{
    public class MongoOption
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public bool Seed { get; set; }
    }
}