using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApiPoc.Controllers
{
    public class ETagActionFilterAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (actionContext.Request.Method != HttpMethod.Get) return base.OnActionExecutingAsync(actionContext, cancellationToken);
            
            var dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;
            var etagGenerator = (IETagGenerator) dependencyResolver.GetService(typeof (IETagGenerator));
            var marketRepository = (IMarketRepository) dependencyResolver.GetService(typeof (IMarketRepository));

            var requestEtag = actionContext.Request.Headers.IfNoneMatch;
            var lut = marketRepository.LastUpdateTime;
            var generatedEtag = etagGenerator.GenerateEtag(lut.ToString("o"));

            if (string.Equals(requestEtag.ToString(), generatedEtag.Tag))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.NotModified);
                return base.OnActionExecutingAsync(actionContext, cancellationToken);
            }
            actionContext.Request.Properties.Add("etag", generatedEtag);
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext.Request.Properties.ContainsKey("etag"))
                actionExecutedContext.Response.Headers.ETag = (EntityTagHeaderValue) actionExecutedContext.Request.Properties["etag"];
            actionExecutedContext.Response.Headers.Date = DateTimeOffset.Now;
            
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }
    }
}