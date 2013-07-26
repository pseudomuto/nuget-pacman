using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NuGet.PacMan.UnitTests
{
    public class PackageInstaller
    {
        public class Constructor
        {
            public class WhenCalledWithNoArguments
            {
                private PacMan.PackageInstaller _subject = new PacMan.PackageInstaller();

                [Fact]
                public void CreatesADefaultPackageRepo()
                {
                    Assert.Equal(1, this._subject.Repos.Count());          
                }

                [Fact]
                public void SetSourceToNuGetV2()
                {
                    Assert.Equal(
                        "https://packages.nuget.org/api/v2/", 
                        this._subject.Repos.ElementAt(0).Source
                    );
                }
            }

            public class WhenGivenSourceUris
            {
                private PacMan.PackageInstaller _subject;

                public WhenGivenSourceUris()
                {
                    var uris = new Uri[] {
                        new Uri("http://nuget.smdg.ca/nuget")
                    };

                    this._subject = new PacMan.PackageInstaller(uris);
                }

                [Fact]
                public void GuardsAgainstNullArgument()
                {
                    Assert.Throws<ArgumentNullException>(() =>
                    {
                        new PacMan.PackageInstaller(null);
                    });
                }

                [Fact]
                public void AddsRepositories()
                {
                    Assert.Equal(2, this._subject.Repos.Count());
                }
            }
        }
        
        public class FindPackage
        {
            public abstract class FindPackageTest
            {
                protected IPackage _subject;

                protected FindPackageTest()
                {
                    var repo = this.MakeRepo();

                    var mock = new Mock<PacMan.PackageInstaller>();
                    mock.CallBase = true;
                    mock.Setup(m => m.CreateRepo(It.IsAny<IEnumerable<IPackageRepository>>()))
                        .Returns(() =>
                        {
                            return repo;
                        });

                    this.SetSubject(mock.Object);
                }

                protected abstract IPackageRepository MakeRepo();
                protected abstract void SetSubject(PacMan.PackageInstaller installer);
            }

            [Fact]
            public void GuardAgainstNullPackageId()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    new PacMan.PackageInstaller().FindPackage(string.Empty);
                });
            }

            public class WhenPackageIsNotFound : FindPackageTest
            {
                [Fact]
                public void ReturnsNull()
                {
                    Assert.Null(this._subject);
                }

                protected override IPackageRepository MakeRepo()
                {
                    var repo = new Mock<IPackageRepository>();
                    repo.Setup(m => m.GetPackages())
                        .Returns(() =>
                        {
                            return new IPackage[] { }.AsQueryable();
                        });

                    return repo.Object;
                }

                protected override void SetSubject(PacMan.PackageInstaller installer)
                {
                    this._subject = installer.FindPackage("MoTime.Rotator");
                }
            }

            public class WithIdOnly : FindPackageTest
            {
                [Fact]
                public void ReturnsPackage()
                {
                    Assert.NotNull(this._subject);
                }

                protected override IPackageRepository MakeRepo()
                {
                    var repo = new Mock<IPackageRepository>();
                    repo.Setup(m => m.GetPackages())
                        .Returns(() =>
                        {
                            return new IPackage[] { 
                                new DataServicePackage
                                {
                                    Id = "MoTime.Rotator"
                                }
                            }.AsQueryable();
                        });

                    return repo.Object;
                }

                protected override void SetSubject(PacMan.PackageInstaller installer)
                {
                    this._subject = installer.FindPackage("MoTime.Rotator");
                }
            }

            public class WithIdAndVersion : FindPackageTest
            {
                [Fact]
                public void ReturnsPackage()
                {
                    Assert.NotNull(this._subject);
                }

                protected override IPackageRepository MakeRepo()
                {
                    var repo = new Mock<IPackageRepository>();
                    repo.Setup(m => m.GetPackages())
                        .Returns(() =>
                        {
                            return new IPackage[] { 
                                new DataServicePackage
                                {
                                    Id = "Ektron-802",
                                    Version = "1.0.0"
                                }
                            }.AsQueryable();
                        });

                    return repo.Object;
                }

                protected override void SetSubject(PacMan.PackageInstaller installer)
                {
                    this._subject = installer.FindPackage(
                        "Ektron-802", 
                        new SemanticVersion("1.0.0")
                    );
                }
            }
        }
    }
}
