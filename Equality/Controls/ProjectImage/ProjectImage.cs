using System.Windows;
using System.Windows.Controls;

namespace Equality.Controls
{
    public class ProjectImage : TeamLogo
    {
        static ProjectImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProjectImage), new FrameworkPropertyMetadata(typeof(ProjectImage)));
        }
    }
}
