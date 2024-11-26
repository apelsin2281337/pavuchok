
namespace Spider_Solitaire
{
    public partial class SpiderRulesWindow : Window
    {
        
        public Dictionary<string, string[]> rulesContent = new()
        {
            { "English", eng },
            { "Russian", ru }
        };
        private static readonly string[] eng =
                [
                    "Objective:", "Arrange all cards into 8 complete sets of descending sequences, from King to Ace, in the same suit.",
                    "Game Setup:", "Spider Solitaire is played with two decks (104 cards) and 10 columns. Only the top card in each column is face-up initially.",
                    "Rules of Play:", "• You can move a face-up card or a sequence of cards in descending order to another column, if the first card of that column is of the same suit.",
                    "• You can only build sequences in descending order within the same suit (e.g., King to Ace in Spades).",
                    "• When you complete a sequence from King to Ace in one suit, it will be removed from the board.",
                    "Dealing New Cards:", "Tap on the deck to deal a new row of cards to each column. A new row can only be dealt if all columns have at least one card.",
                    "Winning the Game:", "You win by clearing all cards from the board by creating complete sequences.",
                    "Difficulty Modes:", "1-suit, 2-suit, and 4-suit modes are available for varying levels of difficulty."
                ];
        private static readonly string[] ru =
                [
                    "Цель:", "Соберите все карты в 8 наборах у убывающей последовательности от Короля до Туза одной масти.",
                    "Настройка игры:", "Spider Solitaire играется двумя колодами (104 карты) и 10 колонками. Изначально только верхняя карта каждой колонки открыта.",
                    "Правила игры:", "• Вы можете перемещать открытую карту или последовательность карт в другую колонку, если первая карта этой колонки той же масти.",
                    "• Вы можете перемещать последовательности только в пределах одной масти (например, Король до Туза в пиках).",
                    "• Когда вы получаете последовательность от Короля до Туза в одной масти, она удаляется с доски.",
                    "Раздача новых карт:", "Нажмите на колоду, чтобы раздать новый ряд карт для каждой колонки. Новый ряд можно раздать только, если во всех колонках есть хотя бы одна карта.",
                    "Победа в игре:", "Вы выигрываете, убирая все карты с доски, создавая полные последовательности.",
                    "Режимы сложности:", "Доступны режимы 1 масть, 2 масти и 4 масти для разных уровней сложности."
                ];

        public SpiderRulesWindow()
        {
            InitializeComponent();
            LanguageComboBox.SelectedIndex = 0;
            UpdateRulesText("English");
        }

        private void UpdateRulesText(string language)
        {
            RulesTextBlock.Inlines.Clear();
            if (rulesContent.TryGetValue(language, out string[]? rules))
            {
                for (int i = 0; i < rules.Length; i++)
                {
                    if (i % 2 == 0)
                        RulesTextBlock.Inlines.Add(new Run(rules[i]) { FontWeight = FontWeights.Bold });
                    else
                        RulesTextBlock.Inlines.Add(new Run(rules[i]));
                    RulesTextBlock.Inlines.Add(new LineBreak());
                    RulesTextBlock.Inlines.Add(new LineBreak());
                }
            }
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string? selectedLanguage = selectedItem.Content.ToString();
                UpdateRulesText(language: selectedLanguage ?? "English");
            }
        }
        
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
    }
}
