

using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Data;
using Catel.MVVM;

using Equality.Http;
using Equality.Extensions;
using Equality.Validation;
using Equality.MVVM;
using Equality.Models;
using Equality.Services;
using System.Windows.Input;
using Equality.Data;

namespace Equality.ViewModels
{
    public class CreateColumnControlViewModel : ViewModel
    {
        Project Project = StateManager.SelectedProject;

        IBoardService BoardService;

        IColumnService ColumnService;

        public CreateColumnControlViewModel(IBoardService boardService, IColumnService columnService)
        {
            BoardService = boardService;
            ColumnService = columnService;

            CreateBoard = new TaskCommand<KeyEventArgs>(OnCreateBoardExecute);
            CloseWindow = new TaskCommand(OnCloseWindowExecute);
        }

        #region Properties

        [Model]
        public Column Column { get; set; } = new();

        [ViewModelToModel(nameof(Column))]
        [Validatable]
        public string Name { get; set; }

        #endregion

        #region Commands

        public TaskCommand<KeyEventArgs> CreateBoard { get; private set; }

        private async Task OnCreateBoardExecute(KeyEventArgs args)
        {
            if (args != null) {
                if (args.Key == Key.Escape) {
                    CloseWindow.Execute();
                }

                if (args.Key != Key.Enter) {
                    return;
                }
            }

            if (FirstValidationHasErrors() || HasErrors) {
                return;
            }

            try {
                var response = await ColumnService.CreateColumnAsync(StateManager.SelectedBoard, Column);
                Project.SyncWith(response.Object);

                await SaveViewModelAsync();
                await CloseViewModelAsync(true);
            } catch (UnprocessableEntityHttpException e) {
                HandleApiErrors(e.Errors);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        public TaskCommand CloseWindow { get; private set; }

        private async Task OnCloseWindowExecute()
        {
            await CancelViewModelAsync();
            await CloseViewModelAsync(false);
        }

        #endregion

        #region Validation

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
