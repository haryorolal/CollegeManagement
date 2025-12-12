using CollegeManagement.Data.Identity;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace CollegeManagement.Data.HelperMethod
{
    public class Generators
    {
        public Generators()
        {
            
        }
        public static (string PasswordHash, string Salt) CreatePasswordHashWithSalt(string passwordd)
        {
            //create the salt
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            //create password hash
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passwordd,
                salt = salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
             ));

            return (hash, Convert.ToBase64String(salt));
        }
        public static string HashPasswordWithExistingSalt(string password, string base64Salt)
        {
            var salt = Convert.FromBase64String(base64Salt);

            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            return hash;
        }

        public async Task<string> GenerateStaffNumber()
        {
            var year = DateTime.Now.Year % 100;
            var random = new Random().Next(100, 999);
            var matGen = $"Staff/{year}{random}";

            return matGen;
        }

        public static string GenerateRandomPassword(int length = 10)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789@$!#?&";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }


    }
}
