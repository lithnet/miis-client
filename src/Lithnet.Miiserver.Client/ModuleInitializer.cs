using System;
using System.Reflection;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
    }
    private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
        if (!args.Name.StartsWith("PropertySheetBase", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
        else
        {
            return Lithnet.Miiserver.Client.AssemblyProvider.MiisWebServiceAssembly;
        }
    }
}
