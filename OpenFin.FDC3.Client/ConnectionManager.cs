using Openfin.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFin.FDC3
{
    public class ConnectionManager
    {
        internal static Runtime RuntimeInstance { get; set; }

        /// <summary>
        /// Creates a connected 
        /// </summary>
        /// <param name="windowAlias"></param>
        /// <returns></returns>
        public async static Task<Connection> CreateConnectionAsync(string windowAlias = "")
        {
            var connection = new Connection(windowAlias);
            await connection.Initialize(RuntimeInstance);

            return connection;
        }            
    }
}
