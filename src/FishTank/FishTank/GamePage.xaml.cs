//
// Copyright - James Finlay
// 

using FishTank.Utilities;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FishTank
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        readonly GameController _game;

        public GamePage()
        {
            this.InitializeComponent();

            // Create the game.
            var launchArguments = string.Empty;
            _game = MonoGame.Framework.XamlGame<GameController>.Create(launchArguments, Window.Current.CoreWindow, swapChainPanel);
        }
    }
}
