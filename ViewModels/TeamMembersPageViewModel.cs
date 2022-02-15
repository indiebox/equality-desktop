using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.Data;
using Catel.MVVM;

using Equality.Core.ViewModel;
using Equality.Models;

namespace Equality.ViewModels
{
    public class TeamMembersPageViewModel : ViewModel
    {
        public TeamMembersPageViewModel()
        {
            Members.AddRange(new User[] {
                new User() { Name = "user1" },
                new User() { Name = "user2" },
                new User() { Name = "user3" },
                new User() { Name = "Пользователь 4" },
                new User() { Name = "Пользователь 5" },
                new User() { Name = "user1" },
                new User() { Name = "user2" },
                new User() { Name = "user3" },
                new User() { Name = "Пользователь 4" },
                new User() { Name = "Пользователь 5" },
                new User() { Name = "user1" },
                new User() { Name = "user2" },
                new User() { Name = "user3" },
                new User() { Name = "Пользователь 4" },
                new User() { Name = "Пользователь 5" },
            });

            FilterMembers();
        }

        public override string Title => "Equality";

        #region Properties

        public string FilterText { get; set; }

        private void OnFilterTextChanged()
        {
            FilterMembers();
        }

        public ObservableCollection<User> Members { get; set; } = new();

        public ObservableCollection<User> FilteredMembers { get; set; } = new();

        #endregion

        #region Commands



        #endregion

        #region Methods

        protected void FilterMembers()
        {
            if (string.IsNullOrEmpty(FilterText)) {
                FilteredMembers.ReplaceRange(Members);

                return;
            }

            FilteredMembers.ReplaceRange(Members.Where(user => user.Name.Contains(FilterText)));
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            StateManager.CurrentUser = new() { Id = 1, Name = "admin" };

            Members.Insert(0, StateManager.CurrentUser);

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
