namespace Equality.ViewModels.Design
{
    public class DesignTeamPageViewModel : TeamPageViewModel
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
