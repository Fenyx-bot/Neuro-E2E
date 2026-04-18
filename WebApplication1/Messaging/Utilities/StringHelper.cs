using System.Security.Cryptography;

namespace Messaging.Utilities;

public static class StringHelper
{
    public static string Hash(this string input) => BCrypt.Net.BCrypt.HashPassword(input);
    public static bool Validate(this string input, string hash) => BCrypt.Net.BCrypt.Verify(input, hash);
}