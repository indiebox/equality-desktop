using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Data;
using Catel.MVVM;

using Equality.Core.ApiClient;
using Equality.Core.Extensions;
using Equality.Core.Helpers;
using Equality.Core.Validation;
using Equality.Core.ViewModel;
using Equality.Models;
using Equality.Services;

using MaterialDesignThemes.Wpf;

namespace Equality.ViewModels
{
    public class InviteUserDialogViewModel : ViewModel
    {
        protected Team Team;

        protected Invite Invite;

        protected ITeamService TeamService;

        public InviteUserDialogViewModel(Invite invite, ITeamService teamService)
        {
            Invite = invite;
            TeamService = teamService;

            InviteUser = new TaskCommand(OnInviteUserExecute, () => !HasErrors);

            ApiFieldsMap = new()
            {
                { nameof(Email), "email" },
            };
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
                var response = await TeamService.InviteUserAsync(Team, Email);

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
