using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Hashing
{
    public static  class HashingHelper
    {
        //enerates a salt, and computes the hash of the password with the salt.
        public static void HashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hash = new HMACSHA512();//secure of hashing password,function of hash
            //Retrieves the key (secret key) used by the HMACSHA512, it is random
            passwordSalt = hash.Key;
            //Converts the password string  to a byte array using UTF-8 encoding before hashing
            passwordHash = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public static bool VerifyPassword(string password,  byte[] passwordHash,  byte[] passwordSalt)
        {
            using var hash = new HMACSHA512(passwordSalt);

            var hashing=hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            for(int i=0;i<passwordHash.Length; i++)
            {
                if (hashing[i]!= passwordHash[i])
                {
                    return false;
                }
                
            }
            return true;
        }

         



    }
}
