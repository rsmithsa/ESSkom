//-----------------------------------------------------------------------
// <copyright file="DatabaseConnectionFactory.cs" company="Richard Smith">
//     Copyright (c) Richard Smith. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ESSkom.Console.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Data.Sqlite;
    using Microsoft.Extensions.Options;

    public class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly IOptionsMonitor<ESSkomConfig> config;

        public DatabaseConnectionFactory(IOptionsMonitor<ESSkomConfig> config)
        {
            this.config = config;
        }

        public DbConnection GetConnection()
        {
            var conn = new SqliteConnection(this.config.CurrentValue.ConnectionString);

            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
PRAGMA journal_mode = WAL;
PRAGMA synchronous = normal;
PRAGMA temp_store = memory;

PRAGMA foreign_keys = on;
";
                cmd.ExecuteNonQuery();
            }

            ////pragma mmap_size = 30000000000;
            return conn;
        }
    }
}
