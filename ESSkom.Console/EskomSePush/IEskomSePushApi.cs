﻿//-----------------------------------------------------------------------
// <copyright file="IEskomSePushApi.cs" company="Richard Smith">
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

    public interface IEskomSePushApi
    {
        Task<IDictionary<string, ESPStatusDto>> GetStatus();

        Task<ESPAreaDto> GetAreaInformation(string areaId);

        Task<ESPAllowanceDto> GetAllowance();
    }
}
