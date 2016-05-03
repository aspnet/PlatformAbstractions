// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions.Native;

namespace Microsoft.Extensions.PlatformAbstractions
{
    public class RuntimeEnvironment
    {
        public RuntimeEnvironment()
        {
            OperatingSystem = PlatformApis.GetOSName();
            OperatingSystemVersion = PlatformApis.GetOSVersion();
            OperatingSystemPlatform = PlatformApis.GetOSPlatform();

            RuntimeType = GetRuntimeType();
            RuntimeVersion = typeof(object).GetTypeInfo().Assembly.GetName().Version.ToString();
            RuntimeArchitecture = GetArch();
        }

        public Platform OperatingSystemPlatform { get; }

        public string OperatingSystemVersion { get; }

        public string OperatingSystem { get; }

        public string RuntimeArchitecture { get; }

        public string RuntimeType { get; }

        public string RuntimeVersion { get; }

        private string GetRuntimeType()
        {
#if NET451
            return Type.GetType("Mono.Runtime") != null ? "Mono" : "CLR";
#else
            return "CoreCLR";
#endif
        }

        private static string GetArch()
        {
#if NET451
            return Environment.Is64BitProcess ? "x64" : "x86";
#else
            return IntPtr.Size == 8 ? "x64" : "x86";
#endif
        }

    }
}
