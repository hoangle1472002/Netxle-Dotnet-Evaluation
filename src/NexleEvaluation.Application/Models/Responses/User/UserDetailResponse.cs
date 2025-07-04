﻿namespace NexleEvaluation.Application.Models.Responses.User
{
    public class UserDetailResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DisplayName => $"{FirstName} {LastName}";
    }
}
