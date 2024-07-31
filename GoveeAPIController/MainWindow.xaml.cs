using GoveeAPIController.View;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Data;

namespace GoveeAPIController;

public partial class MainWindow : MetroWindow, INotifyPropertyChanged
{
    private bool isDarkMode = false;
    private static readonly HttpClient client = new();
    const int SLEEPTIME = 4000;
    const string DARKTHEMEURL = "./src/resources/Themes/DarkTheme.xaml";
    const string LIGHTTHEMEURL = "./src/resources/Themes/LightTheme.xaml";

    public string APIKEY = "";

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

    private string _ApiName;
    public string ApiName
    {
        get => _ApiName; set
        {
            if (_ApiName != value)
            {
                _ApiName = value;
                OnPropertyChanged();
            }
        }
    }

    public MainWindow()
    {
        LookForApiKey();
        SetTheme();
        InitializeComponent();
        this.DataContext = this;
    }

    private void LookForApiKey()
    {
        if (ConfigurationManager.AppSettings["Api-Key"] != null)
        {
            APIKEY = ConfigurationManager.AppSettings["Api-Key"];
            ApiName = APIKEY;
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


    private async void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        if (sender != null)
        {
            if (sender is Button)
            {
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
        }
        else { return; }
    }

    private void BtnMinimize_Click(object sender, RoutedEventArgs e)
    {
        if (sender == null)
        {
            return;

        }

        if (sender is Button)
        {
            this.WindowState = WindowState.Minimized;
        }

    }

    private void TglBtnMaximize_Click(object sender, RoutedEventArgs e)
    {
        if (sender == null)
        {
            return;

        }

        if (sender is ToggleButton senderTglBtn)
        {
            this.WindowState = senderTglBtn.IsChecked == true ? WindowState.Maximized : WindowState.Normal;
        }

    }

    private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (e == null)
        {
            return;
        }

        if (e.LeftButton == MouseButtonState.Pressed)
        {
            this.DragMove();
        }

    }

    private async void BtnColorTemp_Click(object sender, RoutedEventArgs e)
    {
        await PutColorTemp((int)colorTemp_slider.Value);
        Thread.Sleep(SLEEPTIME);
        GetDeviceState();
    }

    private async void BtnBrightness_Click(object sender, RoutedEventArgs e)
    {
        int b = (int)brightness_slider.Value;
        await PutBrightness(b);
        Thread.Sleep(SLEEPTIME);
        GetDeviceState();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        //GetDeviceState();
    }

    public async Task PutColorTemp(int colorTemp)
    {
        GoveeLightBar<int> Govee = new()
        {
            device = "7E:F6:CD:32:37:36:49:09",
            model = "H6056",
            cmd = new Command<int>()
            {
                name = "colorTem",
                value = colorTemp
            },
        };

        string json = JsonConvert.SerializeObject(Govee, Formatting.Indented);

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "https://developer-api.govee.com/v1/devices/control");
            request.Headers.Add("Govee-API-Key", APIKEY);

            var content = new StringContent(json, null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            TbResult = response.StatusCode.ToString();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    public async Task PutBrightness(int brightness)
    {
        GoveeLightBar<int> Govee = new()
        {
            device = "7E:F6:CD:32:37:36:49:09",
            model = "H6056",
            cmd = new Command<int>()
            {
                name = "brightness",
                value = brightness
            },
        };

        string json = JsonConvert.SerializeObject(Govee, Formatting.Indented);

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "https://developer-api.govee.com/v1/devices/control");
            request.Headers.Add("Govee-API-Key", APIKEY);

            var content = new StringContent(json, null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            TbResult = response.StatusCode.ToString();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    public async Task PutOnOff(string OnOff)
    {
        GoveeLightBar<string> Govee = new()
        {
            device = "7E:F6:CD:32:37:36:49:09",
            model = "H6056",
            cmd = new Command<string>()
            {
                name = "turn",
                value = OnOff
            },
        };

        string json = JsonConvert.SerializeObject(Govee, Formatting.Indented);

        try
        {
            HttpRequestMessage request = new(HttpMethod.Put, "https://developer-api.govee.com/v1/devices/control");
            request.Headers.Add("Govee-API-Key", APIKEY);

            var content = new StringContent(json, null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            TbResult = response.StatusCode.ToString();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    public async Task PutColor(RgbColor color)
    {
        GoveeLightBar<RgbColor> Govee = new()
        {
            device = "7E:F6:CD:32:37:36:49:09",
            model = "H6056",
            cmd = new Command<RgbColor>()
            {
                name = "color",
                value = color
            },
        };

        string json = JsonConvert.SerializeObject(Govee, Formatting.Indented);

        try
        {
            HttpRequestMessage request = new(HttpMethod.Put, "https://developer-api.govee.com/v1/devices/control");
            request.Headers.Add("Govee-API-Key", APIKEY);

            var content = new StringContent(json, null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            TbResult = response.StatusCode.ToString();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    public async void GetDeviceState()
    {
        GoveeLightBar<string> Govee = new()
        {
            device = "7E:F6:CD:32:37:36:49:09",
            model = "H6056"
        };

        JsonConvert.SerializeObject(Govee, Formatting.Indented);
        try
        {
            HttpRequestMessage request = new(HttpMethod.Get, "https://developer-api.govee.com/v1/devices/state?device=7E:F6:CD:32:37:36:49:09&model=H6056&");

            request.Headers.Add("Govee-API-Key", APIKEY);
            var content = new StringContent("", null, "text/plain");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            TbResult = response.StatusCode.ToString();
            var jsonString = await response.Content.ReadAsStringAsync();
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<DeviceStateResponse>(jsonString);

            if (deserialized.code != 200)
                throw new Exception($"Request failed. Status code: {deserialized.code}, Message: {deserialized.message}");

            SetSlider(deserialized);


        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void SetSlider(DeviceStateResponse response)
    {
        if (response.data.properties[1].powerState == "on")
        {
            TglBtnSwitch.IsChecked = true;
        }

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
        Thread.Sleep(SLEEPTIME);
        GetDeviceState();
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

    private void TglBtnTheme_Click(object sender, RoutedEventArgs e)
    {
        isDarkMode = !isDarkMode;
    }

    public void SetTheme()
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
        string theme = ConfigurationManager.AppSettings["CurrentTheme"];
        var uri = new Uri(theme, UriKind.Relative);
        ResourceDictionary resourceDict = new() { Source = uri };

        foreach (var item in Application.Current.Resources.MergedDictionaries)
        {
            if (item.Source.ToString().Contains(LIGHTTHEMEURL))
            {
                Application.Current.Resources.MergedDictionaries.Remove(item);
                break;
            }

            if (item.Source.ToString().Contains(DARKTHEMEURL))
            {
                Application.Current.Resources.MergedDictionaries.Remove(item);
                break;
            }
        }
        Application.Current.Resources.MergedDictionaries.Add(resourceDict);
    }
}
