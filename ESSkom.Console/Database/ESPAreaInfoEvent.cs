//-----------------------------------------------------------------------
// <copyright file="ESPAreaInfoEvent.cs" company="Richard Smith">
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

    public class ESPAreaInfoEvent
    {
        public int Id { get; set; }

        public int ESPAreaInfoId { get; set; }

        public DateTime End { get; set; }

        public string Note { get; set; }

        public DateTime Start { get; set; }

        public ESPAreaInfo AreaInfo { get; set; }

        public static ESPAreaInfoEvent FromDto(ESPAreaDto.EventDto dto, ESPAreaInfo areaInfo)
        {
            var result = new ESPAreaInfoEvent()
            {
                End = dto.End,
                Note = dto.Note,
                Start = dto.Start,
                AreaInfo = areaInfo,
            };

            return result;
        }
    }
}
