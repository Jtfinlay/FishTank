//
// Copyright - James Finlay
// 

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FishTank
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        readonly AppController _game;

        public GamePage()
        {
            this.InitializeComponent();

            // Create the game.
            var launchArguments = string.Empty;
            _game = MonoGame.Framework.XamlGame<AppController>.Create(launchArguments, Window.Current.CoreWindow, swapChainPanel);
        }
    }
}
