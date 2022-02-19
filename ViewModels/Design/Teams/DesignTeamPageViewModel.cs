namespace Equality.ViewModels.Design
{
    public class DesignTeamPageViewModel : TeamPageViewModel
    {
        public DesignTeamPageViewModel() : base(null)
        {
            Team = new()
            {
                Name = "Test team"
            };
        }
    }
}
