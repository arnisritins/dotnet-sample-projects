using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using NSwag.Generation.WebApi;

namespace NSwagProject.WebApi.Controllers
{
    public class SwaggerController : ApiController
    {
        [HttpGet]
        [Route("swagger")]
        public async Task<IHttpActionResult> GetSwagger()
        {
            var settings = new WebApiOpenApiDocumentGeneratorSettings
            {
                Title = "Math API",
                Version = "v1",
                Description = "Provides basic mathematical operations.",
            };

            var generator = new WebApiOpenApiDocumentGenerator(settings);

            var controllers = typeof(WebApiApplication).Assembly
                .GetTypes()
                .Where(t => typeof(ApiController).IsAssignableFrom(t))
                .Where(t => !t.IsAbstract)
                .Where(t => t != typeof(SwaggerController))
                .ToArray();

            var document = await generator
                .GenerateForControllersAsync(controllers);

            var json = document.ToJson();

            var message = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
            };

            return new ResponseMessageResult(message);
        }
    }
}
