using GoveeAPIController.src.Models;
using GoveeAPIController.src.Services;
using GoveeAPIController.src.Services.Implementation;
using GoveeAPIController.src.Services.Interfaces;
using GoveeAPIController.View;

namespace GoveeAPIController;

public partial class MainWindow : MetroWindow, INotifyPropertyChanged
{
    const int SLEEPTIME = 4000;
    private DeviceService _deviceService;
    private IApiService _apiService;
    private IHttpService _httpService;

    public event PropertyChangedEventHandler PropertyChanged;

    private string _tbResult;

    public string TbResult
    {
        get => _tbResult;
        set
        {
            if (_tbResult != value)
            {
                _tbResult = value;
                OnPropertyChanged();
            }
        }
    }

    private string _ApiKey;
    private bool _hasAPIKey;

    public string ApiKey
    {
        get => _ApiKey; set
        {
            if (_ApiKey != value)
            {
                _ApiKey = value;
                OnPropertyChanged();
            }
        }
    }

    public bool HasAPIKey
    {
        get => _hasAPIKey;
        set
        {
            if (_hasAPIKey != value)
            {
                _hasAPIKey = value;
                OnPropertyChanged();
            }
        }
    }

    public MainWindow()
    {
        GoveeApplication.LoadAppSettings();
        InitializeServices();
        GetDeviceState();
        SetTheme();
        InitializeComponent();
        this.DataContext = this;
    }

    private void InitializeServices()
    {
        LookForApiKey();
        _deviceService = new DeviceService(GoveeApplication.Device, GoveeApplication.Model);
        _apiService = new ApiService();
        _httpService = new HttpService(_deviceService, _apiService);
    }

    private void LookForApiKey()
    {
        ApiKey = GoveeApplication.ApiKey;
        if (!string.IsNullOrWhiteSpace(ApiKey))
        {
            HasAPIKey = true;
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


    private async void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button)
        {
            return;
        }

        var dialogSetting = new MetroDialogSettings()
        {
            AffirmativeButtonText = "Ja",
            NegativeButtonText = "Nein",
            AnimateShow = true,
            AnimateHide = true
        };

        var erg = await this.ShowMessageAsync("ACHTUNG", "Wollen Sie die Anwendung wirklich schließen?", MessageDialogStyle.AffirmativeAndNegative, dialogSetting);

        if (erg == MessageDialogResult.Affirmative)
        {
            Application.Current.Shutdown();
        }
    }

    private void BtnMinimize_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button)
        {
            this.WindowState = WindowState.Minimized;
        }

    }

    private void TglBtnMaximize_Click(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleButton senderTglBtn)
        {
            this.WindowState = senderTglBtn.IsChecked == true ? WindowState.Maximized : WindowState.Normal;
        }

    }

    private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            this.DragMove();
        }

    }

    private async void BtnColorTemp_Click(object sender, RoutedEventArgs e)
    {
        await PutColorTemp((int)colorTemp_slider.Value);
        await Task.Delay(SLEEPTIME);
    }

    private async void BtnBrightness_Click(object sender, RoutedEventArgs e)
    {
        int b = (int)brightness_slider.Value;
        await PutBrightness(b);
        await Task.Delay(SLEEPTIME);
    }

    public async Task PutColorTemp(int colorTemp)
    {
        await _httpService.SetColorTemp(colorTemp);
    }

    public async Task PutBrightness(int brightness)
    {
        await _httpService.SetBrightness(brightness);
    }

    public async Task PutOnOff(string OnOff)
    {
        await _httpService.ToggleLight(OnOff);
    }

    public async Task PutColor(RgbColor color)
    {
        await _httpService.SetColor(color);
    }

    public async void GetDeviceState()
    {
        var response = await _httpService.GetDeviceState();

        if (response != null)
        {
            SetSlider(response);
        }

    }

    private void SetSlider(DeviceStateResponse response)
    {
        if (response.data.properties[1].powerState == "on")
        {
            TglBtnSwitch.IsChecked = true;
        }

        if (response.data.properties[1].powerState == "off")
        {
            TglBtnSwitch.IsChecked = false;
        }

        TbResult = response.code.ToString();
        brightness_slider.Value = response.data.properties[2].brightness;
        blue_slider.Value = response.data.properties[3].color.b;
        red_slider.Value = response.data.properties[3].color.r;
        green_slider.Value = response.data.properties[3].color.g;
        colorTemp_slider.Value = response.data.properties.Length == 5 ? (double)response.data.properties[4].colorTem : 2000;
    }

    private async void TglBtnSwitch_Click(object sender, RoutedEventArgs e)
    {
        if (TglBtnSwitch.IsChecked == false)
        {
            await PutOnOff("off");
        }
        else
        {
            await PutOnOff("on");
        }
    }

    private void BtnDeviceState_Click(object sender, RoutedEventArgs e)
    {
        GetDeviceState();
    }

    private async void BtnColor_Click(object sender, RoutedEventArgs e)
    {
        RgbColor color = new()
        {
            r = (short)red_slider.Value,
            b = (short)blue_slider.Value,
            g = (short)green_slider.Value
        };
        await PutColor(color);
        await Task.Delay(SLEEPTIME);
    }

    private void BtnSettings_Click(object sender, RoutedEventArgs e)
    {
        SettingsView settingsView = new()
        {
            Owner = this,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        settingsView.ShowDialog();
    }

    public static void SetTheme()
    {
        //string workingDir = Environment.CurrentDirectory;
        //string parnetDir = Directory.GetParent(workingDir).Parent.Parent.FullName;
        //Environment.CurrentDirectory = parnetDir;
        //if (Directory.Exists(THEMESFOLDER))
        //{
        //    var themeFiles = Directory.GetFiles(THEMESFOLDER, "*xaml");
        //    foreach (var themeFile in themeFiles)
        //    {
        //        themeCollection.Add(new Theme()
        //        {
        //            Name = themeFile.Replace(THEMESFOLDER + "\\", ""),
        //            Path = themeFile
        //        });
        //    }
        //}
        string theme = GoveeApplication.CurrentTheme.Path;
        var uri = new Uri(theme, UriKind.Relative);
        ResourceDictionary resourceDict = new() { Source = uri };

        //RemoveExistingTheme();
        Application.Current.Resources.MergedDictionaries.Add(resourceDict);
    }

    //public static void RemoveExistingTheme()
    //{
    //    foreach (var item in Application.Current.Resources.MergedDictionaries)
    //    {
    //        //if (item.Source.ToString().Contains(LIGHTTHEMEURL) || item.Source.ToString().Contains(DARKTHEMEURL))
    //        //{
    //        Application.Current.Resources.MergedDictionaries.Remove(item);
    //        break;
    //        //}
    //    }
    //}

}
