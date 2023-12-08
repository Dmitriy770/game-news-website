using GameNews.Articles.Domain.Models;

namespace GameNews.Articles.Domain.Extensions;

public static class StringExtension
{
    public static RoleModel ToRole(this string role)
    {
        if (Enum.TryParse<RoleModel>(role, out var enumRole))
        {
            return enumRole;
        }
        return RoleModel.Anonymous;
    }
}