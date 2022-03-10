using Equality.Data;

namespace Equality.ViewModels.Design
{
    public class DesignProjectPageViewModel : ProjectPageViewModel
    {
        public DesignProjectPageViewModel() : base(null)
        {
            Project = StateManager.SelectedProject;
        }
    }
}
