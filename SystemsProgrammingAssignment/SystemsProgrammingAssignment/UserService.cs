public class UserService
{
    public int MinLength = 4;
    public int MaxLength = 20;
    public string ReturnUsernameRequirements()
    {
        return $"Please enter a username with {MinLength}-{MaxLength} characters, with no spaces and only letters and numbers";
    }
    public bool VerifyUsername(string username,List<User> users)
    {
        bool valid = false;

        if (!string.IsNullOrEmpty(username))
        {
            int len = username.Length;
            if (len <= MaxLength&&len>=MinLength)
            {
                if (username.All(char.IsLetterOrDigit))
                {
                    valid = true;
                }
            }
        }


        if (!valid)
        {
            Console.WriteLine($"Invalid - {ReturnUsernameRequirements()}");
        }
        if (users.Any(user => user.ReturnUsername() == username))
        {
            valid = false;
            Console.WriteLine("Username already taken.");
        }
        if (valid)
        {
            Console.WriteLine("Username accepted!");
        }
        return valid;
    }
    public string GenerateID(List<User> users)
    {
        Random random = new Random();
        string id;
        do
        {
            id = random.Next(0, 1000000000).ToString();
        }
        while (users.Any(user=>user.ReturnUserID()==id));
        return id;
    }

}