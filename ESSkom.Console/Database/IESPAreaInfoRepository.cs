//-----------------------------------------------------------------------
// <copyright file="IESPAreaInfoRepository.cs" company="Richard Smith">
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

    public interface IESPAreaInfoRepository
    {
        Task<DateTime> GetLatestTimestamp();

        Task<IEnumerable<ESPAreaInfo>> GetAll();

        Task<ESPAreaInfo> Insert(ESPAreaInfo areaInfo);
    }
}
