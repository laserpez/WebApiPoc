using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiPoc.Controllers
{
    [ETagActionFilter]
    [VersionedRoute("api/markets", 1)]
    public class MarketController : ApiController
    {
        private readonly IMarketRepository _marketRepository;
        private readonly IETagGenerator _etagGenerator;

        public IMarketRepository MarketRepository 
        {
            get { return _marketRepository; }
        }

        public IETagGenerator ETagGenerator
        {
            get { return _etagGenerator; }
        }

        public MarketController(IMarketRepository marketRepository, IETagGenerator etagGenerator)
        {
            _marketRepository = marketRepository;
            _etagGenerator = etagGenerator;
        }

        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            var _ = request.CreateResponse(HttpStatusCode.OK, _marketRepository.GetAll());
            return _;
        }

        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            var market = _marketRepository.Get(id);
            return market == null
                ? new HttpResponseMessage(HttpStatusCode.NotFound)
                : request.CreateResponse(HttpStatusCode.OK, market);
        }

        public HttpResponseMessage Post([FromBody]Market value)
        {
            var saved = _marketRepository.Store(value);
            return !saved
                ? new HttpResponseMessage(HttpStatusCode.Conflict)
                : new HttpResponseMessage(HttpStatusCode.Created);
        }

        public HttpResponseMessage Put(int id, [FromBody]Market value)
        {
            var market = _marketRepository.Get(id);
            if (market == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            market.Name = value.Name;
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int id)
        {
            var result = _marketRepository.Delete(id);
            return result
                ? new HttpResponseMessage(HttpStatusCode.Accepted)
                : new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}