//-----------------------------------------------------------------------
// <copyright file="ESPStatus.cs" company="Richard Smith">
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
    using ESSkom.Console.EskomSePush;

    public class ESPStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Stage { get; set; }

        public DateTime StageUpdated { get; set; }

        public DateTime IngestionTimestamp { get; set; }

        public List<ESPStatusNextStage> NextStages { get; set; } = new List<ESPStatusNextStage>();

        public static ESPStatus FromDto(ESPStatusDto dto)
        {
            var result = new ESPStatus()
            {
                Name = dto.Name,
                Stage = dto.Stage,
                StageUpdated = dto.StageUpdated,
            };

            result.NextStages = dto.NextStages.Select(x => ESPStatusNextStage.FromDto(x, result)).ToList();

            return result;
        }
    }
}
