Platform Abstractions (obsolete)
================================

This project is part of ASP.NET Core. You can find samples, documentation and getting started instructions for ASP.NET Core at the [Home](https://github.com/aspnet/home) repo.

This component only shipped in ASP.NET Core 1 and has been removed from ASP.NET Core 2 and up.

### Replacing API usage

The information this API provided maps to the following .NET APIs.

Microsoft.Extensions.PlatformAbstractions | Equivalent .NET API
:-- | :--
ApplicationEnvironment.ApplicationBasePath | `System.AppContext.BaseDirectory` or `System.AppDomain.CurrentDomain.BaseDirectory`
ApplicationEnvironment.ApplicationName  | `System.Reflection.Assembly.GetEntryAssembly().GetName().Name` or `System.AppDomain.CurrentDomain.SetupInformation.ApplicationName`
ApplicationEnvironment.ApplicationVersion  | `System.Reflection.Assembly.GetEntryAssembly().GetName().Version`
ApplicationEnvironment.RuntimeFramework  |  `System.Reflection.Assembly.GetEntryAssembly().GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName` or `System.AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName`
