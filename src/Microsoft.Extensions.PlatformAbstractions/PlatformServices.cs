// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Extensions.PlatformAbstractions
{
    public abstract class PlatformServices
    {
        private static PlatformServices _defaultPlatformServices = new DefaultPlatformServices();

        public static PlatformServices Default
        {
            get
            {
                return _defaultPlatformServices;
            }
        }

        public abstract IApplicationEnvironment Application { get; }

        public abstract IRuntimeEnvironment Runtime { get; }
        
        public static void SetDefault(PlatformServices defaultPlatformServices)
        {
            _defaultPlatformServices = defaultPlatformServices;
        }
    }
}
