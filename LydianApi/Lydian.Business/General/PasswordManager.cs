using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Lydian.Business.General
{
    public class PasswordManager
    {
        public static string CreatePasswordHash(string password)
        {
            var data = Encoding.ASCII.GetBytes(password);

            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(data);

            var hashedPassword = Encoding.ASCII.GetString(md5data);
            return hashedPassword;
        }

        private static bool VerifyPasswordHash(string password, string inputPasswordHash)
        {
            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            var passwordHash = Encoding.ASCII.GetString(md5data);

            if (inputPasswordHash == passwordHash)
            {
                return true;
            }

            return false;
        }

    }
}
