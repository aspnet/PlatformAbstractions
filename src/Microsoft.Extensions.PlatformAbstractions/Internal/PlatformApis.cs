using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Extensions.PlatformAbstractions.Internal
{
    public static class PlatformApis
    {
        private static readonly Lazy<Platform> _platform = new Lazy<Platform>(DetermineOSPlatform);
        private static readonly Lazy<DistroInfo> _distroInfo = new Lazy<DistroInfo>(LoadDistroInfo);

        public static string GetArch()
        {
#if NET451
            return Environment.Is64BitProcess ? "x64" : "x86";
#elif NETSTANDARD1_3
            return RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant();
#else
            return IntPtr.Size == 8 ? "x64" : "x86";
#endif
        }

        public static string GetOSName()
        {
            switch (GetOSPlatform())
            {
                case Platform.Windows:
                    return "Windows";
                case Platform.Linux:
                    return GetDistroId() ?? "Linux";
                case Platform.Darwin:
                    return "Mac OS X";
                default:
                    return "Unknown";
            }
        }

        public static string GetOSVersion()
        {
            switch (GetOSPlatform())
            {
                case Platform.Windows:
                    return NativeMethods.Windows.RtlGetVersion() ?? string.Empty;
                case Platform.Linux:
                    return GetDistroVersionId() ?? string.Empty;
                case Platform.Darwin:
                    return GetDarwinVersion() ?? string.Empty;
                default:
                    return string.Empty;
            }
        }

        private static string GetDarwinVersion() => ConvertDarwinVersionToOSXVersion(NativeMethods.Darwin.GetKernelRelease());

        public static string ConvertDarwinVersionToOSXVersion(string darwinVersion)
        {
            // https://en.wikipedia.org/wiki/Darwin_%28operating_system%29

            Version version;
            if (!Version.TryParse(darwinVersion, out version))
            {
                // If the version is not a valid version number, but we have still detected that it is Darwin, we just assume
                // it is OS X 10.0
                return "10.0";
            }
            else if(version.Major == 1 && version.Minor == 4 && version.Build == 1)
            {
                // 1.4.1 was the first release of 10.1.0
                // Not terribly relevant since we don't run on that but... meh, you never know, and it's easy to add
                return "10.1";
            }
            else if(version.Major < 5)
            {
                // 10.0 covers all versions prior to Darwin 5, except for 1.4.1 (which was 10.1)
                return "10.0";
            }
            else
            {
                // Mac OS X 10.1 mapped to Darwin 5.x, and the mapping continues that way
                // So just subtract 4 from the Darwin version.
                return $"10.{version.Major - 4}";
            }
        }

        public static Platform GetOSPlatform() => _platform.Value;

        private static string GetDistroId() => _distroInfo.Value?.Id;

        private static string GetDistroVersionId() => _distroInfo.Value?.VersionId;

        private static DistroInfo LoadDistroInfo()
        {
            if (File.Exists("/etc/os-release"))
            {
                return DistroInfo.Parse(File.ReadAllText("/etc/os-release"));
            }
            return null;
        }

        // I could probably have just done one method signature and put the #if inside the body but the implementations
        // are just completely different so I wanted to make that clear by putting the whole thing inside the #if.
#if NET451
        private static Platform DetermineOSPlatform()
        {
            var platform = (int)Environment.OSVersion.Platform;
            var isWindows = (platform != 4) && (platform != 6) && (platform != 128);

            if (isWindows)
            {
                return Platform.Windows;
            }
            else
            {
                try
                {
                    var uname = NativeMethods.Unix.GetUname();
                    if (string.Equals(uname, "Darwin", StringComparison.OrdinalIgnoreCase))
                    {
                        return Platform.Darwin;
                    }
                    if (string.Equals(uname, "Linux", StringComparison.OrdinalIgnoreCase))
                    {
                        return Platform.Linux;
                    }
                }
                catch
                {
                }
                return Platform.Unknown;
            }
        }
#else
        private static Platform DetermineOSPlatform()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Platform.Windows;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return Platform.Linux;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return Platform.Darwin;
            }
            return Platform.Unknown;
        }
#endif

        public class DistroInfo
        {
            public string Id { get; }
            public string VersionId { get; }

            public DistroInfo(string id, string versionId)
            {
                Id = id;
                VersionId = versionId;
            }

            public static DistroInfo Parse(string osReleaseFile)
            {
                // Sample os-release file:
                //   NAME="Ubuntu"
                //   VERSION = "14.04.3 LTS, Trusty Tahr"
                //   ID = ubuntu
                //   ID_LIKE = debian
                //   PRETTY_NAME = "Ubuntu 14.04.3 LTS"
                //   VERSION_ID = "14.04"
                //   HOME_URL = "http://www.ubuntu.com/"
                //   SUPPORT_URL = "http://help.ubuntu.com/"
                //   BUG_REPORT_URL = "http://bugs.launchpad.net/ubuntu/"
                // We use ID and VERSION_ID

                string id = null;
                string versionId = null;
                foreach (var line in osReleaseFile.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    if (line.StartsWith("ID=", StringComparison.Ordinal))
                    {
                        id = line.Substring(3).Trim('"', '\'');
                    }
                    else if (line.StartsWith("VERSION_ID=", StringComparison.Ordinal))
                    {
                        versionId = line.Substring(11).Trim('"', '\'');
                    }
                }
                return new DistroInfo(id ?? string.Empty, versionId ?? string.Empty);
            }
        }
    }
}
