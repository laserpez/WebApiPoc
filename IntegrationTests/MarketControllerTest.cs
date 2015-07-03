using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using NUnit.Framework;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using WebApiPoc;
using WebApiPoc.App_Start;
using WebApiPoc.Controllers;

namespace IntegrationTests
{
    [TestFixture]
    internal class MarketControllerTest
    {
        private HttpServer _server;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            
            var container = new Container();
            SimpleInjectorWebApiInitializer.InitializeContainer(container);

            var configuration = new HttpConfiguration();
            SimpleInjectorWebApiInitializer.InitializeRoutes(configuration);
            configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            _server = new HttpServer(configuration);
        }

        [TestFixtureTearDown]
        public void TeardownFixture()
        {
            _server.Dispose();
        }

        [TestCase("http://localhost/api/markets")]
        public void shall_return_OK(string url)
        {
            var client = new HttpClient(_server);
            var result = client.GetAsync(url)
                .GetAwaiter()
                .GetResult();

            var markets = result.Content.ReadAsAsync<IEnumerable<Market>>().Result;
            Assert.That(markets, Is.Empty);
        }
    }
}
