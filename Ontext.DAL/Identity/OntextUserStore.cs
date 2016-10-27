using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Ontext.DAL.Context.Contracts;

namespace Ontext.DAL.Identity
{
    public class OntextUserStore : UserStore<OntextUser, OntextRole, Guid, OntextUserLogin, OntextUserRole, OntextUserClaim>
    {
        public OntextUserStore(IOntextDbContext context)
            : base(context.DbContext)
        {
        }
    }
}