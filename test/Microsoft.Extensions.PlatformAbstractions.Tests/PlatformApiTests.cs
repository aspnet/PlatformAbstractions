using System.Runtime.InteropServices;
using Microsoft.Extensions.PlatformAbstractions.Internal;
using Xunit;

namespace Microsoft.Extensions.PlatformAbstractions.Tests
{
    public class PlatformApiTests
    {
        [Fact]
        public void GetArch_ReturnsAValue()
        {
            // We can't reliably verify the exact value, but this ensures it returns something non-empty
            Assert.False(string.IsNullOrEmpty(PlatformApis.GetArch()));
        }

        [Fact]
        public void GetOSName_ReturnsAValue()
        {
            // We can't reliably verify the exact value, but this ensures it returns something non-empty
            Assert.False(string.IsNullOrEmpty(PlatformApis.GetOSName()));
        }

        [Fact]
        public void GetOSVersion_ReturnsAValue()
        {
            // We can't reliably verify the exact value, but this ensures it returns something non-empty
            Assert.False(string.IsNullOrEmpty(PlatformApis.GetOSVersion()));
        }

        [Theory]
        // Real versions from https://en.wikipedia.org/wiki/Darwin_%28operating_system%29#Release_history
        [InlineData("0.1", "10.0")]
        [InlineData("0.2", "10.0")]
        [InlineData("1.0", "10.0")]
        [InlineData("1.1", "10.0")]
        [InlineData("1.2.1", "10.0")]
        [InlineData("1.3.1", "10.0")]
        [InlineData("1.4.1", "10.1")]
        [InlineData("5.1", "10.1")]
        [InlineData("5.5", "10.1")]
        [InlineData("6.0.1", "10.2")]
        [InlineData("6.8", "10.2")]
        [InlineData("7.0", "10.3")]
        [InlineData("7.9", "10.3")]
        [InlineData("8.0", "10.4")]
        [InlineData("8.11", "10.4")]
        [InlineData("9.0", "10.5")]
        [InlineData("9.8", "10.5")]
        [InlineData("10.0", "10.6")]
        [InlineData("10.8", "10.6")]
        [InlineData("11.0.0", "10.7")]
        [InlineData("11.4.2", "10.7")]
        [InlineData("12.0.0", "10.8")]
        [InlineData("12.6.0", "10.8")]
        [InlineData("13.0.0", "10.9")]
        [InlineData("13.4.0", "10.9")]
        [InlineData("14.0.0", "10.10")]
        [InlineData("14.5.0", "10.10")]
        [InlineData("15.0.0", "10.11")]
        [InlineData("15.4.0", "10.11")]
        public void ConvertDarwinVersionToOSXVersion_ConvertsVersionCorrectly(string darwinVersion, string expectedOSXVersion)
        {
            Assert.Equal(expectedOSXVersion, PlatformApis.ConvertDarwinVersionToOSXVersion(darwinVersion));
        }

        [Fact]
        public void DistroInfo_Parse_ReturnsExpectedResult_WhenOSReleaseEmpty()
        {
            var distroInfo = PlatformApis.DistroInfo.Parse(string.Empty);

            Assert.Equal(string.Empty, distroInfo.Id);
            Assert.Equal(string.Empty, distroInfo.VersionId);
        }

        [Fact]
        public void DistroInfo_Parse_ReturnsExpectedResult_WhenOSReleaseHasNoRelevantFields()
        {
            const string osReleaseFile = @"NAME=""Ubuntu""
VERSION = ""14.04.3 LTS, Trusty Tahr""
ID_LIKE = debian
PRETTY_NAME = ""Ubuntu 14.04.3 LTS""
HOME_URL = ""http://www.ubuntu.com/""
SUPPORT_URL = ""http://help.ubuntu.com/""
BUG_REPORT_URL = ""http://bugs.launchpad.net/ubuntu/""";
            var distroInfo = PlatformApis.DistroInfo.Parse(osReleaseFile);

            Assert.Equal(string.Empty, distroInfo.Id);
            Assert.Equal(string.Empty, distroInfo.VersionId);
        }

        [Fact]
        public void DistroInfo_Parse_SupportsUnquotedValues()
        {
            const string osReleaseFile = @"NAME=""Ubuntu""
ID=Froble
VERSION_ID=1.20.403.1";
            var distroInfo = PlatformApis.DistroInfo.Parse(osReleaseFile);

            Assert.Equal("Froble", distroInfo.Id);
            Assert.Equal("1.20.403.1", distroInfo.VersionId);
        }

        [Fact]
        public void DistroInfo_Parse_SupportsSingleQuotedValues()
        {
            const string osReleaseFile = @"NAME=""Ubuntu""
ID='Froble'
VERSION_ID='1.20.403.1'";
            var distroInfo = PlatformApis.DistroInfo.Parse(osReleaseFile);

            Assert.Equal("Froble", distroInfo.Id);
            Assert.Equal("1.20.403.1", distroInfo.VersionId);
        }

        [Fact]
        public void DistroInfo_Parse_SupportsDoubleQuotedValues()
        {
            const string osReleaseFile = @"NAME=""Ubuntu""
ID=""Froble""
VERSION_ID=""1.20.403.1""";
            var distroInfo = PlatformApis.DistroInfo.Parse(osReleaseFile);

            Assert.Equal("Froble", distroInfo.Id);
            Assert.Equal("1.20.403.1", distroInfo.VersionId);
        }

        [Fact]
        public void DistroInfo_Parse_SupportsMixedQuotedValues()
        {
            const string osReleaseFile = @"NAME=""Ubuntu""
ID='Froble""
VERSION_ID=""1.20.403.1'''''''''";
            var distroInfo = PlatformApis.DistroInfo.Parse(osReleaseFile);

            Assert.Equal("Froble", distroInfo.Id);
            Assert.Equal("1.20.403.1", distroInfo.VersionId);
        }

#if NETCOREAPP1_0
        [Fact]
        public void GetOSPlatform_ReturnsAppropriateValue()
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Equal(Platform.Windows, PlatformApis.GetOSPlatform());
            }
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Assert.Equal(Platform.Darwin, PlatformApis.GetOSPlatform());
            }
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Assert.Equal(Platform.Linux, PlatformApis.GetOSPlatform());
            }
            else
            {
                Assert.Equal(Platform.Unknown, PlatformApis.GetOSPlatform());
            }
        }

        [Fact]
        public void GetOSName_ReturnsAppropriateValue()
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Equal("Windows", PlatformApis.GetOSName());
            }
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Assert.Equal("Mac OS X", PlatformApis.GetOSName());
            }
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // Can't verify exactly what it will be, since it could be a distro name...
                Assert.NotEqual("Unknown", PlatformApis.GetOSName());
                Assert.NotEqual("Windows", PlatformApis.GetOSName());
                Assert.NotEqual("Mac OS X", PlatformApis.GetOSName());
            }
        }
#endif
    }
}
