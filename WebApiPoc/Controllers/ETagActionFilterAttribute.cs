using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Routing;

namespace WebApiPoc.Controllers
{
    public class ETagActionFilterAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (actionContext.Request.Method == HttpMethod.Get)
            {
                var controller = (MarketController) actionContext.ControllerContext.Controller;
                var requestEtag = actionContext.Request.Headers.IfNoneMatch;
                var generatedEtag = controller.ETagGenerator.GenerateEtag(controller.MarketRepository.LastUpdateTime.ToString("o"));

                if (string.Equals(requestEtag.ToString(), generatedEtag.Tag))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.NotModified);
                    return base.OnActionExecutingAsync(actionContext, cancellationToken);
                }
                actionContext.Request.Properties.Add("etag", generatedEtag);
            }
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            if (actionExecutedContext.Request.Properties.ContainsKey("etag"))
                actionExecutedContext.Response.Headers.ETag = (EntityTagHeaderValue) actionExecutedContext.Request.Properties["etag"];
            
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }
    }
}