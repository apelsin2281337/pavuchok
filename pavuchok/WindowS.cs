namespace Spider_Solitaire
{
    /// <summary>
    /// Interaction logic for WSolitaire.xaml
    /// </summary>
    public partial class WindowS : Window
    {
        public Game? game;
        public WindowS()
        {
            InitializeComponent();
            Settings.WriteSettingsFile();
        }



        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            Menu menu = new();
            SolitaireFrame.NavigationService.Navigate(menu);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new();
            settings.Show();

        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
