using Microsoft.AspNet.Identity;
using System;
using System.Security.Cryptography;
using System.Text;

namespace MvcProject.WebApp.Models.User.Services
{
    public class WebAppPasswordService : IPasswordHasher
    {
        private HashAlgorithm _hashAlgorithm;

        public WebAppPasswordService(HashAlgorithm hashAlgorithm)
        {
            _hashAlgorithm = hashAlgorithm;
        }

        public string HashPassword(string password)
        {
            byte[] data = _hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sBuilder = new StringBuilder();
            foreach (byte element in data)
            {
                sBuilder.Append(element.ToString("x2")); // format each byte as a hexadecimal string
            }
            return sBuilder.ToString();
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashedPassword, HashPassword(providedPassword))
                ? PasswordVerificationResult.Success
                : PasswordVerificationResult.Failed;
        }
    }
}