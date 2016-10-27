using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Results;
using Ontext.Core.Enums;
using Ontext.Core.Responses.Base;
using Ontext.Server.Core.Settings;

namespace Ontext.WebAPI.Controllers
{
    [Authorize]
    public class ImagesController : ApiController
    {
        public HttpResponseMessage Get(Guid id)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            var filePath = HostingEnvironment.MapPath(string.Format("{0}{1}{2}", OntextSettings.UploadImageDirectoryPath, id, OntextSettings.UploadImageExtension));
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var image = Image.FromStream(fileStream);
                using (var memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, ImageFormat.Png);
                    result.Content = new ByteArrayContent(memoryStream.ToArray());
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    return result;
                }
            }
        }

        public JsonResult<ApiBaseResponse> Delete(Guid id)
        {
            var result = new ApiBaseResponse();

            try
            {
                var filePath = HostingEnvironment.MapPath(string.Format("{0}{1}{2}", OntextSettings.UploadImageDirectoryPath, id, OntextSettings.UploadImageExtension));
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                result.Status = ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        public async Task<JsonResult<ApiBaseResponse>> Post()
        {
            var result = new ApiBaseResponse();

            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var root = HttpContext.Current.Server.MapPath(OntextSettings.UploadImageDirectoryPath);
                var provider = new CustomMultipartFormDataStreamProvider(root);

                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
            }
            catch (Exception ex)
            {
                result.Status = ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
        {
            public CustomMultipartFormDataStreamProvider(string path)
                : base(path)
            { }

            public override string GetLocalFileName(HttpContentHeaders headers)
            {
                var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
                return name.Replace("\"", string.Empty); //this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped
            }
        }
    }
}
