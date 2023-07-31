//-----------------------------------------------------------------------
// <copyright file="ESPStatusNextStage.cs" company="Richard Smith">
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

    public class ESPStatusNextStage
    {
        public int Id { get; set; }

        public int ESPStatusId { get; set; }

        public int Stage { get; set; }

        public DateTime StageStartTimestamp { get; set; }

        public ESPStatus Status { get; set; }

        public static ESPStatusNextStage FromDto(ESPStatusDto.NextStageDto dto, ESPStatus status)
        {
            var result = new ESPStatusNextStage()
            {
                Stage = dto.Stage,
                StageStartTimestamp = dto.StageStartTimestamp,
                Status = status,
            };

            return result;
        }
    }
}
