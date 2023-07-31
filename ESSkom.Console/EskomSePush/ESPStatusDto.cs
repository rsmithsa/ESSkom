//-----------------------------------------------------------------------
// <copyright file="ESPStatusDto.cs" company="Richard Smith">
//     Copyright (c) Richard Smith. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ESSkom.Console.EskomSePush
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    public class ESPStatusDto
    {
        public string Name { get; set; }

        [JsonPropertyName("next_stages")]
        public NextStageDto[] NextStages { get; set; }

        public int Stage { get; set; }

        [JsonPropertyName("stage_updated")]
        public DateTime StageUpdated { get; set; }

        public class NextStageDto
        {
            public int Stage { get; set; }

            [JsonPropertyName("stage_start_timestamp")]
            public DateTime StageStartTimestamp { get; set; }
        }
    }
}
