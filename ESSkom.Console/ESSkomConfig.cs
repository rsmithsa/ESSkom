//-----------------------------------------------------------------------
// <copyright file="ESSkomConfig.cs" company="Richard Smith">
//     Copyright (c) Richard Smith. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ESSkom.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ESSkomConfig
    {
        public string ConnectionString { get; set; }

        public string ESPToken { get; set; }

        public string ESPAreaId { get; set; }

        public TimeSpan StatusPollInterval { get; set; }
    }
}
