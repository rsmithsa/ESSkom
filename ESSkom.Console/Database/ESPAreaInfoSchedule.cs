//-----------------------------------------------------------------------
// <copyright file="ESPAreaInfoSchedule.cs" company="Richard Smith">
//     Copyright (c) Richard Smith. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ESSkom.Console.Database
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ESSkom.Console.EskomSePush;

    public class ESPAreaInfoSchedule
    {
        public int Id { get; set; }

        public int ESPAreaInfoId { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; }

        public List<ESPAreaInfoScheduleStage> Stages { get; set; } = new List<ESPAreaInfoScheduleStage>();

        public ESPAreaInfo AreaInfo { get; set; }

        public static ESPAreaInfoSchedule FromDto(ESPAreaDto.ScheduleDto.DaysDto dto, ESPAreaInfo areaInfo)
        {
            var result = new ESPAreaInfoSchedule()
            {
                Date = dto.Date,
                Name = dto.Name,
                AreaInfo = areaInfo,
            };

            result.Stages = dto.Stages.Select((x, i) => ESPAreaInfoScheduleStage.FromDto((i + 1, dto.Stages[i]), result)).ToList();

            return result;
        }
    }
}
