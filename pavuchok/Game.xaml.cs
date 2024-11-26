namespace Spider_Solitaire
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        private int CardOffset { get; set; }  //used to render the cards apart from each other
        private readonly Menu _menu;
        private int moves = 0;
        private readonly DispatcherTimer _timer;
        private int _elapsedTime = 0;
        private int score = 0;
        private MediaPlayer backgroundMusic;
        private double scoremultiplier = 3.0;
        private readonly Settings settings;
        private readonly List<Image> AssembledKingCards = []; //used to keep track of assembled king cards to remove when move undone
        private List<Card> Selected { get; set; } = []; //currenly selected card/s
        private int Selected_x { get; set; } //indexes of the currently selected card (and cards underneath)
        private int NewCardNumber { get; set; } = 1;
        private int DecksSolved { get; set; } = 0;   //number of solved decks


        private readonly Deck deck = new();
        public Game(int numberOfColours, Menu menu)
        {
            InitializeComponent();
            KeepAlive = false;
            _menu = menu;
            settings = new();
            CardOffset = settings.CardSpacing;
            deck.GenerateCards(numberOfColours);
            _ = deck.LayOutStartingCardsRecursive(CardOffset, SolitaireGrid, CardSelect, settings.CardSizeFactor);
            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            _timer.Tick += Timer_Tick;
            _elapsedTime = 0;
            _timer.Start();
            StartBackgroundMusic();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _elapsedTime++;
            int hours = _elapsedTime / 3600;
            int minutes = (_elapsedTime % 3600) / 60;
            int seconds = _elapsedTime % 60;
            Timer.Text = $"Playtime: {hours}:{minutes:D2}:{seconds:D2}"; ;
        }

        //Loads all cards that are being selected into a tmp list "Selected"
        private async void CardSelect(object sender, MouseButtonEventArgs e)
        {
            if (deck.activeCards == null || Selected.Count > 0) return;

            PlaySound("125.mp3");

            int x = ((Image)sender).Name[0] - 97; // Get x from image name
            int y = ((Image)sender).Name[1] - 65; // Get y from image name
            char n = ((Image)sender).Name[2];

            while (n > 'A')
            {
                y += 26; // Adjust y for alphabetical index
                n--;
            }

            bool valid = deck.activeCards[x][y].Visible;

            for (int i = y + 1, tmp = 1; i < deck.activeCards[x].Count && valid; i++, tmp++)
            {
                if (deck.activeCards[x][y].Colour != deck.activeCards[x][i].Colour ||
                    deck.activeCards[x][y].Value != deck.activeCards[x][i].Value + tmp)
                {
                    valid = false;
                }
            }

            if (valid)
            {
                SwichHitRegistration(false);

                // Select new cards
                for (int i = y; i < deck.activeCards[x].Count; i++)
                {
                    deck.activeCards[x][i].SelectedMove(i + 1, CardOffset); // Move selected cards up
                    Selected.Add(deck.activeCards[x][i]);
                }
                deck.activeCards[x].RemoveRange(y, deck.activeCards[x].Count - y);
                Selected_x = x;
            }
        }



        //Handles movement of cards from one column to another
        private async void ColumnClick(object sender, MouseButtonEventArgs e)
        {
            if (Selected == null || Selected.Count == 0) return;

            int column_index = ((Grid)sender).Name[3] - '0';

            // Проверяем, допустим ли ход
            if (((deck.activeCards[column_index].Count == 0 ||
                  deck.activeCards[column_index].Last().Value - 1 == Selected[0].Value) &&
                  column_index != Selected_x))
            {
                // Логика для допустимого хода
                string LastCommandArgsEntry = $"{Selected.First().image.Name[0] - 'a'} ";
                deck.activeCards[column_index].AddRange(Selected);
                foreach (var item in deck.activeCards[column_index])
                {
                    bool flag;
                    if (item.image.Name == Selected[0].image.Name) flag = true;
                    else flag = false;
                    string name = "";
                    name += (char)(column_index + 97);
                    char a = (char)(deck.activeCards[column_index].IndexOf(item) + 65);
                    char b = 'A';
                    while (a > 'Z')
                    {
                        a -= (char)26;
                        b++;
                    }
                    name += $"{a}{b}";

                    item.image.Name = name;

                    if (!flag) continue;
                    LastCommandArgsEntry += name;
                }
                foreach (var item in deck.activeCards[column_index])
                {
                    Grid.SetColumn(item.image, column_index + 1);
                    item.image.Margin = new Thickness(0, (deck.activeCards[column_index].IndexOf(item) + 1) * CardOffset + 5, 0, 0);

                }
                moves++;
                Moves.Text = "Moves: " + moves.ToString();
                score += (int)scoremultiplier;
                Score.Text = $"Score: {score}";
                if (moves % 5 == 0 && scoremultiplier > 1.0)
                {
                    scoremultiplier -= 0.1;
                    if (scoremultiplier < 1.0) scoremultiplier = 1.0; // Минимум 1.0
                }
            }
            else
            {
                // Неверный ход: возвращаем карты и запускаем InvalidMove
                foreach (var card in Selected)
                {
                    await card.InvalidMove();
                }

                deck.activeCards[Selected_x].AddRange(Selected);
                foreach (var item in deck.activeCards[Selected_x])
                {
                    item.image.Margin = new Thickness(0, (deck.activeCards[Selected_x].IndexOf(item) + 1) * CardOffset + 5, 0, 0);
                }
            }

            Selected.Clear();
            IsSuitAssembled();
            Refresh();
            SwichHitRegistration(true);

            if (DecksSolved == 8) Victory();
        }

        private void PlaySound(string soundFileName)
        {
            try
            {
                MediaPlayer soundPlayer = new MediaPlayer();

                // Получение потока встроенного ресурса
                var resourceStream = Application.GetResourceStream(new Uri($"pack://application:,,,/assets/{soundFileName}"));
                if (resourceStream == null)
                {
                    MessageBox.Show($"Sound resource {soundFileName} not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Сохраняем ресурс во временный файл
                string tempFilePath = Path.Combine(Path.GetTempPath(), soundFileName);
                using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    resourceStream.Stream.CopyTo(fileStream);
                }

                // Открываем временный файл для воспроизведения
                soundPlayer.Open(new Uri(tempFilePath, UriKind.Absolute));
                soundPlayer.Volume = settings.soundEnabled ? 1 : 0; // Учитываем настройку звука
                soundPlayer.Play();

                // Удаляем временный файл после завершения воспроизведения
                soundPlayer.MediaEnded += (s, e) =>
                {
                    soundPlayer.Close();
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing sound: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Starts the background music
        private void StartBackgroundMusic()
        {
            try
            {
                backgroundMusic = new MediaPlayer();

                // Получение потока встроенного ресурса
                var resourceStream = Application.GetResourceStream(new Uri("pack://application:,,,/assets/sample.mp3"));
                if (resourceStream == null)
                {
                    MessageBox.Show("Music resource not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Сохраняем ресурс во временный файл
                string tempFilePath = Path.Combine(Path.GetTempPath(), "sample_temp.mp3");
                using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    resourceStream.Stream.CopyTo(fileStream);
                }

                // Открываем временный файл для воспроизведения
                backgroundMusic.Open(new Uri(tempFilePath, UriKind.Absolute));
                if (settings.soundEnabled)
                {
                    backgroundMusic.Volume = 0.3;
                }
                else
                {
                    backgroundMusic.Volume = 0;
                }


                // Настраиваем зацикливание и воспроизведение
                backgroundMusic.MediaOpened += (s, e) =>
                {
                    
                    backgroundMusic.Play();
                    backgroundMusic.MediaEnded += (sender, args) =>
                    {
                        backgroundMusic.Position = TimeSpan.Zero;
                        backgroundMusic.Play();
                    };
                };
                backgroundMusic.MediaFailed += (s, e) =>
                {
                    MessageBox.Show($"Failed to play music: {e.ErrorException?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading background music: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        //Checks whether a deck is solved
        private void IsSuitAssembled()
        {
            for (int i = 0; i < 10; i++) //iterates through all columns
            {
                foreach (var item in deck.activeCards[i])
                {
                    if (item.Visible == false || item.Value != 13) continue;

                    int index = deck.activeCards[i].IndexOf(item);
                    if (index + 13 != deck.activeCards[i].Count) continue;
                    for (int j = index + 1, value = 12; j < deck.activeCards[i].Count; j++, value--)
                    {
                        if (deck.activeCards[i][j].Value != value || deck.activeCards[i][j].Colour != item.Colour) goto EndOfForeachLoop;
                    }

                    for (int j = deck.activeCards[i].Count - 1; j >= index; j--)
                    {
                        deck.activeCards[i][j].image.MouseLeftButtonUp -= CardSelect;
                        SolitaireGrid.Children.Remove(deck.activeCards[i][j].image);
                    }

                    Image image = new()
                    {
                        Width = 89,
                        Height = 120,
                        Source = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "assets/" + $"13{item.Colour}" + ".png")),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Visibility = Visibility.Visible,
                        Margin = new Thickness(-220 + DecksSolved * 15, 0, 0, 0)
                    };
                    SolitaireGrid.Children.Add(image);
                    Grid.SetRow(image, 1);
                    Grid.SetColumn(image, 3);

                    char c = deck.activeCards[i].Last().Colour;

                    deck.activeCards[i].RemoveRange(index, 13); //13 cards in full set
                    DecksSolved++;
                    AssembledKingCards.Add(image);

                    return;
                }
            EndOfForeachLoop:;
            }
        }
        //Handles dealing of new row of cards
        public async void NewCardsClick(object sender, MouseButtonEventArgs e)
        {
            PlaySound("124.mp3");
            for (int i = 0; i < 10; i++)
            {
                if (deck.activeCards[i].Count > 0) continue;
                InformationBox.Text = "All columns should have at least 1 card";
                await Task.Delay(10000);
                InformationBox.Text = " ";
                return;
            }

            Image[] newCardimages = [new1, new2, new3, new4, new5];
            newCardimages[NewCardNumber - 1].Visibility = Visibility.Hidden;
            NewCardNumber++;

            for (int index = 0; index < 10; index++)
            {
                Card card = new(deck.values[deck.cardNum], deck.colors[deck.cardNum], true,
                    deck.activeCards[index].Count + 1, index, CardOffset, CardSelect, settings.CardSizeFactor);
                SolitaireGrid.Children.Add(card.image);
                Grid.SetColumn(card.image, index + 1);
                deck.cardNum++;
                deck.activeCards[index].Add(card);
            }
            Refresh();
        }

        //makes sure that all cards are up and the correct ones are being shown
        private void Refresh()
        {
            if (deck.activeCards == null) return;
            for (int i = 0; i < 10; i++)
            {
                foreach (var item in deck.activeCards[i])
                {
                    SolitaireGrid.Children.Remove(item.image);
                    SolitaireGrid.Children.Add(item.image);
                }
                if (deck.activeCards[i].Count > 0 && deck.activeCards[i].Last().Visible == false)
                {
                    deck.activeCards[i].Last().Visible = true;
                    deck.activeCards[i].Last().GetColour();
                }
            }
        }

        //switches hittestvisible property of cards
        private void SwichHitRegistration(bool hit)
        {
            for (int i = 0; i < 10; i++)
            {
                foreach (var item in deck.activeCards[i])
                {
                    item.image.IsHitTestVisible = hit;
                }
            }
        }

        //hanldes victory "screen"
        private async void Victory()
        {
            Exit.IsEnabled = false;
            await Task.Delay(500);
            VictoryText.Text = "Victory!";
            VictoryText.Visibility = Visibility.Visible;
            await Task.Delay(5000);
            NavigationService.Navigate(_menu);
            _timer.Stop();
            backgroundMusic?.Stop();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(_menu);
            backgroundMusic.Stop();
            string tempFilePath = Path.Combine(Path.GetTempPath(), "sample_temp.mp3");
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }

        private class Settings
        {
            public float CardSizeFactor { get; set; }
            public int CardSpacing { get; set; }
            public bool soundEnabled { get; set; }


            public Settings()
            {
                LoadSettings();
            }

            private bool LoadSettings()
            {
                if (!File.Exists(@"settings.txt")) return false;
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
                                CardSizeFactor = (float)Convert.ToDouble(data[1]) / 100.0f;
                                break;
                            case 1:
                                CardSpacing = Convert.ToInt32(data[1]);
                                break;
                            case 2:
                                soundEnabled = bool.Parse(data[1]);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                return true;
            }
        }
    }
}
