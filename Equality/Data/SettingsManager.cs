using System.Collections.Generic;

namespace Equality.Data
{
    internal static class SettingsManager
    {
        public static Dictionary<string, ulong> ActiveBoards { get; set; } = new();

        static SettingsManager()
        {
            LoadSettings();

            Properties.Settings.Default.SettingsSaving += (s, e) => SaveSettings();
        }

        private static void LoadSettings()
        {
            try {
                var activeBoards = Json.Deserialize<Dictionary<string, ulong>>(Properties.Settings.Default.active_boards_id ?? "");
                if (activeBoards != null) {
                    ActiveBoards = activeBoards;
                }
            } catch {
                Properties.Settings.Default.active_boards_id = string.Empty;
                Properties.Settings.Default.Save();
            }
        }

        private static void SaveSettings()
        {
            Properties.Settings.Default.active_boards_id = Json.Serialize(ActiveBoards);
        }
    }
}
