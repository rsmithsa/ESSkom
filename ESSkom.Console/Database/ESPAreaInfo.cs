//-----------------------------------------------------------------------
// <copyright file="ESPAreaInfo.cs" company="Richard Smith">
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

    public class ESPAreaInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Region { get; set; }

        public string Source { get; set; }

        public DateTime IngestionTimestamp { get; set; }

        public List<ESPAreaInfoEvent> Events { get; set; } = new List<ESPAreaInfoEvent>();

        public List<ESPAreaInfoSchedule> Schedule { get; set; } = new List<ESPAreaInfoSchedule>();

        public static ESPAreaInfo FromDto(ESPAreaDto dto)
        {
            var result = new ESPAreaInfo()
            {
                Name = dto.Info.Name,
                Region = dto.Info.Region,
                Source = dto.Schedule.Source,
            };

            result.Events = dto.Events.Select(x => ESPAreaInfoEvent.FromDto(x, result)).ToList();

            result.Schedule = dto.Schedule.Days.Select(x => ESPAreaInfoSchedule.FromDto(x, result)).ToList();

            return result;
        }
    }
}
