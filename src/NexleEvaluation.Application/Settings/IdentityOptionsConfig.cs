namespace NexleEvaluation.Application.Settings
{
    public class IdentityOptionsConfig
    {
        public bool RequiredDigit { get; set; }
        public int RequiredLength { get; set; }
        public bool RequireLowercase { get; set; }
        public int RequiredUniqueChars { get; set; }
        public bool RequireUppercase { get; set; }
        public int MaxFailedAttempts { get; set; }
        public int LockoutTimeSpanInMinutes { get; set; }
        public bool RequireNonAlphanumeric { get; set; }

    }
}