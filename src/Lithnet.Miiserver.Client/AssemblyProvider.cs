using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace Lithnet.Miiserver.Client
{
    internal static class AssemblyProvider
    {
        private static Assembly miisPropertySheetBaseAssembly;

        private static Assembly miisPSModule;

        public static Assembly MiisPSModule
        {
            get
            {
                if (AssemblyProvider.miisPSModule == null)
                {
                    string assyPath = Path.Combine(MiiserverConfig.Path, "UIShell\\Microsoft.DirectoryServices.MetadirectoryServices.Config.dll");
                    AssemblyProvider.miisPSModule = Assembly.LoadFrom(assyPath);
                }

                return AssemblyProvider.miisPSModule;
            }
        }

        public static Assembly MiisWebServiceAssembly
        {
            get
            {
                if (AssemblyProvider.miisPropertySheetBaseAssembly == null)
                {
                    string assyPath = Path.Combine(MiiserverConfig.Path, "UIShell\\PropertySheetBase.dll");
                    AssemblyProvider.miisPropertySheetBaseAssembly = Assembly.LoadFrom(assyPath);
                }

                return AssemblyProvider.miisPropertySheetBaseAssembly;
            }
        }

        public static Process InitializeSvrExportProcess(string path)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = string.Format("\"{0}\" /v", path);
            info.CreateNoWindow = true;
            info.FileName = Path.Combine(MiiserverConfig.Path, "bin\\svrexport.exe");
            info.UseShellExecute = false;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;

            Process p = new Process();
            p.StartInfo = info;

            return p;
        }

        public static Process InitializeMAExportProcess(string maName, string fileName)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = string.Format("\"{0}\" \"{1}\"", maName, fileName);
            info.CreateNoWindow = true;
            info.FileName = Path.Combine(MiiserverConfig.Path, "bin\\maexport.exe");
            info.UseShellExecute = false;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;

            Process p = new Process();
            p.StartInfo = info;

            return p;
        }
    }
}
