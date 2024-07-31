namespace GoveeAPIController;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        MainWindow mw = new();
        mw.Show();
    }
}