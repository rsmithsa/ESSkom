//-----------------------------------------------------------------------
// <copyright file="ESPAllowanceDto.cs" company="Richard Smith">
//     Copyright (c) Richard Smith. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ESSkom.Console.EskomSePush
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ESPAllowanceDto
    {
        public int Count { get; set; }

        public int Limit { get; set; }

        public string Type { get; set; }
    }
}
