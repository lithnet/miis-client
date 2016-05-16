using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Lithnet.Miiserver.Client
{
    public static class MiiserverConfig
    {
        private const string baseKeyName = @"SYSTEM\CurrentControlSet\services\FIMSynchronizationService\Parameters";

        private static RegistryKey baseKey;

        private static RegistryKey BaseKey
        {
            get
            {
                if (MiiserverConfig.baseKey == null)
                {
                    MiiserverConfig.baseKey = Registry.LocalMachine.OpenSubKey(MiiserverConfig.baseKeyName);

                    if (MiiserverConfig.baseKey == null)
                    {
                        throw new NotSupportedException("The FIM Synchronization Service is not installed on this machine");
                    }
                }

                return MiiserverConfig.baseKey;
            }
        }

        public static string Path
        {
            get
            {
                return MiiserverConfig.BaseKey.GetValue("Path", null) as string;
            }
        }

        public static string DBServerName
        {
            get
            {
                string serverName = MiiserverConfig.BaseKey.GetValue("Server", "localhost") as string;
                if (string.IsNullOrWhiteSpace(serverName))
                {
                    return "localhost";
                }
                else
                {
                    return serverName;
                }
            }
        }

        public static string DBInstanceName
        {
            get
            {
                return MiiserverConfig.BaseKey.GetValue("SQLInstance", null) as string;
            }
        }

        public static string DBName
        {
            get
            {
                return MiiserverConfig.BaseKey.GetValue("DBName", "FIMSynchronizationService") as string;
            }
        }
    }
}
