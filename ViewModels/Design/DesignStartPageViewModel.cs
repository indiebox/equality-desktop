using System;
using System.Collections.Generic;
using System.Text;

namespace Equality.ViewModels.Design
{
    public class DesignStartPageViewModel : StartPageViewModel
    {
        public DesignStartPageViewModel() : base(null, null)
        {
            Name = "Hello, " +
                "Peter";
        }
    }
}
