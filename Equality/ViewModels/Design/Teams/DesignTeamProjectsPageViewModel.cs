﻿using Catel.Collections;

using Equality.Models;

namespace Equality.ViewModels.Design
{
    public class DesignTeamProjectsPageViewModel : TeamProjectsPageViewModel
    {
        public DesignTeamProjectsPageViewModel() : base(null)
        {
            Projects.AddRange(new Project[] {
                new Project { Name = "Project I"},
                new Project { Name = "Project II"},
                new Project { Name = "Project III"}
            });
        }
    }
}
