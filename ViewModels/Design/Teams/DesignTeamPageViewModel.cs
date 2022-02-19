namespace Equality.ViewModels.Design
{
    class DesignTeamPageViewModel : TeamPageViewModel
    {
        public DesignTeamPageViewModel() : base()
        {
            Team = new()
            {
                Name = "Test team"
            };
        }
    }
}
