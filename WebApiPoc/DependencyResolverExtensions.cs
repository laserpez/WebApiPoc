using System.Web.Http.Dependencies;

namespace WebApiPoc
{
    internal static class DependencyResolverExtensions
    {
        public static T Resolve<T>(this IDependencyResolver dependencyResolver)
        {
            return (T) dependencyResolver.GetService(typeof (T));
        }
    }
}