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
        internal static List<string> connectionAliases = new List<string>();

        /// <summary>
        /// Creates a connected 
        /// </summary>
        /// <param name="connectionAlias"></param>
        /// <returns></returns>
        public async static Task<Connection> CreateConnectionAsync(string connectionAlias)
        {
            if (string.IsNullOrEmpty(connectionAlias))
                throw new ArgumentNullException("connectionAlias", "A connection alias is required");

            if (connectionAliases.Contains(connectionAlias))
                throw new ArgumentNullException("connectionAlias", "A connection with this alias has already been established");

            var connection = new Connection(connectionAlias);
            await connection.Initialize(RuntimeInstance);

            return connection;
        }      
    }
}
