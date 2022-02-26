using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Data;
using Catel.Fody;
using Catel.MVVM;

using Equality.Core.Validation;
using Equality.Core.ViewModel;
using Equality.Models;
using Equality.Services;

namespace Equality.ViewModels
{
    public class InvitationsDataWindowViewModel : ViewModel
    {

        public InvitationsDataWindowViewModel()
        {
        }

        public override string Title => "Equality";

        #region Properties



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
