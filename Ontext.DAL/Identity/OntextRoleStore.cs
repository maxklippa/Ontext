using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Ontext.DAL.Context.Contracts;

namespace Ontext.DAL.Identity
{
    public class OntextRoleStore : RoleStore<OntextRole, Guid, OntextUserRole>
    {
        public OntextRoleStore(IOntextDbContext context)
            : base(context.DbContext)
        {

        }

    }
}