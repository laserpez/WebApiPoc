using System.Net.Http.Headers;

namespace WebApiPoc.Controllers
{
    public interface IETagGenerator
    {
        EntityTagHeaderValue GenerateEtag(string text);
    }
}