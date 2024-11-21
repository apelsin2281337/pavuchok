namespace Spider_Solitaire
{
    public enum CommandType
    {
        select, //Used when cards are selected
        add,    //Used when player adds new cards
        move    //used when selected cards are moved to another pile
    }
    internal class Command
    {
        public readonly CommandType type;
        public string[]? args;

        public Command(CommandType Type, string[]? Arguments)
        {
            type = Type;
            if (Arguments != null)
            {
                try
                {
                    args = new string[Arguments.Length];
                    Arguments.CopyTo(args, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }



        public static void ExecuteSelect(Action<object, MouseButtonEventArgs> CardSelect, string[] args)
        {
            Image img = new() { Name = args[0] };
            CardSelect(img, new MouseButtonEventArgs(InputManager.Current.PrimaryMouseDevice, 0, MouseButton.Left));
        }
        public static void ExecuteMove(Action<object, MouseButtonEventArgs> ColumnClick, string[] args)
        {
            Grid grid = new() { Name = args[0] };
            ColumnClick(grid, new MouseButtonEventArgs(InputManager.Current.PrimaryMouseDevice, 0, MouseButton.Left));
        }
        public static void ExecuteAdd(Action<object, MouseButtonEventArgs> NewCardsClick)
        {
            NewCardsClick(new Image(), new MouseButtonEventArgs(InputManager.Current.PrimaryMouseDevice, 0, MouseButton.Left));
        }
    }
}
