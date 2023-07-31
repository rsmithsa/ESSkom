//-----------------------------------------------------------------------
// <copyright file="ESPAreaInfoScheduleStageSlot.cs" company="Richard Smith">
//     Copyright (c) Richard Smith. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ESSkom.Console.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class ESPAreaInfoScheduleStageSlot
    {
        private static readonly Regex SlotParser = new Regex(@"(\d\d):(\d\d)-(\d\d):(\d\d)");

        public int Id { get; set; }

        public int ESPAreaInfoScheduleStageId { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public ESPAreaInfoScheduleStage AreaInfoScheduleStage { get; set; }

        public static List<ESPAreaInfoScheduleStageSlot> FromDto(string[] dto, ESPAreaInfoScheduleStage areaInfoScheduleStage)
        {
            var result = new List<ESPAreaInfoScheduleStageSlot>();

            foreach (var item in dto)
            {
                var m = SlotParser.Match(item);
                if (m.Success)
                {
                    var day = areaInfoScheduleStage.AreaInfoSchedule.Date;
                    var slot = new ESPAreaInfoScheduleStageSlot()
                    {
                        Start = new DateTime(day.Year, day.Month, day.Day, int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), 0),
                        End = new DateTime(day.Year, day.Month, day.Day, int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value), 0),
                        AreaInfoScheduleStage = areaInfoScheduleStage,
                    };

                    result.Add(slot);
                }
            }

            return result;
        }
    }
}
