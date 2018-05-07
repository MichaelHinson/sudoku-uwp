using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SudokuApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HelpPage : Page
    {
        public HelpPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            infoBlock.Text = "How to play: \n" +
                             "Once you hit the start button you are presented with three different difficulty buttons. " +
                             "These buttons determine the number of cells that are hidden. Scoring is based off of " +
                             "difficulty and time. As you are playing you'll notice that the square the cell is in will " +
                             "be highlighted, as well as the row and colmun the cell resides in. This is to help you solve " +
                             "the puzzle and see conflicts. Once the puzzle is finshed hit the finished button. This will " +
                             "result in the button changing to invalid or ending the game and taking you to the score screen " +
                             "if the puzzle is correct. If you wish to see past scores check the stats screen in the main menu.";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void infoBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
