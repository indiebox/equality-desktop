using Equality.Models;

namespace Equality.ViewModels.Design
{
    public class DesignApplicationWindowViewModel : ApplicationWindowViewModel
    {
        public DesignApplicationWindowViewModel() : base(null, null, null)
        {
            SelectedTeam = new Team() { Name = "Выбранная команда" };
        }
    }
}
