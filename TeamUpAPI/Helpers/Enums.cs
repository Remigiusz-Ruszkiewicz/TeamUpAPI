namespace TeamUpAPI.Helpers
{
    public static class Enums
    {
        public enum GameCategories
        {
            Sandbox = 0,
            RealTimeStrategy = 1,
            Shooters = 2,
            MultiplayerOnlineBattleArena = 3,
            RolePlaying = 4,
            SimulationAndSports = 5,
            PuzzlersAndPartyGames = 6,
            ActionAdventure = 7,
            SurvivalAndHorror = 8,
            Racing = 9,
            Strategy = 10,
            Platformer = 11
        }
        public enum OperationResult
        {
            Ok = 200,
            Created = 201,
            BadRequest = 400,
            NotFound = 404,
            Error = 500,
            DBConnectionFailed = 503
        }
    }
}
