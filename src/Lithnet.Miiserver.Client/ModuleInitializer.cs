using System;
using System.Reflection;

/// <summary>
/// Used by Fody ModuleInit. All code inside the Initialize method is run as soon as the assembly is loaded.
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

    /// <summary>
    /// Resolves the PropertySheetBase assembly location
    /// </summary>
    /// <param name="sender">The sending object</param>
    /// <param name="args">The arguments needed to resolve the assembly location</param>
    /// <returns></returns>
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
