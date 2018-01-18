Platform Abstractions
=====================

## This repository is obsolete and no longer used or maintained except for 1.0/1.1 patch builds.

As a result, we're not accepting anymore changes to this project. Please file any new issues on http://github.com/dotnet/cli.

### Replacing API usage

The information this API provided maps to the following .NET APIs.

Microsoft.Extensions.PlatformAbstractions | Equivalent .NET API
:-- | :--
ApplicationEnvironment.ApplicationBasePath | `System.AppContext.BaseDirectory` or `System.AppDomain.CurrentDomain.BaseDirectory`
ApplicationEnvironment.ApplicationName  | `System.Reflection.Assembly.GetEntryAssembly().GetName().Name` or `System.AppDomain.CurrentDomain.SetupInformation.ApplicationName`
ApplicationEnvironment.ApplicationVersion  | `System.Reflection.Assembly.GetEntryAssembly().GetName().Version`
ApplicationEnvironment.RuntimeFramework  |  `System.Reflection.Assembly.GetEntryAssembly().GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName` or `System.AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName`
