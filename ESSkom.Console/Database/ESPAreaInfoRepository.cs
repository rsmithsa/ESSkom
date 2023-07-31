//-----------------------------------------------------------------------
// <copyright file="ESPAreaInfoRepository.cs" company="Richard Smith">
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

    public class ESPAreaInfoRepository : IESPAreaInfoRepository
    {
        private readonly IDatabaseConnectionFactory connectionFactory;
        private readonly ILogger<ESPAreaInfoRepository> logger;

        public ESPAreaInfoRepository(IDatabaseConnectionFactory connectionFactory, ILogger<ESPAreaInfoRepository> logger)
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
FROM ESPAreaInfo
ORDER BY IngestionTimestamp DESC
LIMIT 1";
                this.logger.LogDebug(sql);

                return await connection.QuerySingleOrDefaultAsync<DateTime>(sql);
            }
        }

        public async Task<IEnumerable<ESPAreaInfo>> GetAll()
        {
            using (var connection = this.connectionFactory.GetConnection())
            using (var transaction = await connection.BeginTransactionAsync())
            {
                var aMap = new Dictionary<int, ESPAreaInfo>();
                var sMap = new Dictionary<int, ESPAreaInfoSchedule>();
                var ssMap = new Dictionary<int, ESPAreaInfoScheduleStage>();

                var sql = @"
SELECT
    a.Id, a.Name, a.Region, a.Source, a.IngestionTimestamp,
    e.Id, e.ESPAreaInfoId, e.End, e.Note, e.Start
FROM ESPAreaInfo a
LEFT JOIN ESPAreaInfoEvent e ON e.ESPAreaInfoId = a.Id";
                this.logger.LogDebug(sql);

                await connection.QueryAsync<ESPAreaInfo, ESPAreaInfoEvent, ESPAreaInfo>(
                    sql,
                    (a, e) =>
                    {
                        if (!aMap.TryGetValue(a.Id, out var aExisting))
                        {
                            aExisting = a;
                            aMap[aExisting.Id] = aExisting;
                        }

                        if (e != null)
                        {
                            aExisting.Events.Add(e);
                            e.AreaInfo = aExisting;
                        }

                        return aExisting;
                    },
                    transaction: transaction);

                var sql2 = @"
SELECT
    a.Id, a.Name, a.Region, a.Source, a.IngestionTimestamp,
    s.Id, s.ESPAreaInfoId, s.Date, s.Name,
    ss.Id, ss.ESPAreaInfoScheduleId, ss.Stage,
    sss.Id, sss.ESPAreaInfoScheduleStageId, sss.Start, sss.End
FROM ESPAreaInfo a
LEFT JOIN ESPAreaInfoSchedule s ON s.ESPAreaInfoId = a.Id
LEFT JOIN ESPAreaInfoScheduleStage ss ON ss.ESPAreaInfoScheduleId = s.Id
LEFT JOIN ESPAreaInfoScheduleStageSlot sss ON sss.ESPAreaInfoScheduleStageId = ss.Id";
                this.logger.LogDebug(sql2);

                await connection.QueryAsync<ESPAreaInfo, ESPAreaInfoSchedule, ESPAreaInfoScheduleStage, ESPAreaInfoScheduleStageSlot, ESPAreaInfo>(
                    sql2,
                    (a, s, ss, sss) =>
                    {
                        if (!aMap.TryGetValue(a.Id, out var aExisting))
                        {
                            aExisting = a;
                            aMap[aExisting.Id] = aExisting;
                        }

                        ESPAreaInfoSchedule? sExisting = null;
                        if (s != null && !sMap.TryGetValue(s.Id, out sExisting))
                        {
                            sExisting = s;
                            sMap[sExisting.Id] = sExisting;

                            aExisting.Schedule.Add(s);
                            s.AreaInfo = aExisting;
                        }

                        ESPAreaInfoScheduleStage? ssExisting = null;
                        if (ss != null && !ssMap.TryGetValue(ss.Id, out ssExisting))
                        {
                            ssExisting = ss;
                            ssMap[ssExisting.Id] = ssExisting;

                            sExisting.Stages.Add(ss);
                            ss.AreaInfoSchedule = sExisting;
                        }

                        if (sss != null)
                        {
                            ssExisting.StageSlots.Add(sss);
                            sss.AreaInfoScheduleStage = ssExisting;
                        }

                        return aExisting;
                    },
                    transaction: transaction);

                return aMap.Values;
            }
        }

        public async Task<ESPAreaInfo> Insert(ESPAreaInfo areaInfo)
        {
            using (var connection = this.connectionFactory.GetConnection())
            using (var transaction = await connection.BeginTransactionAsync())
            {
                var timeStamp = DateTime.Now;
                areaInfo.IngestionTimestamp = timeStamp;

                var sql = @"
INSERT INTO ESPAreaInfo (Name, Region, Source, IngestionTimestamp)
VALUES (@Name, @Region, @Source, @IngestionTimestamp)
RETURNING id;";
                this.logger.LogDebug(sql);
                areaInfo.Id = await connection.QuerySingleAsync<int>(sql, areaInfo, transaction);

                foreach (var areaInfoEvent in areaInfo.Events)
                {
                    areaInfoEvent.ESPAreaInfoId = areaInfo.Id;

                    var childSql = @"
INSERT INTO ESPAreaInfoEvent (ESPAreaInfoId, End, Note, Start)
VALUES (@ESPAreaInfoId, @End, @Note, @Start)
RETURNING id;";
                    this.logger.LogDebug(childSql);
                    areaInfoEvent.Id = await connection.QuerySingleAsync<int>(childSql, areaInfoEvent, transaction);
                }

                foreach (var areaInfoSchedule in areaInfo.Schedule)
                {
                    areaInfoSchedule.ESPAreaInfoId = areaInfo.Id;

                    var childSql = @"
INSERT INTO ESPAreaInfoSchedule (ESPAreaInfoId, Date, Name)
VALUES (@ESPAreaInfoId, @Date, @Name)
RETURNING id;";
                    this.logger.LogDebug(childSql);
                    areaInfoSchedule.Id = await connection.QuerySingleAsync<int>(childSql, areaInfoSchedule, transaction);

                    foreach (var areaInfoScheduleStage in areaInfoSchedule.Stages)
                    {
                        areaInfoScheduleStage.ESPAreaInfoScheduleId = areaInfoSchedule.Id;

                        var gchildSql = @"
INSERT INTO ESPAreaInfoScheduleStage (ESPAreaInfoScheduleId, Stage)
VALUES (@ESPAreaInfoScheduleId, @Stage)
RETURNING id;";
                        this.logger.LogDebug(gchildSql);
                        areaInfoScheduleStage.Id = await connection.QuerySingleAsync<int>(gchildSql, areaInfoScheduleStage, transaction);

                        foreach (var areaInfoScheduleStageSlot in areaInfoScheduleStage.StageSlots)
                        {
                            areaInfoScheduleStageSlot.ESPAreaInfoScheduleStageId = areaInfoScheduleStage.Id;

                            var ggchildSql = @"
INSERT INTO ESPAreaInfoScheduleStageSlot (ESPAreaInfoScheduleStageId, Start, End)
VALUES (@ESPAreaInfoScheduleStageId, @Start, @End)
RETURNING id;";
                            this.logger.LogDebug(ggchildSql);
                            areaInfoScheduleStageSlot.Id = await connection.QuerySingleAsync<int>(ggchildSql, areaInfoScheduleStageSlot, transaction);
                        }
                    }
                }

                await transaction.CommitAsync();

                return areaInfo;
            }
        }
    }
}
