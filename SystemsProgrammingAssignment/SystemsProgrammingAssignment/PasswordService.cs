using System.Security.Cryptography;

public class PasswordService
{

    public int MinLength = 8;
    public int MaxLength = 20;
    public int MinLetters = 4;
    public int MinNumbers = 2;
    public int MinSpecialChars = 1;


    private int SaltSize = 16;
    private int HashSize = 32;
    private int Iterations = 60000;

    public string ReturnPasswordRequirements()
    {
        return $"Please enter a password with no spaces, between {MinLength} - {MaxLength} characters, atleast {MinLetters} letters, {MinNumbers} numbers, and {MinSpecialChars} special character(s).";
    }
    public bool VerifyPassword( string password)
    {
        bool valid  = false;
        if (!string.IsNullOrWhiteSpace(password))
        {
            int len = password.Length;
            int lettersCount = password.Count(char.IsLetter);
            int numbersCount = password.Count(char.IsDigit);
            int specialCharsCount = password.Count(c=>!char.IsLetterOrDigit(c));
            if (len >= MinLength && len <= MaxLength && lettersCount >= MinLetters && numbersCount >= MinNumbers && specialCharsCount >= MinSpecialChars)
            {
                if (password.All(c=> (c>=33 &&c<=126) || c=='£'))
                { 
                    valid = true;
                }
            }
        }
        if (!valid)
        {
            Console.WriteLine($"Invalid - {ReturnPasswordRequirements()}");
        }
        else
        {
            Console.WriteLine("Password Accepted!");
        }
        return valid;
    }
    public string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }
    public bool VerifyHashedPassword(string password,string storedHash)
    {
        string[] parts = storedHash.Split('.');

        int iterations = int.Parse(parts[0]);
        byte[] salt = Convert.FromBase64String(parts[1]);
        byte[] ogHash = Convert.FromBase64String(parts[2]);

        byte[] newHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, ogHash.Length);
        return CryptographicOperations.FixedTimeEquals(ogHash, newHash);
    }
}