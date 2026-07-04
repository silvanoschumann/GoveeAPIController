
namespace GoveeAPIController.Classes;

public static class GoveeApplication
{
    public static string ThemesPath { get; private set; } = string.Empty;
    public static Theme CurrentTheme { get; private set; } = new();
    public static string OpenWeatherApiKey { get; private set; } = string.Empty;
    public static string ApiKey { get; private set; } = string.Empty;
    public static string Device { get; private set; } = string.Empty;
    public static string Model { get; private set; } = string.Empty;

    public static void LoadAppSettings()
    {
        ThemesPath = GetSetting("ThemesPath");
        ApiKey = GetSetting("ApiKey");
        OpenWeatherApiKey = GetSetting("OpenWeatherApiKey");
        Device = GetSetting("Device");
        Model = GetSetting("Model");

        string themeName = GetSetting("CurrentTheme");

        CurrentTheme = new Theme
        {
            Name = themeName,
            Path = GetSetting(themeName),
            IsSelected = true
        };
    }

    private static string GetSetting(string key)
    {
        return ConfigurationManager.AppSettings[key] ?? throw new ConfigurationErrorsException($"The app setting '{key}' is missing.");
    }

    public static void SetTheme(Theme theme)
    {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        AppSettingsSection appset = (AppSettingsSection)config.GetSection("appSettings");

        appset.Settings["CurrentTheme"].Value = theme.Name;

        config.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection("appSettings");

        CurrentTheme = theme;
    }
}