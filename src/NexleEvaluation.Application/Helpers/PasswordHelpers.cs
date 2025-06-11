namespace NexleEvaluation.Application.Helpers
{
    public static class PasswordHelpers
    {
        public static string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 10);
        }

        public static bool Verify(string password, string cipher)
        {
            return BCrypt.Net.BCrypt.Verify(password, cipher);
        }
    }
}

