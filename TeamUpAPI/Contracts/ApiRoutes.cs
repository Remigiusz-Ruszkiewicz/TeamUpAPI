﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamUpAPI.Contracts
{
    public static class ApiRoutes
    {

        public const string Base = "api/v1/";

        public static class User
        {

            public const string AddUser = Base + "User";

            public const string GetAllUsers = Base + "User";

            public const string GetUserById = Base + "User/{id}";

            public const string UpdateUser = Base + "User/{id}";

            public const string DeleteUser = Base + "User/{id}";
        }

    }
}