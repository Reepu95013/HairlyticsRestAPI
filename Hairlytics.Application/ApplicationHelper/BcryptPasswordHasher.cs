using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.ApplicationHelper
{
    public class BcryptPasswordHasher : IPasswordHasher
    {
        private readonly int _workFactor; 

        public BcryptPasswordHasher(int workFactor=12)
        {
           _workFactor = workFactor;
        }
        public string HashPassword(string plainPassword)
        {
            if (string.IsNullOrEmpty(plainPassword))
                throw new ArgumentException("Password cannot be null or empty", nameof(plainPassword));
            // Use BCrypt.Net library

            return BCrypt.Net.BCrypt.HashPassword(plainPassword, _workFactor);
          }

        public bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            if (string.IsNullOrEmpty(plainPassword) || string.IsNullOrEmpty(hashedPassword))
                return false;

            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }
    }
}
