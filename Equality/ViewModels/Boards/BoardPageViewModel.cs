using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.MVVM;

using Catel.Services;

using Equality.Extensions;
using Equality.Helpers;
using Equality.Models;
using Equality.MVVM;

namespace Equality.ViewModels
{
    public class BoardPageViewModel : ViewModel
    {
        #region DesignModeConstructor

        public BoardPageViewModel()
        {
            HandleDesignMode(() =>
            {
                Columns.AddRange(new Column[] {
                    new Column() { Name = "Column1" },
                    new Column() { Name = "Column2" },
                });
            });

        }

        #endregion

        INavigationService NavigationService { get; set; }

        public BoardPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            ToBoards = new Command(OnToBoardsExecute);
            OpenCreateColumnWindow = new TaskCommand(OnOpenCreateColumnWindowExecuteAsync);
        }

        #region Properties

        public ObservableCollection<Column> Columns { get; set; } = new();

        public CreateColumnControlViewModel CreateColumnVm { get; set; }

        #endregion

        #region Commands


        public TaskCommand OpenCreateColumnWindow { get; private set; }

        private async Task OnOpenCreateColumnWindowExecuteAsync()
        {
            CreateColumnVm = MvvmHelper.CreateViewModel<CreateColumnControlViewModel>();
            CreateColumnVm.ClosedAsync += CreateColumnVmClosedAsync;
        }

        public Command ToBoards { get; private set; }

        private void OnToBoardsExecute()
        {
            NavigationService.Navigate<BoardsPageViewModel, ProjectPageViewModel>();
        }

        #endregion

        #region Methods

        private Task CreateColumnVmClosedAsync(object sender, ViewModelClosedEventArgs e)
        {
            if (e.Result ?? false) {
                Columns.Add(CreateColumnVm.Column);
            }

            CreateColumnVm.ClosedAsync -= CreateColumnVmClosedAsync;
            CreateColumnVm = null;

            return Task.CompletedTask;
        }

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
