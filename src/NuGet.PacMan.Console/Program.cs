using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuGet.PacMan.Console
{
    using console = System.Console;

    class Program
    {
        static void Main(string[] args)
        {
            var manager = new PackageInstaller(
                new Uri("http://nuget.dev.smdg.ca/nuget")
            );
            console.WriteLine("Installing packages...");
            manager.InstallPackage("MoTime.Rotator", Environment.CurrentDirectory);
            
            console.WriteLine("Done.");
            console.Read();
        }
    }
}
