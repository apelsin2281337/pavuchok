namespace Spider_Solitaire
{
    internal class Deck
    {
        public int[] values = new int[8 * 13];  //8 total columns with 13 cards each
        public char[] colors = new char[8 * 13];
        public int cardNum = 0; //used to iterate throgh the deck while handing out new card
        public List<Card>[] activeCards = new List<Card>[10];   //array of lists containing currently held out cards
        private readonly Random random = new();

        public Deck()
        {
            for (int i = 0; i < 10; i++) activeCards[i] = [];
            cardNum = 0;
        }

        //randomly generates the card deck from with the cards are given out in order
        public void GenerateCards(in int numberOfColors)
        {
            bool picked;
            int[] coloursPool = new int[numberOfColors * 13];
            for (int i = 0; i < numberOfColors * 13; i++) coloursPool[i] = 8 / numberOfColors;

            for (int i = 0; i < 8 * 13; i++)
            {
                picked = false;
                while (!picked)
                {
                    int rng = random.Next(coloursPool.Length);
                    if (coloursPool[rng] > 0)
                    {
                        if (rng >= 3 * 13) colors[i] = 'b';
                        else if (rng >= 2 * 13) colors[i] = 'a';
                        else if (rng >= 1 * 13) colors[i] = 'd';
                        else colors[i] = 'c';
                        values[i] = rng % 13 + 1;
                        coloursPool[rng]--;
                        picked = true;
                    }
                }
            }
            if (!IsValidDeck()) GenerateCards(numberOfColors);
        }

        //method determines whether the generated deck is valid according to a generation ruleset
        private bool IsValidDeck()
        {
            //3 or more of the same card beside each other
            for (int i = 0; i < (8 * 13) - 2; i++)
            {
                if (colors[i] == colors[i + 1] && colors[i] == colors[i + 2] &&
                   values[i] == values[i + 1] && values[i] == values[i + 2]) return false;
            }

            //4 or more same cards in the same row (starting from the first row, fifth card)
            for (int i = 4; i < 8 * 13; i += 10)  //jumps from line to line
            {
                for (int j = i; j < i + 7; j++)  //itterates thrugh current line 
                {
                    int count = 1;
                    for (int k = j + 1; k < i + 10; k++)    //itterates through the rest of the line
                    {
                        if (colors[k] == colors[j] && values[k] == values[j]) count++;
                    }
                    if (count > 3) return false;
                }
            }

            //5 or more of the same values of cards in the same row (starting from the first row, fifth card)
            for (int i = 4; i < 8 * 13; i += 10)  //jumps from line to line
            {
                for (int j = i; j < i + 5; j++)  //itterates thrugh current line 
                {
                    int count = 1;
                    for (int k = j + 1; k < i + 10; k++)    //itterates through the rest of the line
                    {
                        if (values[k] == values[j]) count++;
                    }
                    if (count > 4) return false;
                }
            }

            //max 3 kings allowed after 80th card
            for (int i = 76, count = 0; i < 8 * 13; i++)
            {
                if (values[i] == 13) count++;
                if (count > 3) return false;
            }

            //at least 4 different values in one line after 54th card
            for (int i = 54; i < 8 * 13; i += 10)
            {
                int[] cardValues = new int[10];
                for (int j = i, index = 0; j < i + 10; j++, index++)
                {
                    cardValues[index] = values[j];
                }
                Array.Sort(cardValues);
                int n = 1;
                for (int j = 0; j < 9; j++)
                {
                    if (cardValues[j] != cardValues[j + 1]) n++;
                }
                if (n < 4) return false;
            }


            return true;
        }

        // Method to Lay out the cards that the game starts with onto the game field
        public async Task LayOutStartingCardsRecursive(int cardOffset, Grid SolitaireGrid, MouseButtonEventHandler CardSelect, float Scale)
        {
            int index = cardNum % 10;
            Card card = new(values[cardNum], colors[cardNum], cardNum > 43,
                activeCards[index].Count + 1, index, cardOffset, CardSelect, Scale);
            if (card == null) return;
            activeCards[index].Add(card);
            SolitaireGrid.Children.Add(card.image);
            Grid.SetColumn(card.image, index + 1);
            await Task.Delay(10);
            cardNum++;
            if (cardNum < 54) await LayOutStartingCardsRecursive(cardOffset, SolitaireGrid, CardSelect, Scale);
        }
    }
}
