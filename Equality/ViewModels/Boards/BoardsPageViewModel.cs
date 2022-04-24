using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Catel;
using Catel.Collections;
using Catel.Data;
using Catel.MVVM;
using Catel.Services;

using Equality.Data;
using Equality.Extensions;
using Equality.Helpers;
using Equality.Http;
using Equality.Models;
using Equality.MVVM;
using Equality.Services;
using Equality.Validation;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Equality.ViewModels
{
    public class BoardsPageViewModel : ViewModel
    {
        #region DesignModeConstructor

        public BoardsPageViewModel()
        {
            HandleDesignMode(() =>
            {
                Boards.AddRange(new Board[]
                {
                    new() { Id = 1, Name = "Board" },
                    new() { Id = 2, Name = "Board1" },
                });
            });
        }

        #endregion

        IBoardService BoardService;

        INavigationService NavigationService;

        public BoardsPageViewModel(IBoardService boardService, INavigationService navigationService)
        {
            BoardService = boardService;
            NavigationService = navigationService;

            OpenBoardPage = new Command<Board>(OnOpenOpenBoardPageExecute);
            OpenCreateBoardWindow = new TaskCommand(OnOpenCreateBoardWindowExecuteAsync, () => CreateBoardVm is null);
            StartEditBoardName = new Command<Board>(OnStartEditBoardNameExecuteAsync);
            SaveNewBoardName = new TaskCommand(OnSaveNewBoardNameExecuteAsync, () => GetFieldErrors(nameof(NewBoardName)) == string.Empty);
            CancelEditBoardName = new Command(OnCancelEditBoardNameExecute);
            MarkAsActive = new Command<Board>(OnMarkAsActiveExecute);
            ClearActiveBoard = new Command(OnClearActiveBoardExecute);

            ApiFieldsMap = new Dictionary<string, string>()
            {
                { nameof(NewBoardName), "name" },
            };
        }

        #region Properties

        public Project Project { get; set; } = StateManager.SelectedProject;

        public ObservableCollection<Board> Boards { get; set; } = new();

        public CreateBoardControlViewModel CreateBoardVm { get; set; }

        public Board EditableBoard { get; set; } = null;

        public Board ActiveBoard { get; set; } = null;

        [Validatable]
        public string NewBoardName { get; set; }

        #endregion

        #region Commands

        public Command<Board> OpenBoardPage { get; private set; }

        private void OnOpenOpenBoardPageExecute(Board board)
        {
            StateManager.SelectedBoard = board;

            NavigationService.Navigate<BoardPageViewModel, ProjectPageViewModel>();
        }

        public TaskCommand OpenCreateBoardWindow { get; private set; }

        private async Task OnOpenCreateBoardWindowExecuteAsync()
        {
            CreateBoardVm = MvvmHelper.CreateViewModel<CreateBoardControlViewModel>();
            CreateBoardVm.ClosedAsync += CreateBoardVmClosedAsync;
        }

        public Command<Board> MarkAsActive { get; private set; }

        private void OnMarkAsActiveExecute(Board board)
        {
            try {
                var boardsIds = Json.Deserialize<Dictionary<string, ulong>>(Properties.Settings.Default.active_boards_id);
                string projectId = Project.Id.ToString();

                if (boardsIds != null) {
                    boardsIds[projectId] = board.Id;
                } else {
                    boardsIds = new() { { projectId, board.Id } };
                }

                Properties.Settings.Default.active_boards_id = Json.Serialize(boardsIds);

                ActiveBoard = board;
            } catch {
                Properties.Settings.Default.active_boards_id = string.Empty;
            }

            Properties.Settings.Default.Save();
        }

        public Command ClearActiveBoard { get; private set; }

        private void OnClearActiveBoardExecute()
        {
            try {
                var boardsIds = Json.Deserialize<Dictionary<string, ulong>>(Properties.Settings.Default.active_boards_id);
                string projectId = Project.Id.ToString();
                boardsIds.Remove(projectId);

                Properties.Settings.Default.active_boards_id = Json.Serialize(boardsIds);

                ActiveBoard = null;
            } catch {
                Properties.Settings.Default.active_boards_id = string.Empty;
            }

            Properties.Settings.Default.Save();
        }

        public Command<Board> StartEditBoardName { get; private set; }

        private void OnStartEditBoardNameExecuteAsync(Board board)
        {
            NewBoardName = board.Name;
            EditableBoard = board;
        }

        public Command CancelEditBoardName { get; private set; }

        private void OnCancelEditBoardNameExecute()
        {
            EditableBoard = null;
            NewBoardName = null;
            Validate(true);
        }

        public TaskCommand SaveNewBoardName { get; private set; }

        private async Task OnSaveNewBoardNameExecuteAsync()
        {
            if (FirstValidationHasErrors()) {
                return;
            }

            if (NewBoardName == EditableBoard.Name) {
                CancelEditBoardName.Execute();

                return;
            }

            try {
                Board board = new()
                {
                    Id = EditableBoard.Id,
                    Name = NewBoardName,
                };

                var response = await BoardService.UpdateBoardAsync(board);

                EditableBoard.SyncWith(response.Object);

                CancelEditBoardName.Execute();
            } catch (UnprocessableEntityHttpException e) {
                HandleApiErrors(e.Errors);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        #endregion

        #region Methods

        private void LoadActiveBoard()
        {
            try {
                var boardsIds = Json.Deserialize<Dictionary<string, ulong>>(Properties.Settings.Default.active_boards_id);
                if (boardsIds != null) {
                    string projectId = Project.Id.ToString();
                    if (boardsIds.ContainsKey(projectId)) {
                        ActiveBoard = Boards.Where(board => board.Id == boardsIds[projectId]).FirstOrDefault();
                    }
                }
            } catch {
                Properties.Settings.Default.active_boards_id = String.Empty;
                Properties.Settings.Default.Save();
            }
        }

        private Task CreateBoardVmClosedAsync(object sender, ViewModelClosedEventArgs e)
        {
            if (CreateBoardVm.Result) {
                Boards.Add(CreateBoardVm.Board);
            }

            CreateBoardVm.ClosedAsync -= CreateBoardVmClosedAsync;
            CreateBoardVm = null;

            return Task.CompletedTask;
        }

        public (string, string) Deserialize(JToken data)
        {
            Argument.IsNotNull(nameof(data), data);

            return data.ToObject<(string, string)>(new()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
        }

        protected async void LoadBoardsAsync()
        {
            try {
                var response = await BoardService.GetBoardsAsync(StateManager.SelectedProject);

                Boards.AddRange(response.Object);

                LoadActiveBoard();

                if (ActiveBoard != null) {
                    StateManager.SelectedBoard = ActiveBoard;

                    NavigationService.Navigate<BoardPageViewModel, ProjectPageViewModel>();
                }
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        #endregion

        #region Validation

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            var validator = new Validator(validationResults);

            if (EditableBoard != null) {
                validator.ValidateField(nameof(NewBoardName), NewBoardName, new()
                {
                    new NotEmptyStringRule(),
                    new MaxStringLengthRule(255),
                });
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            LoadBoardsAsync();
        }

        protected override async Task CloseAsync()
        {
            if (CreateBoardVm != null) {
                CreateBoardVm.ClosedAsync -= CreateBoardVmClosedAsync;
            }

            await base.CloseAsync();
        }
    }
}
