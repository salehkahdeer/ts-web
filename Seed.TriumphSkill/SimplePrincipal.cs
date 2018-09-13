using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Seed.TriumphSkill
{
    public class SimplePrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public SimplePrincipal(IIdentity identity)
        {
            this.Identity = identity;
        }

        public bool IsInRole(string role)
        {
            return Identity != null && Identity.IsAuthenticated &&
               !string.IsNullOrWhiteSpace(role) && Roles.IsUserInRole(Identity.Name, role);
        }
    }
}