﻿using TeamUpAPI.Models;

namespace TeamUpAPI.Contracts.Requests
{
    public class AddUserRequest
    {
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public int Age { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public List<string>? GamesList { get; set; }
    }
}
