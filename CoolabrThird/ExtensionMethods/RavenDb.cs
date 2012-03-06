using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CoolabrThird.Models;
using Raven.Client;

namespace Coollabr.Utilities.ExtensionMethods
{
    public static class RavenDb
    {
        public static User GetCurrentUser(this IDocumentSession session)
        {
            var user =
                session.Query<User>().FirstOrDefault(u => u.Email == HttpContext.Current.User.Identity.Name);

            return user;
        }
    }
}
