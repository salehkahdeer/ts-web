using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace Seed.TriumphSkill
{
    public class SimpleIdentity : IIdentity
    {
        private FormsAuthenticationTicket ticket;
        private SimpleUser simpleUser;

        public SimpleIdentity(FormsAuthenticationTicket ticket)
        {
            this.ticket = ticket;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            simpleUser = serializer.Deserialize<SimpleUser>(this.ticket.UserData);
        }

        public string Username
        {
            get
            {
                return this.simpleUser.Username;
            }
        }

        public string Name
        {
            get
            {
                return this.simpleUser.Name;
            }
        }

        public string Timezone
        {
            get
            {
                return "Asia/Kolkata";
            }
        }

        public bool IsAdministrator
        {
            get
            {
                return this.simpleUser.IsAdministrator;
            }
        }

        public string AuthenticationType
        {
            get { return "Simple Authentication"; }
        }

        public bool IsAuthenticated
        {
            get
            {
                return (simpleUser != null);
            }
        }
    }
}