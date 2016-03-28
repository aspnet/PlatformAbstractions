using System;
using Microsoft.Extensions.PlatformAbstractions;

namespace QuickTest
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                var runtimeEnvironment = PlatformServices.Default.Runtime;
                Console.WriteLine($" OS       : {runtimeEnvironment.OperatingSystem}");
                Console.WriteLine($" Platform : {runtimeEnvironment.OperatingSystemPlatform}");
                Console.WriteLine($" Version  : {runtimeEnvironment.OperatingSystemVersion}");
                Console.WriteLine($" RID      : {runtimeEnvironment.GetRuntimeIdentifier()}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Test Failed: {ex}");
                return 1;
            }
        }
    }
}