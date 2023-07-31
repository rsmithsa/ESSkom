//-----------------------------------------------------------------------
// <copyright file="IESPStatusRepository.cs" company="Richard Smith">
//     Copyright (c) Richard Smith. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ESSkom.Console.Database
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IESPStatusRepository
    {
        Task<DateTime> GetLatestTimestamp();

        Task<IEnumerable<ESPStatus>> GetAll();

        Task<ICollection<ESPStatus>> Insert(ICollection<ESPStatus> status);
    }
}
