#if !NET5_0_OR_GREATER
using System;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Polyfill for <c>System.Runtime.CompilerServices.ModuleInitializerAttribute</c>, which is not
    /// present in the .NET Framework class library. The C# 9 (and later) compiler recognises any type
    /// with this exact full name to mark a static, parameterless, void method as a module initializer,
    /// so defining it ourselves enables the language feature on .NET Framework without any dependency.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class ModuleInitializerAttribute : Attribute
    {
    }
}
#endif
