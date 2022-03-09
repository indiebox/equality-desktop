using System;

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
                    Name = "Indie Box",
                    Email = "indiebox.company@gmail.com",
                    CreatedAt = DateTime.Today,
                };

                SelectedTeam = new Team()
                {
                    Id = 1,
                    Name = "Indie Box",
                    Url = "https://indiebox.ru/",
                };
            }
        }

        public static event EventHandler SelectedTeamChanged;

        private static Team _selectedTeam;

        public static string ApiToken { get; set; }

        public static User CurrentUser { get; set; }

        public static Team SelectedTeam
        {
            get { return _selectedTeam; }
            set {
                _selectedTeam = value;
                SelectedTeamChanged?.Invoke(null, new EventArgs());
            }
        }
    }
}
