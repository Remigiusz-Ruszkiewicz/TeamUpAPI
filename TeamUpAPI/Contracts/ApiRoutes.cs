namespace TeamUpAPI.Contracts
{
    public static class ApiRoutes
    {
        private const string Base = "api/v1/";

        public static class User
        {
            public const string AddUser = Base + "User";

            public const string AllUsers = Base + "User";

            public const string UserById = Base + "User/{id}";

            public const string UpdateUser = Base + "User";

            public const string DeleteUser = Base + "User";

            public const string CurrentUserInfo = Base + "User/currentUser";

            public const string RecommendedUsers = Base + "User/Recommended";

            //public const string GetRecommendedUsersByGame = Base + "User/Recommended/{gameId}";

            public const string UserFriends = Base + "User/Friends";

            public const string AddToUserFriends = Base + "User/Friends";

            public const string DeleteFromUserFriends = Base + "User/Friends/{id}";
        }
        public static class Auth
        {
            public const string Login = Base + "Login";
            public const string Register = Base + "Register";
            public const string LoginWithToken = Base + "LoginWithToken";
            public const string RegisterWithToken = Base + "RegisterWithToken";
        }
        public static class Game
        {
            public const string AllGames = Base + "Game";
            public const string GameById = Base + "Game/{id}";
            public const string CurrentUserGamesList = Base + "Game/Games";
            public const string AddToUserGames = Base + "Game/Games";
            public const string DeleteFromUserGames = Base + "Game/Games";
            public const string AllCategories = Base + "Game/Categories";
            public const string GamesByCategory = Base + "Game/Categories/{category}";
        }
    }
}