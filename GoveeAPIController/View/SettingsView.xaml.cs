using System.Collections.ObjectModel;
using System.IO;

namespace GoveeAPIController.View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : MetroWindow, INotifyPropertyChanged
    {

        private ObservableCollection<Theme> _themeCollection = new();

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Theme> ThemeCollection
        {
            get => _themeCollection;
            set
            {
                if (_themeCollection != value)
                {
                    _themeCollection = value;
                    OnPropertyChanged();
                }
            }
        }

        public SettingsView()
        {
            InitializeComponent();
            ReadThemes();
            this.DataContext = this;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void ReadThemes()
        {
            string workingDir = Environment.CurrentDirectory;
            string parentDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            Environment.CurrentDirectory = parentDir;
            string themePath = ConfigurationManager.AppSettings["ThemesPath"];

            if (Directory.Exists(themePath))
            {
                var themeFiles = Directory.GetFiles(themePath, "*xaml");

                foreach (var themeFile in themeFiles)
                {
                    Theme thm = new()
                    {
                        Name = themeFile.Replace(themePath + "\\", ""),
                        Path = themeFile.Replace('\\', '/')
                    };

                    if (ConfigurationManager.AppSettings["CurrentTheme"].Contains(thm.Name))
                    {
                        thm.IsSelected = true;
                    }

                    ThemeCollection.Add(thm);
                }
            }

            Environment.CurrentDirectory = workingDir;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            if (sender is Button)
            {
                this.Close();
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


        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (Theme theme in ThemeCollection)
            {
                if (theme.IsSelected)
                {
                    if (ConfigurationManager.AppSettings["CurrentTheme"] != theme.Path)
                    {
                        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        AppSettingsSection appset = (AppSettingsSection)config.GetSection("appSettings");
                        appset.Settings["CurrentTheme"].Value = theme.Path;
                        config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");

                        var dialogSetting = new MetroDialogSettings()
                        {
                            AffirmativeButtonText = "Ok",
                            AnimateShow = true,
                            AnimateHide = true
                        };

                        await this.ShowMessageAsync("SPEICHERN", "Es wurde eine Speicherung vorgenommen!", MessageDialogStyle.Affirmative, dialogSetting);

                        this.Close();

                    }
                }
            }
        }
    }
}
