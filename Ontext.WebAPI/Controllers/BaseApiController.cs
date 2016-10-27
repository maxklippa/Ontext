using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Ontext.BLL.Identity;
using Ontext.BLL.ServicesHost.Contracts;

namespace Ontext.WebAPI.Controllers
{
    public class BaseApiController : ApiController
    {
        /// <summary>
        /// The session service.
        /// </summary>
        protected readonly IServicesHost ServicesHost;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiController"/> class.
        /// </summary>
        /// <param name="servicesHost">The Services host.</param>
        public BaseApiController(IServicesHost servicesHost)
        {
            this.ServicesHost = servicesHost;
        }

        /// <summary>
        /// Gets user manager
        /// </summary>
        protected OntextUserManager UserManager
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<OntextUserManager>();
            }
        }
    }
}
