﻿using Catel.Windows;

namespace Equality.Views
{
    public partial class CreateTeamDataWindow
    {
        public CreateTeamDataWindow()
        : base(DataWindowMode.Custom, infoBarMessageControlGenerationMode: InfoBarMessageControlGenerationMode.None)
        {
            InitializeComponent();
        }
    }
}