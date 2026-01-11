
namespace GoveeAPIController.Classes;

public static class GoveeApplication
{
    public static string ThemesPath { get; set; }
    public static Theme CurrentTheme { get; set; } = new Theme();
    public static string OpenWeatherApiKey { get; set; }
    public static string ApiKey { get; set; }
    public static string Device { get; set; }
    public static string Model { get; set; }

    public static void LoadAppSettings()
    {
        ThemesPath = ConfigurationManager.AppSettings["ThemesPath"];
        GetCurrentTheme();
        ApiKey = ConfigurationManager.AppSettings["ApiKey"];
        OpenWeatherApiKey = ConfigurationManager.AppSettings["OpenWeatherApiKey"];
        Device = ConfigurationManager.AppSettings["Device"];
        Model = ConfigurationManager.AppSettings["Model"];
    }

    private static void GetCurrentTheme()
    {
        CurrentTheme.Name = ConfigurationManager.AppSettings["CurrentTheme"];
        CurrentTheme.Path = ConfigurationManager.AppSettings[CurrentTheme.Name];
        CurrentTheme.IsSelected = true;
    }
}
