using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLogistics.Data
{
    /// <summary>
    /// Handles the database information. This class is injectable
    /// </summary>
    public class DatabaseConfiguration
    {
        /// <summary>
        /// Connection String used to connect to the database.
        /// Data comes from appsettings.json
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
