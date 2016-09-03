using System;
using Microsoft.Win32;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Provides access to the synchronization service settings such as database name, host, and installation path
    /// </summary>
    public static class MiiserverConfig
    {
        /// <summary>
        /// The base key where miiserver configuration is stored
        /// </summary>
        private const string BaseKeyName = @"SYSTEM\CurrentControlSet\services\FIMSynchronizationService\Parameters";

        private static RegistryKey baseKey;

        private static RegistryKey BaseKey
        {
            get
            {
                if (MiiserverConfig.baseKey != null)
                {
                    return MiiserverConfig.baseKey;
                }

                MiiserverConfig.baseKey = Registry.LocalMachine.OpenSubKey(MiiserverConfig.BaseKeyName);

                if (MiiserverConfig.baseKey == null)
                {
                    throw new NotSupportedException("The FIM Synchronization Service is not installed on this machine");
                }

                return MiiserverConfig.baseKey;
            }
        }

        /// <summary>
        /// Gets the installation path of the synchronization service
        /// </summary>
        public static string Path => MiiserverConfig.BaseKey.GetValue("Path", null) as string;

        /// <summary>
        /// Gets the name of the database server where the synchronization service database is hosted
        /// </summary>
        public static string DBServerName
        {
            get
            {
                string serverName = MiiserverConfig.BaseKey.GetValue("Server", "localhost") as string;

                return string.IsNullOrWhiteSpace(serverName) ? "localhost" : serverName;
            }
        }

        /// <summary>
        /// Gets the name of the database instance where the synchronization service database is hosted
        /// </summary>
        public static string DBInstanceName => MiiserverConfig.BaseKey.GetValue("SQLInstance", null) as string;

        /// <summary>
        /// Gets the name of the synchronization service database
        /// </summary>
        public static string DBName => MiiserverConfig.BaseKey.GetValue("DBName", "FIMSynchronizationService") as string;
    }
}
