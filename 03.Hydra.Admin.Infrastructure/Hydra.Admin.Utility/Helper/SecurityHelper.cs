using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Hydra.Admin.Utility.Helper
{
    public static class SecurityHelper
    {
        public static string Md5(string inputStr)
        {
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(inputStr));
            StringBuilder builder = new StringBuilder();
            foreach (byte num in buffer)
            {
                builder.Append(num.ToString("x").PadLeft(2, '0'));
            }
            return builder.ToString();
        }
        public static string SaltWithTwiceMd5(string password, string salt)
        {
            string encryptedPassword = Md5(password);
            string encryptedWithSaltPassword = Md5(encryptedPassword + salt);
            return encryptedWithSaltPassword;
        }
    }
}
