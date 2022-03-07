using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Data;
using Catel.MVVM;

using Equality.Http;
using Equality.Extensions;
using Equality.Helpers;
using Equality.Validation;
using Equality.MVVM;
using Equality.Models;
using Equality.Services;

using MaterialDesignThemes.Wpf;

namespace Equality.ViewModels
{
    public class InviteUserDialogViewModel : ViewModel
    {
        protected Team Team;

        protected Invite Invite;

        protected IInviteService InviteService;

        public InviteUserDialogViewModel(Invite invite, IInviteService inviteService)
        {
            Invite = invite;
            InviteService = inviteService;

            InviteUser = new TaskCommand(OnInviteUserExecute, () => !HasErrors);
        }

        public override string Title => "Отправить приглашение";

        #region Properties

        public string Email { get; set; }

        #endregion

        #region Commands

        public TaskCommand InviteUser { get; private set; }

        private async Task OnInviteUserExecute()
        {
            if (FirstValidationHasErrors()) {
                return;
            }

            try {
                var response = await InviteService.InviteUserAsync(Team, Email);

                Invite.SyncWith(response.Object);

                DialogHost.CloseDialogCommand.Execute(true, null);
            } catch (UnprocessableEntityHttpException e) {
                HandleApiErrors(e.Errors);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        #region Validation

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            var validator = new Validator(validationResults);

            validator.ValidateField(nameof(Email), Email, new()
            {
                new NotEmptyStringRule(),
                new MaxStringLengthRule(255),
            });
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            Team = MvvmHelper.GetFirstInstanceOfViewModel<TeamPageViewModel>().Team;
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
