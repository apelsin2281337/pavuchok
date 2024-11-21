namespace Spider_Solitaire
{
    internal class Card
    {
        public readonly int Value;
        public readonly char Colour;
        public bool Visible { get; set; }
        public Image image;
        public Card(int value, char colour, bool visible, int y, int x, int offset, MouseButtonEventHandler CardSelect, float Scale)
        {
            Value = value;
            Colour = colour;
            Visible = visible;
            image = new Image();
            Createimage(y, x, offset, CardSelect, Scale);

        }

        //initialised image properties
        private void Createimage(int y, int x, int offset, MouseButtonEventHandler CardSelect, float Scale)
        {
            GetColour();
            image.Width = Convert.ToInt32(89.0f * Scale);
            image.Height = Convert.ToInt32(120.0f * Scale);
            image.HorizontalAlignment = HorizontalAlignment.Center;
            image.VerticalAlignment = VerticalAlignment.Top;
            image.Visibility = Visibility.Visible;
            image.Margin = new Thickness(0, y * offset + 5, 0, 0);   //y == activeCards[index].Count
            image.MouseLeftButtonUp += new MouseButtonEventHandler(CardSelect);
            string name = "";   //first letter, lowercase, indicates which column index wise the image is at, and the second one, uppercase
                                //indicates on which position y wise the image (card) is
            name += (char)(x + 97); //index == x

            char a = (char)(y + 64);
            char b = 'A';
            while (a > 'Z')
            {
                a -= (char)26;
                b++;
            }
            name += $"{a}{b}";

            image.Name = name;

        }

        //Moves the card up
        public async Task SelectedMove(int y, int cardOffset)
        {
            // Move the card up
            for (int i = 0; i < 15; i += 1)
            {
                image.Margin = new Thickness(0, y * cardOffset + 5 - i, 0, 0);
                await Task.Delay(3);
            }


        }

        /*public async Task InvalidMove(int y, int cardOffset)
        {
            for (int i = 0; i <= 20; i += 2)
            {
                image.Margin = new Thickness(i, y * cardOffset + 5 - i, 0, 0);
                await Task.Delay(10);
            }
            for (int i = 20; i >= -20; i -= 2)
            {
                image.Margin = new Thickness(i, y * cardOffset + 5 - i, 0, 0);
                await Task.Delay(10);
            }
            for (int i = -20; i != 0; i += 2)
            {
                image.Margin = new Thickness(i, y * cardOffset + 5 - i, 0, 0);
                await Task.Delay(10);
            }
        }*/

        public void GetColour()
        {
            image.Source = new BitmapImage(new Uri(@"assets/" + (Visible ? $"{Value}{Colour}" : "uncovered") + ".png", UriKind.Relative));
        }
    }
}
