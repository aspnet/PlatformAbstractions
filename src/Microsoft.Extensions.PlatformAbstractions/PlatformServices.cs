// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.PlatformAbstractions
{
    public class PlatformServices
    {
        private PlatformServices()
        {
            Application = new ApplicationEnvironment();
            Runtime = new RuntimeEnvironment();
        }

        public static PlatformServices Default { get; } = new PlatformServices();

        public ApplicationEnvironment Application { get; }

        public RuntimeEnvironment Runtime { get; }
    }
}
