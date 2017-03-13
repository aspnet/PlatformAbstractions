using System;
using Xunit;

namespace Microsoft.Extensions.PlatformAbstractions.Tests
{
    public class PlatformServicesTests
    {
        [Fact]
        public void ApplictionNameIsNotNullInAppDomain()
        {
            Assert.NotNull(PlatformServices.Default.Application.ApplicationName);
        }
    }
}
