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

                SelectedProject = new Project()
                {
                    Name = "Equality",
                    Team = SelectedTeam,
                };

                SelectedBoard = new Board()
                {
                    Name = "Board 1",
                };
            }
        }

        public delegate void PropertyChangedHandler();

        public static event PropertyChangedHandler SelectedTeamChanged;

        public static event PropertyChangedHandler SelectedProjectChanged;

        private static Team _selectedTeam;

        private static Project _selectedProject;

        public static string ApiToken { get; set; }

        public static User CurrentUser { get; set; }

        public static Team SelectedTeam
        {
            get { return _selectedTeam; }
            set {
                _selectedTeam = value;
                SelectedTeamChanged?.Invoke();
            }
        }

        public static Project SelectedProject
        {
            get { return _selectedProject; }
            set {
                _selectedProject = value;
                SelectedProjectChanged?.Invoke();
            }
        }

        public static Board SelectedBoard { get; set; }
    }
}
