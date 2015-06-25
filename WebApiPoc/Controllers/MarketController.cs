using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebApiPoc.Controllers
{
    [RoutePrefix("api/markets")]
    public class MarketController : ApiController
    {
        private readonly IMarketRepository _marketRepository;

        public MarketController(IMarketRepository marketRepository)
        {
            _marketRepository = marketRepository;
        }

        [Route("")]
        public IEnumerable<Market> Get()
        {
            return _marketRepository.GetAll();
        }

        [Route("{id}")]
        [ResponseType(typeof(Market))]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            var market = _marketRepository.Get(id);
            return market == null
                ? new HttpResponseMessage(HttpStatusCode.NotFound)
                : request.CreateResponse(HttpStatusCode.OK, market);
        }

        // POST api/<controller>
        [Route("{id}")]
        public HttpResponseMessage Post([FromBody]Market value)
        {
            var saved = _marketRepository.Store(value);
            return !saved
                ? new HttpResponseMessage(HttpStatusCode.Conflict)
                : new HttpResponseMessage(HttpStatusCode.Created);
        }

        // PUT api/<controller>/5
        [Route("{id}")]
        public HttpResponseMessage Put(int id, [FromBody]Market value)
        {
            var market = _marketRepository.Get(id);
            if (market == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            market.Name = value.Name;
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // DELETE api/<controller>/5
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var result = _marketRepository.Delete(id);
            return result
                ? new HttpResponseMessage(HttpStatusCode.Accepted)
                : new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}