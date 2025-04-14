// TgParser/Extensions/UserExtensions.cs
using TL;

namespace TgParser.Extensions
{
    public static class UserExtensions
    {
        public static string GetDisplayName(this User user)
        {
            return !string.IsNullOrEmpty(user.username)
                ? $"@{user.username}"
                : $"{user.first_name} {user.last_name}".Trim();
        }
    }
}