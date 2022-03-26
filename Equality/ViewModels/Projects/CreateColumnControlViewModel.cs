

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

namespace Equality.ViewModels
{
    public class CreateColumnControlViewModel : ViewModel
    {
        public CreateColumnControlViewModel()
        {
        }

        #region Properties

        [Model]
        public Column Column { get; set; } = new();

        [ViewModelToModel(nameof(Column))]
        [Validatable]
        public string Name { get; set; }

        #endregion

        #region Commands

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
