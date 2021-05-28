using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using OEEMicroservice.Models.OEE;
using OEEMicroservice.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace OEEMicroservice.IntegrationTests
{
    public class TestFixture<TStartup> : IDisposable
    {
        private readonly TestServer _server;
        public HttpClient Client { get; }

        #region Constructors

        public TestFixture() : this(Path.Combine(""))
        {
        }

        private TestFixture(string relativeTargetProjectParentDir) 
        {
            var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(relativeTargetProjectParentDir, startupAssembly);

            _server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Testing")
                .ConfigureTestServices(InitializeServices)
                .UseContentRoot(contentRoot)
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(contentRoot)
                    .AddJsonFile("appsettings.json")
                    .Build())
                .UseStartup<Startup>());
            Client = _server.CreateClient();
        }
        
        #endregion

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }

        protected virtual void InitializeServices(IServiceCollection services)
        {
            var contextClient = new Mock<IContextClient>();
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
                ProductionBreakDuration = string.Empty,
                ProductionIdealDuration = string.Empty
            }));

            services.AddSingleton(contextClient.Object);
            services.BuildServiceProvider();
        }
        
        private static string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
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
    }
}
