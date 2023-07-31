//-----------------------------------------------------------------------
// <copyright file="ESPAreaInfoScheduleStage.cs" company="Richard Smith">
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

    public class ESPAreaInfoScheduleStage
    {
        public int Id { get; set; }

        public int ESPAreaInfoScheduleId { get; set; }

        public int Stage { get; set; }

        public List<ESPAreaInfoScheduleStageSlot> StageSlots { get; set; } = new List<ESPAreaInfoScheduleStageSlot>();

        public ESPAreaInfoSchedule AreaInfoSchedule { get; set; }

        public static ESPAreaInfoScheduleStage FromDto((int Stage, string[] Slots) dto, ESPAreaInfoSchedule areaInfoSchedule)
        {
            var result = new ESPAreaInfoScheduleStage()
            {
                Stage = dto.Stage,
                AreaInfoSchedule = areaInfoSchedule,
            };

            result.StageSlots = ESPAreaInfoScheduleStageSlot.FromDto(dto.Slots, result);

            return result;
        }
    }
}
