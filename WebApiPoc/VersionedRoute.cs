using System.Collections.Generic;
using System.Web.Http.Routing;

namespace WebApiPoc
{
    internal class VersionedRoute : RouteFactoryAttribute
    {
        public VersionedRoute(string template, int allowedVersion): base(template)
        {
            AllowedVersion = allowedVersion;
        }

        public int AllowedVersion {get; private set;}

        public override IDictionary<string, object> Constraints
        {
            get
            {
                return new HttpRouteValueDictionary {{"version", new VersionConstraint(AllowedVersion)}};
            }
        }
    }
}