﻿using System;
using System.Collections.Generic;
using System.Text;

using Catel.MVVM;

namespace Equality.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        public string Name { get; set; }

        public StartPageViewModel()
        {
            Name = "Hello, " + Properties.Settings.Default.api_name.ToString();
        }
    }
}
