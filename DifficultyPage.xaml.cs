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
    public sealed partial class DifficultyPage : Page
    {
        //public string Difficulty { set; get; }
        public DifficultyPage()
        {
            this.InitializeComponent();
        }

        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            var difficulty = new Difficulty { Setting = "easy" };
            this.Frame.Navigate(typeof(grid), difficulty);
        }

        private void HardButton_Click(object sender, RoutedEventArgs e)
        {
            var difficulty = new Difficulty { Setting = "hard" };
            this.Frame.Navigate(typeof(grid), difficulty);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void MediumButton_Click(object sender, RoutedEventArgs e)
        {
            var difficulty = new Difficulty { Setting = "medium" };
            this.Frame.Navigate(typeof(grid), difficulty);
        }
    }

    class Difficulty
    {
        public string Setting { get; set; }
    }

}

