using System;
using System.Collections.Generic;
using System.Text;

namespace Equality.Services
{
    public interface IThemeService
    {
        public enum Theme
        {
            Light,
            Dark,
            Sync,
        }

        public Theme GetCurrentTheme();

        public void SetColorTheme(Theme theme);
    }
}
