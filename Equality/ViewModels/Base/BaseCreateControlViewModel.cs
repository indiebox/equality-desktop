using System;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.MVVM;

using Equality.Data;
using Equality.Http;
using Equality.MVVM;

namespace Equality.ViewModels.Base
{
    public abstract class BaseCreateControlViewModel : ViewModel
    {
        protected BaseCreateControlViewModel()
        {
            OkCommand = new(OnOkCommandExecute, OnOkCommandCanExecute);
            CloseCommand = new(OnCloseCommandExecute, OnCloseCommandCanExecute);
        }

        #region Properties

        /// <summary>
        /// The result of action.
        /// When <see cref="OkCommand"/> is executed, this property will set to <see langword="true"/>, otherwise <see langword="false"/>.
        /// </summary>
        public bool Result { get; set; } = false;

        #endregion

        #region Commands

        public TaskCommand<object> OkCommand { get; private set; }

        protected virtual async Task OnOkCommandExecute(object param)
        {
            if (FirstValidationHasErrors()) {
                return;
            }

            try {
                await OkAction(param);

                Result = true;
                await SaveViewModelAsync();
                await CloseViewModelAsync(true);
            } catch (UnprocessableEntityHttpException e) {
                HandleApiErrors(e.Errors);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        protected virtual bool OnOkCommandCanExecute(object param)
        {
            return true;
        }

        protected abstract Task OkAction(object param);

        public TaskCommand CloseCommand { get; private set; }

        private async Task OnCloseCommandExecute()
        {
            Result = false;
            await CancelViewModelAsync();
            await CloseViewModelAsync(false);
        }

        protected virtual bool OnCloseCommandCanExecute()
        {
            return true;
        }

        #endregion
    }
}
