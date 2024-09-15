using finanzas_user_service.Utilities;

namespace finanzas_user_service.Enums;

public enum Roles
{
    Admin = 1,
    User = 2
}

public static class RolesExtensions
{
    public static int GetNumeralRoleByEmail(string email)
    {
        return MailRegexUtility.GetDomain(email).Equals("soleel.cl") ? (int) Roles.Admin : (int) Roles.User;
    }
}