using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuGet.PacMan
{
    public class PackageInstaller
    {
        private static readonly string DEFAULT_PACKAGE_SOURCE = "https://packages.nuget.org/api/v2";

        private IList<IPackageRepository> _repos = new List<IPackageRepository>();

        public IEnumerable<IPackageRepository> Repos { get { return this._repos; } }

        public PackageInstaller()
            : this(new Uri[] { })
        {            
        }

        public PackageInstaller(params Uri[] packageSources)
        {
            Guard.AgainstNullArgument("packageSources", packageSources);

            foreach (var uri in packageSources)
            {
                this.AddSource(uri.ToString());
            }

            this.AddSource(DEFAULT_PACKAGE_SOURCE);
        }

        public IPackage FindPackage(string packageId, SemanticVersion version = null)
        {
            Guard.AgainstNullArgument("packageId", packageId);

            var repo = this.CreateRepo(this.Repos);

            if (version == null)
            {
                return repo.FindPackage(packageId);
            }

            return repo.FindPackage(packageId, version);
        }

        public void InstallPackage(string packageId, string workingDirectory, 
            SemanticVersion version = null)
        {
            var repo = this.CreateRepo(this.Repos);
            var pm = new PackageManager(repo, workingDirectory);
                        
            if (version != null)
            {
                pm.InstallPackage(packageId, version);
            }
            else
            {
                pm.InstallPackage(packageId);
            }
        }

        public virtual IPackageRepository CreateRepo(IEnumerable<IPackageRepository> repos)
        {
            return new AggregateRepository(repos);
        }

        private void AddSource(string sourceUrl)
        {
            this._repos.Add(
                PackageRepositoryFactory.Default.CreateRepository(sourceUrl)
            );
        }
    }
}
