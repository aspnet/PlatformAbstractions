using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Extensions.PlatformAbstractions.Tests
{
    public class RIDDerivationTests
    {
        [Theory]

        // "leg" is just a bogus architecture I invented to test the combination logic ;) -anurse

        // Windows
        //  Pre-XP and XP (not supported, so we just return 'win'
        [InlineData(Platform.Windows, "doesn'tmatter", "3.11", "x86", "win-x86")]
        [InlineData(Platform.Windows, "doesn'tmatter", "5.0", "x86", "win-x86")]
        [InlineData(Platform.Windows, "doesn'tmatter", "5.0", "x64", "win-x64")]
        [InlineData(Platform.Windows, "doesn'tmatter", "5.0", "leg", "win-leg")]
        [InlineData(Platform.Windows, "doesn'tmatter", "6.0", "x86", "win-x86")]
        [InlineData(Platform.Windows, "doesn'tmatter", "6.0", "x64", "win-x64")]
        [InlineData(Platform.Windows, "doesn'tmatter", "6.0", "leg", "win-leg")]
        //  Windows 7
        [InlineData(Platform.Windows, "doesn'tmatter", "6.1", "x86", "win7-x86")]
        [InlineData(Platform.Windows, "doesn'tmatter", "6.1", "x64", "win7-x64")]
        [InlineData(Platform.Windows, "doesn'tmatter", "6.1", "leg", "win7-leg")]
        //  Windows 8
        [InlineData(Platform.Windows, "doesn'tmatter", "6.2", "x86", "win8-x86")]
        [InlineData(Platform.Windows, "doesn'tmatter", "6.2", "x64", "win8-x64")]
        [InlineData(Platform.Windows, "doesn'tmatter", "6.2", "leg", "win8-leg")]
        //  Windows 8.1
        [InlineData(Platform.Windows, "doesn'tmatter", "6.3", "x86", "win81-x86")]
        [InlineData(Platform.Windows, "doesn'tmatter", "6.3", "x64", "win81-x64")]
        [InlineData(Platform.Windows, "doesn'tmatter", "6.3", "leg", "win81-leg")]
        //  Windows 10
        [InlineData(Platform.Windows, "doesn'tmatter", "10.0", "x86", "win10-x86")]
        [InlineData(Platform.Windows, "doesn'tmatter", "10.0", "x64", "win10-x64")]
        [InlineData(Platform.Windows, "doesn'tmatter", "10.0", "leg", "win10-leg")]

        // Mac OS X
        [InlineData(Platform.Darwin, "doesn'tmatter", "10.0", "x86", "osx.10.0-x86")]
        [InlineData(Platform.Darwin, "doesn'tmatter", "10.7", "x64", "osx.10.7-x64")]
        [InlineData(Platform.Darwin, "doesn'tmatter", "9.1", "ppc", "osx.9.1-ppc")]
        [InlineData(Platform.Darwin, "doesn'tmatter", "12.1", "leg", "osx.12.1-leg")]

        // Linuxes
        [InlineData(Platform.Linux, "Ubuntu", "10.0", "x86", "ubuntu.10.0-x86")]
        [InlineData(Platform.Linux, "Debian", "10.7", "x64", "debian.10.7-x64")]
        [InlineData(Platform.Linux, "CentOS", "9.1", "ppc", "centos.9.1-ppc")]
        [InlineData(Platform.Linux, "RHEL", "12.1", "leg", "rhel.12.1-leg")]
        [InlineData(Platform.Linux, "A Name With Spaces", "12.1", "leg", "anamewithspaces.12.1-leg")]

        // Arch-less
        [InlineData(Platform.Windows, "doesn'tmatter", "6.1", "", "win7")]
        [InlineData(Platform.Darwin, "doesn'tmatter", "12.1", "", "osx.12.1")]
        [InlineData(Platform.Linux, "Ubuntu", "10.0", "", "ubuntu.10.0")]

        // Version-less
        [InlineData(Platform.Windows, "doesn'tmatter", "", "x64", "win-x64")]
        [InlineData(Platform.Darwin, "doesn'tmatter", "", "x64", "osx-x64")]
        [InlineData(Platform.Linux, "Ubuntu", "", "x64", "ubuntu-x64")]
        public void GetRuntimeIdentifier_ReturnsExpectedValues(Platform platform, string osName, string version, string arch, string expectedRid)
        {
            var env = new TestRuntimeEnvironment()
            {
                OperatingSystemPlatform = platform,
                OperatingSystem = osName,
                OperatingSystemVersion = version,
                RuntimeArchitecture = arch
            };
            Assert.Equal(expectedRid, env.GetRuntimeIdentifier());
        }
    }

    // Makes protected setters public for testing
    internal class TestRuntimeEnvironment : RuntimeEnvironment
    {
        public new Platform OperatingSystemPlatform { get { return base.OperatingSystemPlatform; } set { base.OperatingSystemPlatform = value; } }
        public new string OperatingSystemVersion { get { return base.OperatingSystemVersion; } set { base.OperatingSystemVersion = value; } }
        public new string OperatingSystem { get { return base.OperatingSystem; } set { base.OperatingSystem = value; } }
        public new string RuntimeArchitecture { get { return base.RuntimeArchitecture; } set { base.RuntimeArchitecture = value; } }
    }
}
