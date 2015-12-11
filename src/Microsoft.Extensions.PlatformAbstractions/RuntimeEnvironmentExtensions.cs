using System;

namespace Microsoft.Extensions.PlatformAbstractions
{
    public static class RuntimeEnvironmentExtensions
    {
        private static readonly string OverrideEnvironmentVariableName = "DOTNET_RUNTIME_ID";
        private static readonly string LegacyOverrideEnvironmentVariableName = "DNX_RUNTIME_ID";

        public static string GetRuntimeIdentifier(this IRuntimeEnvironment self)
        {
            return
                Environment.GetEnvironmentVariable(OverrideEnvironmentVariableName) ??
                Environment.GetEnvironmentVariable(LegacyOverrideEnvironmentVariableName) ??
                (GetRIDOS(self) + GetRIDVersion(self) + GetRIDArch(self));
        }

        private static string GetRIDArch(IRuntimeEnvironment self)
        {
            if(!string.IsNullOrEmpty(self.RuntimeArchitecture))
            {
                return $"-{self.RuntimeArchitecture.ToLowerInvariant()}";
            }
            return string.Empty;
        }

        private static string GetRIDVersion(IRuntimeEnvironment self)
        {
            // Windows RIDs do not separate OS name and version by "." due to legacy
            // Others do, that's why we have the "." prefix on them below
            switch (self.OperatingSystemPlatform)
            {
                case Platform.Windows:
                    return GetWindowsProductVersion(self);
                case Platform.Linux:
                    return $".{self.OperatingSystemVersion}";
                case Platform.Darwin:
                    return $".{self.OperatingSystemVersion}";
                default:
                    return string.Empty; // Unknown Platform? Unknown Version!
            }
        }

        private static string GetWindowsProductVersion(IRuntimeEnvironment self)
        {
            var ver = Version.Parse(self.OperatingSystemVersion);
            if (ver.Major == 6)
            {
                if (ver.Minor == 1)
                {
                    return "7";
                }
                else if (ver.Minor == 2)
                {
                    return "8";
                }
                else if (ver.Minor == 3)
                {
                    return "81";
                }
            }
            else if (ver.Major == 10 && ver.Minor == 0)
            {
                // Not sure if there will be  10.x (where x > 0) or even 11, so let's be defensive.
                return "10";
            }
            return string.Empty; // Unknown version
        }

        private static string GetRIDOS(IRuntimeEnvironment self)
        {
            switch (self.OperatingSystemPlatform)
            {
                case Platform.Windows:
                    return "win";
                case Platform.Linux:
                    return self.OperatingSystem.ToLowerInvariant();
                case Platform.Darwin:
                    return "osx";
                default:
                    return "unknown";
            }
        }
    }
}
