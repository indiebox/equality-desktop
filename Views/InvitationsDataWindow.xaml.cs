using Catel.Windows;

namespace Equality.Views
{
    public partial class InvitationsDataWindow
    {
        public InvitationsDataWindow()
        : base(DataWindowMode.Custom, infoBarMessageControlGenerationMode: InfoBarMessageControlGenerationMode.None)
        {
            InitializeComponent();
        }
    }
}
