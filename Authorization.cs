using System.Linq;


namespace Demo4
{
    internal class Authorization
    {
        public static string authorizationRole;
        public static string GetRole(string login, string password)
        {
            var account = TechnoServiceEntities.GetContext().Account.FirstOrDefault(a => a.Login_ == login && a.Password_ == password);
            if (account != null) return authorizationRole = account.Role_.name_role;
            return null;
        }
    }
}
