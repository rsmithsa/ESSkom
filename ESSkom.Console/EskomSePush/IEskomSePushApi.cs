//-----------------------------------------------------------------------
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
        Task GetStatus();

        Task GetInformation(string areaId);

        Task<ESPAllowanceDto> GetAllowance();
    }
}
