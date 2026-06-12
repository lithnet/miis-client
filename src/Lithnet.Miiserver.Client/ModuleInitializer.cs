using System;
using System.Reflection;
using System.Runtime.CompilerServices;

/// <summary>
/// All code inside the Initialize method is run as soon as the assembly's module is loaded, via a
/// C# module initializer (see <see cref="ModuleInitializerAttribute"/>).
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    [ModuleInitializer]
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
