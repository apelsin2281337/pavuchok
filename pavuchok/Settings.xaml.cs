namespace Spider_Solitaire
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public bool aaaaAaaAAA = false;

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
                for (int i = 0; i < 2; i++)
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
            if (File.Exists(@"settings.txt")) return false; ;
            try
            {
                string[] data =
                [
                    "Card_size= 100",
                    "Card_spacing= 20",
                ];
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
                [
                    $"Card_size= {cardSize}",
                    $"Card_spacing= {cardSpacing}",

                ];
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

        private void AAaAaA(object sender, RoutedEventArgs e)
        {
            const string AaAaAa = "YXBlbHNpbg==";
            const string AaAaAaAa = "UERdVA==";
            string AaAaaA = Encoding.UTF8.GetString(Convert.FromBase64String(AaAaAa));

            string aAaaaA = adminpwbox.Password;

            string AaAaaAaA = aAaAaA(aAaaaA, AaAaaA);

            if (AaAaaAaA == Encoding.UTF8.GetString(Convert.FromBase64String(AaAaAaAa)))
            {
                MessageBox.Show("A");
                aaaaAaaAAA = true;
            }
            else
            {
                MessageBox.Show("a");
            }

            static string aAaAaA(string aAaaaA, string AaAaaA)
            {
                var AaAaaaA = new StringBuilder();
                for (int aAaaA = 0; aAaaA < aAaaaA.Length; aAaaA++)
                {
                    AaAaaaA.Append((char)(aAaaaA[aAaaA] ^ AaAaaA[aAaaA % AaAaaA.Length]));
                }
                return AaAaaaA.ToString();
            }

            if (aaaaAaaAAA)
            {
                
            }
        }

        


    }
}


