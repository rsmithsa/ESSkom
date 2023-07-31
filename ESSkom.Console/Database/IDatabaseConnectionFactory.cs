//-----------------------------------------------------------------------
// <copyright file="IDatabaseConnectionFactory.cs" company="Richard Smith">
//     Copyright (c) Richard Smith. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ESSkom.Console.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IDatabaseConnectionFactory
    {
        DbConnection GetConnection();
    }
}
