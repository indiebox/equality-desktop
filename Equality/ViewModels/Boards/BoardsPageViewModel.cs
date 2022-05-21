using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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

namespace Equality.ViewModels
{
    /*
     * NavigationContext: 
     * open-active-board - open active board, if available
     */
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
                ActiveBoard = Boards[^1];
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
            OpenCreateBoardWindow = new TaskCommand(OnOpenCreateBoardWindowExecuteAsync, () => CreateBoardVm is null && !IsBoardsLimitReached);
            StartEditBoardName = new Command<Board>(OnStartEditBoardNameExecuteAsync);
            SaveNewBoardName = new TaskCommand(OnSaveNewBoardNameExecuteAsync, () => GetFieldErrors(nameof(NewBoardName)) == string.Empty);
            CancelEditBoardName = new Command(OnCancelEditBoardNameExecute);
            MarkAsActive = new Command<Board>(OnMarkAsActiveExecute);

            Boards.CollectionChanged += (s, e) =>
            {
                RaisePropertyChanged(nameof(IsBoardsLimitReached));

                if (IsBoardsLimitReached && CreateBoardVm != null) {
                    CreateBoardVm.CloseCommand.Execute();
                }
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

        public bool IsBoardsLimitReached => Boards.Count >= 10;

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

        private Task CreateBoardVmClosedAsync(object sender, ViewModelClosedEventArgs e)
        {
            if (CreateBoardVm.Result) {
                Boards.Add(CreateBoardVm.Board);
            }

            CreateBoardVm.ClosedAsync -= CreateBoardVmClosedAsync;
            CreateBoardVm = null;

            return Task.CompletedTask;
        }

        public Command<Board> MarkAsActive { get; private set; }

        private void OnMarkAsActiveExecute(Board board)
        {
            try {
                var boardsIds = Json.Deserialize<Dictionary<string, ulong>>(Properties.Settings.Default.active_boards_id);
                string projectId = Project.Id.ToString();

                // Is currently active.
                if (ActiveBoard == board) {
                    boardsIds?.Remove(projectId);
                    Properties.Settings.Default.active_boards_id = Json.Serialize(boardsIds);

                    ActiveBoard = null;
                } else {
                    if (boardsIds != null) {
                        boardsIds[projectId] = board.Id;
                    } else {
                        boardsIds = new() { { projectId, board.Id } };
                    }

                    Properties.Settings.Default.active_boards_id = Json.Serialize(boardsIds);

                    ActiveBoard = board;
                }
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
                HandleApiErrors(e.Errors, new() { { "name", nameof(NewBoardName) } });
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

                        if (NavigationContext.Values.ContainsKey("open-active-board")) {
                            OpenBoardPage.Execute(ActiveBoard);
                        }
                    }
                }
            } catch {
                Properties.Settings.Default.active_boards_id = String.Empty;
                Properties.Settings.Default.Save();
            }
        }

        protected async void LoadBoardsAsync()
        {
            try {
                var response = await BoardService.GetBoardsAsync(StateManager.SelectedProject);

                Boards.AddRange(response.Object);

                LoadActiveBoard();
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
