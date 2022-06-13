using System.Collections.Generic;

namespace Equality.Data
{
    /// <summary>
    /// Class for working with complex deserializable types in Properties.Settings.
    /// </summary>
    internal static class SettingsManager
    {
        /// <summary>
        /// Key-value pair for favorite board for project.
        /// The key is a project id.
        /// The value is a board id.
        /// </summary>
        public static Dictionary<ulong, ulong> FavoriteBoards { get; set; } = new();

        /// <summary>
        /// The recent projects ids.
        /// </summary>
        public static BufferList<ulong> RecentProjects { get; set; } = new(5);

        static SettingsManager()
        {
            LoadSettings();

            Properties.Settings.Default.SettingsSaving += (s, e) => SaveSettings();
        }

        private static void LoadSettings()
        {
            try {
                var favoriteBoards = Json.Deserialize<Dictionary<ulong, ulong>>(Properties.Settings.Default.favorite_boards ?? "");
                if (favoriteBoards != null) {
                    FavoriteBoards = favoriteBoards;
                }
            } catch {
                Properties.Settings.Default.favorite_boards = string.Empty;
            }

            try {
                var recentProjects = Json.Deserialize<List<ulong>>(Properties.Settings.Default.recent_projects ?? "");
                if (recentProjects != null) {
                    RecentProjects.AddRange(recentProjects);
                }
            } catch {
                Properties.Settings.Default.recent_projects = string.Empty;
            }

            Properties.Settings.Default.Save();
        }

        private static void SaveSettings()
        {
            Properties.Settings.Default.favorite_boards = Json.Serialize(FavoriteBoards);
            Properties.Settings.Default.recent_projects = Json.Serialize(RecentProjects);
        }
    }
}
