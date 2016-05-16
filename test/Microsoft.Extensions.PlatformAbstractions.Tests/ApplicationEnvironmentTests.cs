using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Extensions.PlatformAbstractions.Tests
{
    public class ApplicationEnvironmentTests
    {
        [Fact]
        public void ApplicationBasePath_MatchesExpectedValue()
        {
            var env = new ApplicationEnvironment();

#if NET451
            var expected = (string)AppDomain.CurrentDomain.GetData("APP_CONTEXT_BASE_DIRECTORY") ??
            AppDomain.CurrentDomain.BaseDirectory;
            Assert.Equal(expected, env.ApplicationBasePath);
#else
            Assert.Equal(AppContext.BaseDirectory, env.ApplicationBasePath);
#endif

            var appAsm = Path.Combine(env.ApplicationBasePath, $"{typeof(ApplicationEnvironmentTests).GetTypeInfo().Assembly.GetName().Name}.dll");
            Assert.True(File.Exists(appAsm));
        }

        [Fact]
        public void ApplicationName_IsNameOfEntryPointAssembly()
        {
            var env = new ApplicationEnvironment();

            Assert.Equal("dotnet-test-xunit", env.ApplicationName);
        }

        [Fact]
        public void ApplicationVersion_IsVersionOfEntryPointAssembly()
        {
            var env = new ApplicationEnvironment();

            Assert.Equal(typeof(Xunit.Runner.DotNet.Program).GetTypeInfo().Assembly.GetName().Version.ToString(), env.ApplicationVersion);
        }

        [Fact]
        public void RuntimeFramework_MatchesExpectedValue()
        {
#if NET451
            const string expected = ".NETFramework,Version=v4.5.1";
#else
            const string expected = ".NETCoreApp,Version=v1.0";
#endif

            var env = new ApplicationEnvironment();

            Assert.Equal(expected, env.RuntimeFramework.FullName);
        }
    }
}
