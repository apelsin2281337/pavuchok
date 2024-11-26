namespace Spider_Solitaire
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        public Game? game;
        public Menu()
        {
            InitializeComponent();

        }

        public void DestroyGameReference()
        {
            if (game == null) return;
            game.SolitaireGrid.Children.Clear();
            game.SolitaireGrid = null;
            game = null;

            GC.Collect();
        }

        private void OneSuiteNewGameClick(object sender, RoutedEventArgs e)
        {
            StartGame(1);
        }

        private void TwoSuiteNewGameClick(object sender, RoutedEventArgs e)
        {
            StartGame(2);
        }

        private void FourSuiteNewGameClick(object sender, RoutedEventArgs e)
        {
            StartGame(4);
        }

        private void ViewRulesClick(object sender, RoutedEventArgs e)
        {
            // Instantiate and display the rules window
            SpiderRulesWindow rulesWindow = new SpiderRulesWindow();
            rulesWindow.ShowDialog();
        }

        private void StartGame(int numberOfSuits)
        {
            if (game != null) DestroyGameReference();
            game = new Game(numberOfSuits, this);
            NavigationService.Navigate(game);

        }
    }
}
