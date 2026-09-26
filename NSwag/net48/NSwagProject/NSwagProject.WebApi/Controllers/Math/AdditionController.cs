using NSwag.Annotations;
using NSwagProject.WebApi.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace NSwagProject.WebApi.Controllers.Math
{
    public class AdditionController : ApiController
    {
        [HttpGet]
        [Route("math/addition/result")]
        [SwaggerResponse(typeof(AdditionResponse))]
        public IHttpActionResult GetSum(int a, int b)
        {
            return Json(new AdditionResponse
            {
                NumberA = a,
                NumberB = b,
                Sum = a + b,
            });
        }

        [HttpGet]
        [Route("math/addition/export")]
        [SwaggerResponse(typeof(IHttpActionResult))]
        public HttpResponseMessage ExportSum(int a, int b)
        {
            var str = new StringBuilder();

            str.AppendLine("NumberA,NumberB,Sum");
            str.AppendLine($"{a},{b},{a + b}");

            string csv = str.ToString();

            var bytes = Encoding.UTF8.GetBytes(csv);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(bytes),
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Addition.csv",
            };

            return response;
        }
    }
}
