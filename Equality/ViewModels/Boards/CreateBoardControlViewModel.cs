using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using Catel.Data;
using Catel.MVVM;

using Equality.Data;
using Equality.Extensions;
using Equality.Models;
using Equality.Services;
using Equality.Validation;
using Equality.ViewModels.Base;

namespace Equality.ViewModels
{
    public class CreateBoardControlViewModel : BaseCreateControlViewModel
    {
        protected IBoardService BoardService;

        public CreateBoardControlViewModel(IBoardService boardService)
        {
            BoardService = boardService;
        }

        #region Properties

        [Model]
        public Board Board { get; set; } = new();

        [ViewModelToModel(nameof(Board))]
        [Validatable]
        public string Name { get; set; }

        #endregion

        #region Commands

        public TaskCommand<KeyEventArgs> CreateBoard { get; private set; }

        protected override async Task OkAction(object param)
        {
            var response = await BoardService.CreateBoardAsync(StateManager.SelectedProject, Board);
            Board.SyncWith(response.Object);
        }

        protected override bool OnOkCommandCanExecute(object param) => !HasErrors;

        #endregion

        #region Validation

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            var validator = new Validator(validationResults);

            validator.ValidateField(nameof(Name), Name, new()
            {
                new NotEmptyStringRule(),
                new MaxStringLengthRule(255),
            });
        }

        #endregion
    }
}
