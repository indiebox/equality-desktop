using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.Services;

using Equality.Models;
using Equality.MVVM;

namespace Equality.ViewModels
{
    public class LeaderElectionPageViewModel : ViewModel
    {
        #region DesignModeConstructor

        public LeaderElectionPageViewModel()
        {
            HandleDesignMode(() =>
            {
                NominatedMembers.AddRange(new LeaderNomination[]
                {
                    new LeaderNomination() { User = new User(){ Name = "user1" }, Count = 5, PercentageSupport = 50 },
                    new LeaderNomination() { User = new User(){ Name = "user2" }, Count = 3, PercentageSupport = 30 },
                    new LeaderNomination() { User = new User(){ Name = "user3" }, Count = 2, PercentageSupport = 20 },
                });
            });
        }

        #endregion

        public LeaderElectionPageViewModel(INavigationService navigationService)
        {
        }

        #region Properties

        public ObservableCollection<LeaderNomination> NominatedMembers { get; set; } = new();

        #endregion

        #region Commands



        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
