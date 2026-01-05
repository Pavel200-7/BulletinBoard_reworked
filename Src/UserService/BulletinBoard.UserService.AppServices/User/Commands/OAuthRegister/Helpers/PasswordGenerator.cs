using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;


namespace BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister.Helpers;

public static class PasswordGenerator
{
    private const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
    private const string DigitChars = "0123456789";
    private const string NonAlphanumericChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";
    private const string AllChars = UppercaseChars + LowercaseChars + DigitChars + NonAlphanumericChars;

    /// <summary>
    /// Генерирует пароль, соответствующий требованиям Identity по умолчанию
    /// </summary>
    public static string GeneratePassword()
    {
        using var rng = RandomNumberGenerator.Create();
        var password = new StringBuilder();

        password.Append(GetRandomChar(UppercaseChars, rng));  
        password.Append(GetRandomChar(LowercaseChars, rng));    
        password.Append(GetRandomChar(DigitChars, rng));        
        password.Append(GetRandomChar(NonAlphanumericChars, rng)); 

        for (int i = password.Length; i < 10; i++)
        {
            password.Append(GetRandomChar(AllChars, rng));
        }

        var passwordChars = password.ToString().ToCharArray();
        Shuffle(passwordChars, rng);

        return new string(passwordChars);
    }

    public static string GeneratePassword(
        int requiredLength = 10,
        int requiredUniqueChars = 3,
        bool requireUppercase = true,
        bool requireLowercase = true,
        bool requireDigit = true,
        bool requireNonAlphanumeric = true)
    {
        using var rng = RandomNumberGenerator.Create();
        var password = new StringBuilder();
        var requiredChars = new List<char>();

        if (requireUppercase)
        {
            var ch = GetRandomChar(UppercaseChars, rng);
            password.Append(ch);
            requiredChars.Add(ch);
        }

        if (requireLowercase)
        {
            var ch = GetRandomChar(LowercaseChars, rng);
            password.Append(ch);
            requiredChars.Add(ch);
        }

        if (requireDigit)
        {
            var ch = GetRandomChar(DigitChars, rng);
            password.Append(ch);
            requiredChars.Add(ch);
        }

        if (requireNonAlphanumeric)
        {
            var ch = GetRandomChar(NonAlphanumericChars, rng);
            password.Append(ch);
            requiredChars.Add(ch);
        }

        while (requiredChars.Distinct().Count() < requiredUniqueChars && password.Length < requiredLength)
        {
            var ch = GetRandomChar(AllChars, rng);
            if (!requiredChars.Contains(ch))
            {
                password.Append(ch);
                requiredChars.Add(ch);
            }
        }

        for (int i = password.Length; i < requiredLength; i++)
        {
            password.Append(GetRandomChar(AllChars, rng));
        }

        var passwordChars = password.ToString().ToCharArray();
        Shuffle(passwordChars, rng);

        return new string(passwordChars);
    }


    public static bool ValidatePassword(string password, PasswordOptions options)
    {
        if (string.IsNullOrEmpty(password))
            return false;

        if (password.Length < options.RequiredLength)
            return false;

        if (CountUniqueChars(password) < options.RequiredUniqueChars)
            return false;

        if (options.RequireUppercase && !password.Any(char.IsUpper))
            return false;

        if (options.RequireLowercase && !password.Any(char.IsLower))
            return false;

        if (options.RequireDigit && !password.Any(char.IsDigit))
            return false;

        if (options.RequireNonAlphanumeric && !password.Any(ch => !char.IsLetterOrDigit(ch)))
            return false;

        return true;
    }

    private static char GetRandomChar(string chars, RandomNumberGenerator rng)
    {
        var randomNumber = new byte[1];
        rng.GetBytes(randomNumber);
        return chars[randomNumber[0] % chars.Length];
    }

    private static void Shuffle(char[] array, RandomNumberGenerator rng)
    {
        int n = array.Length;
        while (n > 1)
        {
            var randomNumber = new byte[1];
            rng.GetBytes(randomNumber);
            int k = randomNumber[0] % n;
            n--;
            (array[n], array[k]) = (array[k], array[n]);
        }
    }

    private static int CountUniqueChars(string str)
    {
        return str.Distinct().Count();
    }
}