using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using KPIMicroservice.Models.OEE;
using KPIMicroservice.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace KPIMicroservice.IntegrationTests
{
    public class TestFixture<TStartup> : IDisposable
    {
        public static string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
        {
            var projectName = startupAssembly.GetName().Name;

            var applicationBasePath = AppContext.BaseDirectory;

            var directoryInfo = new DirectoryInfo(applicationBasePath);

            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));

                if (projectDirectoryInfo.Exists)
                    if (new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj")).Exists)
                        return Path.Combine(projectDirectoryInfo.FullName, projectName);
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        }

        private TestServer Server;

        public TestFixture() : this(Path.Combine(""))
        {
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }

        protected virtual void InitializeServices(IServiceCollection services)
        {
            var contextClient = new Mock<ContextClient>();
            contextClient.Setup(x => x.Init()).Verifiable();
            contextClient.Setup(x => x.GetProducts());
            contextClient.Setup(x => x.CreateEntityAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>()
            )).Returns(Task.FromResult(default(object)));
            contextClient.Setup(x => x.UpdateStationMeta(
                It.IsAny<string>(),
                It.IsAny<StationMeta>()
            )).Returns(Task.FromResult(default(object)));
            contextClient.Setup(x => x.GetEntitiesAsync(
                It.IsAny<string>()
            )).Returns(Task.FromResult(new Station
            {
                Metrics = new List<OeeMetric>(),
                ProductionBreakDuration = "00:00:01",
                ProductionIdealDuration = "00:00:55.323",
            }));

            services.AddSingleton<IContextClient>(contextClient.Object);
            services.BuildServiceProvider();
        }

        protected TestFixture(string relativeTargetProjectParentDir) 
        {
            var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(relativeTargetProjectParentDir, startupAssembly);

            Server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Testing")
                .ConfigureTestServices(InitializeServices)
                .UseContentRoot(contentRoot)
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(contentRoot)
                    .AddJsonFile("appsettings.json")
                    .Build())
                .UseStartup<Startup>());
            Client = Server.CreateClient();
        }
    }
}
