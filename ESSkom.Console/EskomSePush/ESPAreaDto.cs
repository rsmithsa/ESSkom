//-----------------------------------------------------------------------
// <copyright file="ESPAreaDto.cs" company="Richard Smith">
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

    public class ESPAreaDto
    {
        public EventDto[] Events { get; set; }

        public InfoDto Info { get; set; }

        public ScheduleDto Schedule { get; set; }

        public class EventDto
        {
            public DateTime End { get; set; }

            public string Note { get; set; }

            public DateTime Start { get; set; }
        }

        public class InfoDto
        {
            public string Name { get; set; }

            public string Region { get; set; }
        }

        public class ScheduleDto
        {
            public DaysDto[] Days { get; set; }

            public string Source { get; set; }

            public class DaysDto
            {
                public DateTime Date { get; set; }

                public string Name { get; set; }

                public string[][] Stages { get; set; }
            }
        }
    }
}
