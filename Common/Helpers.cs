using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public static class Helpers
    {
        public static string GetStringHash(string s)
        {
            using var hashAlgorithm = SHA512.Create();
            var hash = hashAlgorithm.ComputeHash(Encoding.Unicode.GetBytes(s));
            return string.Concat(hash.Select(item => item.ToString("x2")));
        }

        public static bool IsPasswordValid(string password)
        {
            if (password.Length < 6)
            {
                return false;
            }
            if (!password.Any(char.IsUpper))
            {
                return false;
            }
            if (!password.Any(char.IsLower))
            {
                return false;
            }
            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            return true;
        }
    }
}
