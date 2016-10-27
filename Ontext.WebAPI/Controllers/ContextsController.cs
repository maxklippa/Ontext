using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Ontext.BLL.Providers.Contracts;
using Ontext.BLL.ServicesHost.Contracts;
using Ontext.Core.Responses;

namespace Ontext.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Contexts")]
    public class ContextsController : BaseApiController
    {
        public ContextsController(IServicesHost servicesHost) : base(servicesHost)
        {
        }

        [Route("GetContexts")]
        [HttpGet]
        public JsonResult<ApiContextResponse> GetContexts()
        {
            var result = new ApiContextResponse();

            try
            {
                result.Items = ServicesHost.GetService<IContextsProvider>().GetAll().ToList();
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("GetLastModifiedContexts")]
        [HttpGet]
        public JsonResult<ApiContextResponse> GetLastModifiedContexts(long lastModifiedDateTicks)
        {
            var result = new ApiContextResponse();

            try
            {
                var lastModifiedDate = new DateTime(lastModifiedDateTicks);
                result.Items = ServicesHost.GetService<IContextsProvider>().GetLastModifiedContexts(lastModifiedDate).ToList();
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

    }
}
