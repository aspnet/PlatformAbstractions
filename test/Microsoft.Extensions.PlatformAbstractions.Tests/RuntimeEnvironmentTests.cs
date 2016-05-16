using System;
using Xunit;

namespace Microsoft.Extensions.PlatformAbstractions.Tests
{
    public class RuntimeEnvironmentTests
    {
        // We can't really use AspNetCore.Testing and ConditionalFact because those are
        // based on this assembly! It would be a weirdly recursive test and it wouldn't
        // show any inconsistencies

#if NET451
        [Fact]
        public void RuntimeType_IsMonoWhenOnMono()
        {
            if(Type.GetType("Mono.Runtime") == null)
            {
                // Skip this test. We're on Desktop CLR
                return;
            }
            Assert.Equal("Mono", new RuntimeEnvironment().RuntimeType);
        }

        [Fact]
        public void RuntimeType_IsCLRWhenOnCLR()
        {
            if(Type.GetType("Mono.Runtime") != null)
            {
                // Skip this test. We're on Mono
                return;
            }
            Assert.Equal("CLR", new RuntimeEnvironment().RuntimeType);
        }
#elif NETCOREAPP1_0
        [Fact]
        public void RuntimeType_IsCoreCLRWhenOnCoreCLR()
        {
            Assert.Equal("CoreCLR", new RuntimeEnvironment().RuntimeType);
        }
#else
#error Added a framework target without adding it to the RuntimeEnvironmentTests!
#endif

        // Everything else in RuntimeEnvironment comes from PlatformApis, which we test separately.
    }
}
