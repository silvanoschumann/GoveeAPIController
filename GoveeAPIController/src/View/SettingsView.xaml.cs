using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

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

        private void ReadThemes()
        {
            ThemeCollection.Clear();

            string currentTheme = ConfigurationManager.AppSettings["CurrentTheme"];

            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                if (!key.EndsWith(".xaml")) { continue; }

                ThemeCollection.Add(new Theme
                {
                    Name = key,
                    Path = ConfigurationManager.AppSettings[key],
                    IsSelected = key == currentTheme
                });
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button) { return; }
            this.Close();
        }

        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) { return; }
            this.DragMove();
        }


        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Theme selectedTheme = ThemeCollection.FirstOrDefault(t => t.IsSelected);

            if (selectedTheme == null) { return; }

            if (ConfigurationManager.AppSettings["CurrentTheme"] == selectedTheme.Name)
            {
                this.Close();
                return;
            }

            GoveeApplication.SetTheme(selectedTheme);

            var dialogSetting = new MetroDialogSettings
            {
                AffirmativeButtonText = "Ok",
                AnimateShow = true,
                AnimateHide = true
            };

            await this.ShowMessageAsync(
                "SPEICHERN",
                "Es wurde eine Speicherung vorgenommen!",
                MessageDialogStyle.Affirmative,
                dialogSetting);

            this.Close();
        }
    }
}
