using System.Text;

namespace TinyUrl.Backend.Infrastructure
{
    public static class StringHelper
    {

        public static string GenerateRandomBase62String(int length)
        {
            const string charSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            var randomString = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                var randomChar = charSet[random.Next(charSet.Length)];
                randomString.Append(randomChar);
            }

            return randomString.ToString();
        }
    }
}
