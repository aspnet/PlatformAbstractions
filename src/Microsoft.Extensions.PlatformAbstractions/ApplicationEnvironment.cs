// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

namespace Microsoft.Extensions.PlatformAbstractions
{
    public class ApplicationEnvironment
    {
        public string ApplicationBasePath { get; } = GetApplicationBasePath();

        public string ApplicationName { get; } = GetEntryAssembly()?.GetName().Name ?? GetApplicationDomainName();

        public string ApplicationVersion { get; } = GetEntryAssembly()?.GetName().Version.ToString();

        public FrameworkName RuntimeFramework
        {
            get
            {
                string frameworkName = null;
#if NET46
                // Try the setup information
                frameworkName = AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
#endif
                // Try the target framework attribute
                frameworkName = frameworkName ?? GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;

                // TODO: Use when implemented https://github.com/dotnet/corefx/issues/3049

                return string.IsNullOrEmpty(frameworkName) ? null : new FrameworkName(frameworkName);
            }
        }

        private static string GetApplicationBasePath()
        {
            var basePath = AppContext.BaseDirectory;
            return Path.GetFullPath(basePath);
        }

        private static Assembly GetEntryAssembly()
        {
#if NET46
            return Assembly.GetEntryAssembly();
#elif NETSTANDARD1_3
            // TODO: Remove private reflection when we get this: https://github.com/dotnet/corefx/issues/4146
            var getEntryAssemblyMethod =
                typeof(Assembly).GetMethod("GetEntryAssembly", BindingFlags.Static | BindingFlags.NonPublic) ??
                typeof(Assembly).GetMethod("GetEntryAssembly", BindingFlags.Static | BindingFlags.Public);
            return getEntryAssemblyMethod.Invoke(obj: null, parameters: Array.Empty<object>()) as Assembly;
#else
#error Target frameworks need to be updated.
#endif
        }

        private static string GetApplicationDomainName()
        {
#if NET46
            return AppDomain.CurrentDomain.SetupInformation.ApplicationName;
#else
            return null;
#endif
        }
    }
}
