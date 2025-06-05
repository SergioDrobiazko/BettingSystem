using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.API
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}
