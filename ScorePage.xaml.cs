using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class ScorePage : Page
    {

        private int UserScore {get; set;}
        private string Time { get; set; }

        public ScorePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var points = (Score)e.Parameter;
            UserScore = points.Points;
            string minutes = points.Min.ToString();
            string seconds = points.Sec.ToString();
            Time = minutes + ":" + seconds;

            if (!points.Visible)
            {
                userScoreBox.Text = "";
            }
            else
            {
                userScoreBox.Text = "Your score: " + UserScore;
            }
            GetPastScoring();
        }

        public async void GetPastScoring()
        {
            string fileName = "score.txt";
            string scores = await GetTextFile(fileName);
            string input = null;
            int index = 0;
            int greatestScore = 0;
            int i = 0;
            int b;
            int c = 1;

            List<int> scoreList = new List<int>();
            List<int> scoreList2 = new List<int>();
            List<string> timeList = new List<string>();
            List<string> timeList2 = new List<string>();
            List<string> dateList = new List<string>();
            List<string> dateList2 = new List<string>();

            //topTenScore.Text += "file word: " + scores + "\n";

            string[] words = scores.Split(',');
            foreach (var word in words)
            {
                //topTenScore.Text += "file: " + word + "\n";
                if (c == 3)
                {
                    if (int.TryParse(word, out b))
                    {
                        scoreList.Add(int.Parse(word));
                        i++;
                    }
                    c = 1;
                }
                else if(c == 2)
                {
                    timeList.Add(word);
                    c++;
                }
                else if(c == 1)
                {
                    if (word.Length == 9)
                    {
                        dateList.Add(word);
                        c++;
                    }
                }
            }

            scoreList.Add(UserScore);
            timeList.Add(Time);
            dateList.Add(DateTime.Now.ToString("M/d/yyyy"));

            while (i < 10)
            {
                scoreList.Add(-1);
                timeList.Add("");
                dateList.Add("");
                i++;
            }

            while (scoreList2.Count < 10)
            {
                greatestScore = 0;
                for (int a = 0; a < scoreList.Count; a++)
                {
                    if(scoreList[a] > greatestScore)
                    {
                        index = a;
                        greatestScore = scoreList[a];
                    }
                }
                scoreList2.Add(scoreList[index]);
                timeList2.Add(timeList[index]);
                dateList2.Add(dateList[index]);
                scoreList.RemoveAt(index);
                timeList.RemoveAt(index);
                dateList.RemoveAt(index);
                index = 0;
            }

            for (int a = 0; a < scoreList2.Count; a++)
            {
                if(scoreList2[a] == -1)
                {
                    topTenScore.Text += (a + 1) + ": " + "\n";
                    input += "";
                }
                else
                {
                    topTenScore.Text += (a + 1) + ": " + " Date: " + dateList2[a] + "," + " Time Remaining: " + timeList2[a] + "," + " Score: " + scoreList2[a] + "\n";
                    input += dateList2[a] + "," + timeList2[a] + "," + scoreList2[a] + ",";
                }
            }
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile scoreFile = await storageFolder.CreateFileAsync("score.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(scoreFile, input);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        public static async Task<string> GetTextFile(string _filename)
        {
            string contents = null;
            StorageFolder localfolder = ApplicationData.Current.LocalFolder;
            if (await localfolder.TryGetItemAsync(_filename) != null)
            {
                StorageFile textfile = await localfolder.GetFileAsync(_filename);
                contents = await FileIO.ReadTextAsync(textfile);
            }
            else
            {
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("score.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            }
            return contents;
        }

        private void userScoreBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void topTenScore_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
