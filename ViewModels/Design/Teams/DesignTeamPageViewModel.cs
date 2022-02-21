namespace Equality.ViewModels.Design
{
    public class DesignTeamPageViewModel : TeamPageViewModel
    {
        public DesignTeamPageViewModel() : base(null, null, null)
        {
            Team = new()
            {
                Name = "Test team"
            };
        }
    }
}
