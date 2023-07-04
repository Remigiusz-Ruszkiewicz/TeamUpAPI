﻿using TeamUpAPI.Models;

namespace TeamUpAPI.Contracts.Requests
{
    public class AddUserRequest
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public string FriendsList { get; set; }
        public string GamesList { get; set; }
    }
}
