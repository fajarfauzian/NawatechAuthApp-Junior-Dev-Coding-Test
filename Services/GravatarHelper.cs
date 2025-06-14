using System.Security.Cryptography;
using System.Text;

namespace NawatechAuthApp.Services
{
    public static class GravatarHelper
    {
        public static string GetGravatarUrl(string email, int size = 200, string defaultImage = "identicon")
        {
            if (string.IsNullOrEmpty(email))
            {
                return GetDefaultGravatarUrl(size, defaultImage);
            }

            // Normalize email: trim whitespace and convert to lowercase
            var normalizedEmail = email.Trim().ToLowerInvariant();
            
            // Create MD5 hash of the email
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(normalizedEmail);
                var hashBytes = md5.ComputeHash(inputBytes);
                var hash = Convert.ToHexString(hashBytes).ToLowerInvariant();
                
                return $"https://www.gravatar.com/avatar/{hash}?s={size}&d={defaultImage}";
            }
        }
        
        public static string GetDefaultGravatarUrl(int size = 200, string defaultImage = "identicon")
        {
            var randomEmail = $"default{DateTime.Now.Ticks}@example.com";
            return GetGravatarUrl(randomEmail, size, defaultImage);
        }
        
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
                
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}