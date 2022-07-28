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

        /// <summary>
        /// Get the current color theme.
        /// </summary>
        public Theme GetCurrentTheme();

        /// <summary>
        /// Set the new color theme.
        /// </summary>
        /// <param name="theme">The new theme.</param>
        public void SetColorTheme(Theme theme);
    }
}
