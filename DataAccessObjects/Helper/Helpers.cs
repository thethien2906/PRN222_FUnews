using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;


namespace DataAccessObjects.Helpers
{
    public class PasswordHelper
    {
        // Mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8]; // Tạo salt ngẫu nhiên
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt); // Lấy ngẫu nhiên salt
            }

            // Mã hóa mật khẩu với salt
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Kết hợp salt và hashed password
            return $"{Convert.ToBase64String(salt)}.{hashedPassword}";
        }

        // Kiểm tra mật khẩu khi người dùng đăng nhập
        public static bool VerifyPassword(string storedHashedPassword, string passwordToCheck)
        {
            // Tách salt và hashed password từ chuỗi đã lưu
            var parts = storedHashedPassword.Split('.');
            byte[] salt = Convert.FromBase64String(parts[0]);
            string storedHash = parts[1];

            // Mã hóa mật khẩu nhập vào
            string hashToCheck = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passwordToCheck,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // So sánh hash của mật khẩu đã nhập và mật khẩu trong cơ sở dữ liệu
            return storedHash == hashToCheck;
        }
    }
}