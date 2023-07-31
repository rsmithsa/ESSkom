//-----------------------------------------------------------------------
// <copyright file="ESPStatusRepository.cs" company="Richard Smith">
//     Copyright (c) Richard Smith. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ESSkom.Console.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dapper;
    using Microsoft.Extensions.Logging;

    public class ESPStatusRepository : IESPStatusRepository
    {
        private readonly IDatabaseConnectionFactory connectionFactory;
        private readonly ILogger<ESPStatusRepository> logger;

        public ESPStatusRepository(IDatabaseConnectionFactory connectionFactory, ILogger<ESPStatusRepository> logger)
        {
            this.connectionFactory = connectionFactory;
            this.logger = logger;
        }

        public async Task<DateTime> GetLatestTimestamp()
        {
            using (var connection = this.connectionFactory.GetConnection())
            {
                var sql = @"
SELECT IngestionTimestamp
FROM ESPStatus
ORDER BY IngestionTimestamp DESC
LIMIT 1";
                this.logger.LogDebug(sql);

                return await connection.QuerySingleOrDefaultAsync<DateTime>(sql);
            }
        }

        public async Task<IEnumerable<ESPStatus>> GetAll()
        {
            using (var connection = this.connectionFactory.GetConnection())
            {
                var map = new Dictionary<int, ESPStatus>();

                var sql = @"
SELECT s.Id, s.Name, s.Stage, s.StageUpdated, s.IngestionTimestamp, n.Id, n.ESPStatusId, n.Stage, n.StageStartTimestamp
FROM ESPStatus s
LEFT JOIN ESPStatusNextStage n ON n.ESPStatusId = s.Id";
                this.logger.LogDebug(sql);

                await connection.QueryAsync<ESPStatus, ESPStatusNextStage, ESPStatus>(sql, (s, n) =>
                {
                    if (!map.TryGetValue(s.Id, out var existingRecord))
                    {
                        existingRecord = s;
                        map[existingRecord.Id] = existingRecord;
                    }

                    if (n != null)
                    {
                        existingRecord.NextStages.Add(n);
                        n.Status = existingRecord;
                    }

                    return existingRecord;
                });

                return map.Values;
            }
        }

        public async Task<ICollection<ESPStatus>> Insert(ICollection<ESPStatus> statuses)
        {
            using (var connection = this.connectionFactory.GetConnection())
            using (var transaction = await connection.BeginTransactionAsync())
            {
                var timeStamp = DateTime.Now;
                foreach (var status in statuses)
                {
                    status.IngestionTimestamp = timeStamp;

                    var sql = @"
INSERT INTO ESPStatus (Name, Stage, StageUpdated, IngestionTimestamp)
VALUES (@Name, @Stage, @StageUpdated, @IngestionTimestamp)
RETURNING id;";
                    this.logger.LogDebug(sql);
                    status.Id = await connection.QuerySingleAsync<int>(sql, status, transaction);

                    foreach (var statusNextStage in status.NextStages)
                    {
                        statusNextStage.ESPStatusId = status.Id;

                        var childSql = @"
INSERT INTO ESPStatusNextStage (ESPStatusId, Stage, StageStartTimestamp)
VALUES (@ESPStatusId, @Stage, @StageStartTimestamp)
RETURNING id;";
                        this.logger.LogDebug(childSql);
                        statusNextStage.Id = await connection.QuerySingleAsync<int>(childSql, statusNextStage, transaction);
                    }
                }

                await transaction.CommitAsync();

                return statuses;
            }
        }
    }
}
