using System.Windows.Controls;

namespace Spider_Solitaire
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        // Bind soundEnabled to the CheckBox
        public bool soundEnabled = true;
        

        public Settings()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (!File.Exists(@"settings.txt")) WriteSettingsFile();
            try
            {
                string[] lines = File.ReadAllLines(@"settings.txt");
                for (int i = 0; i < 3; i++)
                {
                    string[] data = lines[i].Split(' ');
                    if (data.Length != 2) throw new FileFormatException();
                    switch (i)
                    {
                        case 0:
                            CardSizeText.Text = data[1];
                            break;
                        case 1:
                            CardSpacingText.Text = data[1];
                            break;
                        case 2:
                            SoundCheckBox.IsChecked = bool.Parse(data[1]); // Ensure the correct index is used
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                CancelButtonClick(new Button(), new RoutedEventArgs());
                return;
            }

            VisualsText.Text = "Visuals";
            CardSizeDesc.Text = "Card size";
            CardSpacingDesc.Text = "Card spacing";
            Title = "Settings";
            RestartOnLanguageChangeDesc.Text = "NO";
        }

        public static bool WriteSettingsFile()
        {
            if (File.Exists(@"settings.txt")) return false;
            try
            {
                string[] data =
                {
                    "Card_size= 100",
                    "Card_spacing= 20",
                    "soundEnabled= true" // Default value
                };
                File.WriteAllLines(@"settings.txt", data);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool WriteSettingsFile(int cardSize, int cardSpacing)
        {
            if (!File.Exists(@"settings.txt")) return false;
            try
            {
                string[] data =
                {
                    $"Card_size= {cardSize}",
                    $"Card_spacing= {cardSpacing}",
                    $"soundEnabled= {soundEnabled.ToString().ToLower()}" // Write the value as true/false
                };
                File.WriteAllLines(@"settings.txt", data);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                CancelButtonClick(new Button(), new RoutedEventArgs());
                return false;
            }
            return true;
        }



        private void CardSizeUpClick(object sender, RoutedEventArgs e)
        {
            int size = Convert.ToInt32(CardSizeText.Text);
            if (size >= 200) return;
            size++;
            CardSizeText.Text = size.ToString();
        }

        private void checkBoxChecked(object sender, RoutedEventArgs e)
        {
            soundEnabled = true;
        }

        private void checkBoxUnchecked(object sender, RoutedEventArgs e)
        {
            soundEnabled = false;
        }

        private void CardSizeDownClick(object sender, RoutedEventArgs e)
        {
            int size = Convert.ToInt32(CardSizeText.Text);
            if (size <= 50) return;
            size--;
            CardSizeText.Text = size.ToString();
        }

        private void CardSpacingUpClick(object sender, RoutedEventArgs e)
        {
            int size = Convert.ToInt32(CardSpacingText.Text);
            if (size >= 60) return;
            size++;
            CardSpacingText.Text = size.ToString();
        }
        private void CardSpacingDownClick(object sender, RoutedEventArgs e)
        {
            int size = Convert.ToInt32(CardSpacingText.Text);
            if (size <= 10) return;
            size--;
            CardSpacingText.Text = size.ToString();
        }



        private void DefaultSettButtonClick(object sender, RoutedEventArgs e)
        {
            CardSizeText.Text = "100";
            CardSpacingText.Text = "20";
            soundEnabled = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ApplyButtonClick(object sender, RoutedEventArgs e)
        {
            if (WriteSettingsFile(Convert.ToInt32(CardSizeText.Text),
                                 Convert.ToInt32(CardSpacingText.Text))
                == false) return;
            Close();

        }
    }
}


