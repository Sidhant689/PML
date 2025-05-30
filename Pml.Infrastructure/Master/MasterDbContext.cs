﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Pml.Shared.Entities.Settings;

namespace Pml.Infrastructure.Master
{
    public class MasterDbContext
    {
        private readonly string _connectionString;

        public MasterDbContext(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.MasterConnectionString;
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
