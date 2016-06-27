using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SimpleInjector;

namespace WebApiPoc
{
    public class ETagHandler : DelegatingHandler
    {
        private readonly Container _container;

        public ETagHandler(Container container)
        {
            _container = container;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Contains("If-None-Match"))
            {
                var repo = _container.GetInstance<IMarketRepository>();
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotModified));
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}