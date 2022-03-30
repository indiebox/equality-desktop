using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Catel.Collections;
using Catel.MVVM;

using Catel.Services;

using Equality.Controls;
using Equality.Data;
using Equality.Extensions;
using Equality.Helpers;
using Equality.Models;
using Equality.MVVM;
using Equality.Services;

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

        IColumnService ColumnService { get; set; }

        public BoardPageViewModel(INavigationService navigationService, IColumnService columnService)
        {
            NavigationService = navigationService;
            ColumnService = columnService;

            ToBoards = new Command(OnToBoardsExecute);
            OpenCreateColumnWindow = new TaskCommand(OnOpenCreateColumnWindowExecuteAsync);
        }

        #region Properties


        [Model]
        public Board Board { get; set; }

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

        private void Card_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is ColumnControl column && e.LeftButton == MouseButtonState.Pressed) {

            }
        }

        protected async Task LoadColumnsAsync()
        {
            try {
                var response = await ColumnService.GetColumnsAsync(StateManager.SelectedBoard);

                Columns.AddRange(response.Object);

            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

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

            await LoadColumnsAsync();
            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
