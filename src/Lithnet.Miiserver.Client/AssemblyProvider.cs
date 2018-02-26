using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Resolves and loads the synchronization service assemblies required for this library to work
    /// </summary>
    internal static class AssemblyProvider
    {
        private static Assembly miisPropertySheetBaseAssembly;

        private static Assembly miisPSModule;

        /// <summary>
        /// Gets the assembly containing the out-of-box powershell module for the synchronization service
        /// </summary>
        public static Assembly MiisPSModule
        {
            get
            {
                if (AssemblyProvider.miisPSModule != null)
                {
                    return AssemblyProvider.miisPSModule;
                }

                string assyPath = Path.Combine(MiiserverConfig.Path, "UIShell\\Microsoft.DirectoryServices.MetadirectoryServices.Config.dll");
                AssemblyProvider.miisPSModule = Assembly.LoadFrom(assyPath);

                return AssemblyProvider.miisPSModule;
            }
        }

        /// <summary>
        /// Gets the assembly containing the miiserver web service
        /// </summary>
        public static Assembly MiisWebServiceAssembly
        {
            get
            {
                if (AssemblyProvider.miisPropertySheetBaseAssembly != null)
                {
                    return AssemblyProvider.miisPropertySheetBaseAssembly;
                }

                string assyPath = Path.Combine(MiiserverConfig.Path, "UIShell\\PropertySheetBase.dll");
                AssemblyProvider.miisPropertySheetBaseAssembly = Assembly.LoadFrom(assyPath);

                return AssemblyProvider.miisPropertySheetBaseAssembly;
            }
        }

        /// <summary>
        /// Initializes a process that can be executed to export the synchronization service configuration
        /// </summary>
        /// <param name="path">The location to export the configuration to. This must be an empty folder</param>
        /// <returns>A Process object that has not yet been started</returns>
        public static Process InitializeSvrExportProcess(string path)
        {
            ProcessStartInfo info = new ProcessStartInfo
            {
                Arguments = $"\"{path}\" /v",
                CreateNoWindow = true,
                FileName = Path.Combine(MiiserverConfig.Path, "bin\\svrexport.exe"),
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            };

            Process p = new Process
            {
                StartInfo = info
            };

            return p;
        }

        /// <summary>
        /// Initializes a process that can be executed to export a management agent from the synchronization service
        /// </summary>
        /// <param name="maName">The name of the management agent to export</param>
        /// <param name="fileName">The file to export the MA configuration to</param>
        /// <returns>A Process object that has not yet been started</returns>
        public static Process InitializeMAExportProcess(string maName, string fileName)
        {
            ProcessStartInfo info = new ProcessStartInfo
            {
                Arguments = $"\"{maName}\" \"{fileName}\"",
                CreateNoWindow = true,
                FileName = Path.Combine(MiiserverConfig.Path, "bin\\maexport.exe"),
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            };

            Process p = new Process {StartInfo = info};

            return p;
        }
    }
}
