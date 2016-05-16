// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions.Internal;

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
            RuntimeArchitecture = PlatformApis.GetArch();
        }

        public Platform OperatingSystemPlatform { get; protected set; }

        public string OperatingSystemVersion { get; protected set; }

        public string OperatingSystem { get; protected set; }

        public string RuntimeArchitecture { get; protected set; }

        public string RuntimeType { get; protected set; }

        public string RuntimeVersion { get; protected set; }

        private string GetRuntimeType()
        {
#if NET451
            return Type.GetType("Mono.Runtime") != null ? "Mono" : "CLR";
#else
            return "CoreCLR";
#endif
        }
    }
}
