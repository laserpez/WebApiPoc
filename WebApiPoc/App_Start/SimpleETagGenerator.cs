using System.Net.Http.Headers;
using WebApiPoc.Controllers;

namespace WebApiPoc
{
    internal class SimpleETagGenerator: IETagGenerator
    {
        public EntityTagHeaderValue GenerateEtag(string text)
        {
            return new EntityTagHeaderValue("\"" + text + "\"");
        }
    }
}
