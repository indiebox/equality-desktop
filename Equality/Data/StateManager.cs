using Catel;

using Equality.Models;

namespace Equality.Data
{
    internal static class StateManager
    {
        static StateManager()
        {
            if (CatelEnvironment.IsInDesignMode) {
                CurrentUser = new User()
                {
                    Id = 1,
                    Name = "Logged user",
                    Email = "example@example.org",
                    CreatedAt = System.DateTime.Today,
                };

                SelectedProject = new Project()
                {
                    Name = "Equality",
                };
            }
        }

        public static User CurrentUser { get; set; }

        public static string ApiToken { get; set; }

        public static Project SelectedProject { get; set; }
    }
}
