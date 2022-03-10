﻿using Catel.Collections;

using Equality.Models;

namespace Equality.ViewModels.Design
{
    public class DesignProjectsPageViewModel : ProjectsPageViewModel
    {
        public DesignProjectsPageViewModel() : base(null, null, null)
        {
            Teams.AddRange(new Team[] {
                new Team() { Name = "Test dafs dfsafdsa fdsafdsafdsa", Projects = { new() } },
                new Team() { Name = "Test 2"},
                new Team() { Name = "Test 3"},
            });

            FilteredTeams.AddRange(Teams);
        }
    }
}
