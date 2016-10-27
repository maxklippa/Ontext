using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Ontext.DAL.Identity
{
    public class OntextRole : IdentityRole<Guid, OntextUserRole>
    {

    }
}