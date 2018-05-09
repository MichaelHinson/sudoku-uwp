
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
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
    public sealed partial class grid : Page
    {

        private string DificultySetting;
        private String selectedNumber = " "; //Used to determine what to input in a clicked cell
        DispatcherTimer dispatcherTimer;
        DateTimeOffset startTime;
        DateTimeOffset lastTime;
        DateTimeOffset stopTime;
        int timesTicked = 1;
        int timesToTick = 1800;
        int min = 30;
        int sec = 0;

        public Button[] buttonArray { get; set; }
        public ButtonWithInfo[] buttonWithInfoArray { get; set; }
        public grid()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var set = (Difficulty)e.Parameter;
            DificultySetting = set.Setting;
            generatePuzzle();
            DispatcherTimerSetup();
        }

        public void SetDificultySetting(string setting)
        {
            DificultySetting = setting;
        }
        public void generatePuzzle()
        {
            buttonWithInfoArray = new ButtonWithInfo[81];
            buttonArray = new Button[]{
                 c1,  c2,  c3,    c4,  c5,  c6,   c7,  c8,  c9,
                c10, c11, c12,   c13, c14, c15,  c16, c17, c18,
                c19, c20, c21,   c22, c23, c24,  c25, c26, c27,

                c28, c29, c30,   c31, c32, c33,  c34, c35, c36,
                c37, c38, c39,   c40, c41, c42,  c43, c44, c45,
                c46, c47, c48,   c49, c50, c51,  c52, c53, c54,

                c55, c56, c57,   c58, c59, c60,  c61, c62, c63,
                c64, c65, c66,   c67, c68, c69,  c70, c71, c72,
                c73, c74, c75,   c76, c77, c78,  c79, c80, c81
                                    };
            //Attaches event handler to all cell buttons
            foreach (var button in buttonArray)
                button.Click += c_Button_Click;
            for (int i = 0; i < buttonArray.Length; i++)
            {
                buttonWithInfoArray[i] = new ButtonWithInfo
                {
                    button = buttonArray[i],
                    Index = i,
                    Row = i / 9 + 1,
                    Column = i % 9 + 1
                };
                //Top 3 Squares
                if (i / 9 <= 2 && i % 9 <= 2)
                    buttonWithInfoArray[i].Square = 1;
                else if (i / 9 <= 2 && i % 9 <= 5)
                    buttonWithInfoArray[i].Square = 2;
                else if (i / 9 <= 2 && i % 9 <= 8)
                    buttonWithInfoArray[i].Square = 3;
                //Middle 3 Squares
                else if (i / 9 <= 5 && i % 9 <= 2)
                    buttonWithInfoArray[i].Square = 4;
                else if (i / 9 <= 5 && i % 9 <= 5)
                    buttonWithInfoArray[i].Square = 5;
                else if (i / 9 <= 5 && i % 9 <= 8)
                    buttonWithInfoArray[i].Square = 6;
                //Bottom 3 Squares
                else if (i / 9 <= 8 && i % 9 <= 2)
                    buttonWithInfoArray[i].Square = 7;
                else if (i / 9 <= 8 && i % 9 <= 5)
                    buttonWithInfoArray[i].Square = 8;
                else if (i / 9 <= 8 && i % 9 <= 8)
                    buttonWithInfoArray[i].Square = 9;
            }
            Board newBoard = new Board(true);//True fills the board completely

            for (int counter = 0; counter < 81; counter++)
            {
                buttonArray[counter].Content = newBoard.CellArray[counter].Value.ToString();
            }

            if (DificultySetting == "easy")
            {
                List<int> blankCells = Difficulty(40);//81-40 = 41
                for (int item = 0; item < blankCells.Count; item++)
                {
                    FillBlankCells(blankCells);
                } // Will leave 41 numbers visible.

            }
            else if (DificultySetting == "medium")
            {
                List<int> blankCells = Difficulty(51);//81-51 = 30
                for (int item = 0; item < blankCells.Count; item++)
                {
                    FillBlankCells(blankCells);
                }// Will leave 30 numbers visible.
            }
            else if (DificultySetting == "hard")
            {
                List<int> blankCells = Difficulty(56);//81-56 = 25
                for (int item = 0; item < blankCells.Count; item++)
                {
                    FillBlankCells(blankCells);
                }
                // Will leave 25 numbers visible.
            }
        }
        public void FillBlankCells(List<int> blankCells)
        {
            for (int item = 0; item < blankCells.Count; item++)
            {
                buttonWithInfoArray[blankCells[item]].button.Content = " ";
                buttonWithInfoArray[blankCells[item]].button.FontWeight = FontWeights.Bold;
                buttonWithInfoArray[blankCells[item]].button.FontStyle = FontStyle.Italic;
                buttonWithInfoArray[blankCells[item]].Editable = true;
            }
        }
        public List<int> Difficulty(int blanks)
        {
            Random randNum = new Random();
            List<int> blankCells = new List<int>(); // To store the indexes of cells that will be blank.
            for (int i = 1; i <= 9; i++)//This loop creates 3 blank cells in each 3x3 area to avoid completely filled squares
            {
                int num;
                for (int innerLoop = 0; innerLoop < 3; innerLoop++)
                {
                    do
                    {
                        num = randNum.Next(0, 81);
                    } while (buttonWithInfoArray[num].Square != i || isSame(num, blankCells));
                    blankCells.Add(num);
                }
            }
            for (int count = 0; count < blanks - 27; count++)//This loop randomly removes the remaining blanks required for the difficulty setting
            {
                int num;
                //bool oneCellLeftInSquare = false;
                do
                {
                    num = randNum.Next(0, 81);
                } while (isSame(num, blankCells) || IsOneCellLeft(blankCells, buttonWithInfoArray[num].Square));
                blankCells.Add(num);
            }

            return blankCells;
        }
        public bool IsOneCellLeft(List<int> list, int square)//Ensures that there will not be a completely empty 3x3 area
        {
            int count = 0;
            for (int i = 0; i < 81; i++)
                if (buttonWithInfoArray[i].Square == square && buttonWithInfoArray[i].button.Content.ToString() == " ")
                    count++;
            return (count == 8);

        }
        public bool isSame(int number, List<int> array) // This method will help ensure that we do not duplicate blanks.
        {
            foreach (var item in array)
            {
                if (item == number)
                {
                    return true;
                }
            }
            return false;
        }
        public bool Conflict(int currentIndex)//Checks if all Check functions returned true or not
        {
            return (RowCheck(currentIndex) && ColumnCheck(currentIndex) && SquareCheck(currentIndex));
        }
        public bool RowCheck(int currentIndex)//Return true if no conflicts
        {
            for (int i = 0; i < buttonWithInfoArray.Length; i++)//Refer to Square Check
            {
                if (buttonWithInfoArray[i].button.Content.ToString() != " ")
                {
                    if (buttonWithInfoArray[i].Row == buttonWithInfoArray[currentIndex].Row && i != buttonWithInfoArray[currentIndex].Index)
                    {
                        if (int.Parse(buttonWithInfoArray[i].button.Content.ToString()) == int.Parse(buttonWithInfoArray[currentIndex].button.Content.ToString()))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public bool ColumnCheck(int currentIndex)//Return true if no conflicts
        {
            for (int i = 0; i < buttonWithInfoArray.Length; i++)//Refer to Square Check
            {
                if (buttonWithInfoArray[i].button.Content.ToString() != " ")
                {
                    if (buttonWithInfoArray[i].Column == buttonWithInfoArray[currentIndex].Column && i != buttonWithInfoArray[currentIndex].Index)
                    {
                        if (int.Parse(buttonWithInfoArray[i].button.Content.ToString()) == int.Parse(buttonWithInfoArray[currentIndex].button.Content.ToString()))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        public bool SquareCheck(int currentIndex)//Return true if no conflicts
        {
            for (int i = 0; i < buttonWithInfoArray.Length; i++)//Refer to Square Check
            {
                if (buttonWithInfoArray[i].button.Content.ToString() != " ")
                {
                    if (buttonWithInfoArray[i].Square == buttonWithInfoArray[currentIndex].Square && i != buttonWithInfoArray[currentIndex].Index)
                    {
                        if (int.Parse(buttonWithInfoArray[i].button.Content.ToString()) == int.Parse(buttonWithInfoArray[currentIndex].button.Content.ToString()))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;//Nothing triggered it is a valid number
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        private void set_Button_Click(object sender, RoutedEventArgs e)
        {
            eraseToggle.IsOn = false;
            selectedNumber = (sender as Button).Content.ToString();
        }
        private void c_Button_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;//Will be used to determine the location of the clicked button
            for (int i = 0; i < buttonWithInfoArray.Length; i++)
            {
                if (buttonWithInfoArray[i].button == (sender as Button))//Determines which button was clicked on the board
                    index = i;
                //Resets all squares to original color
                if (buttonWithInfoArray[i].Square == 2 || buttonWithInfoArray[i].Square == 4 ||
                    buttonWithInfoArray[i].Square == 6 || buttonWithInfoArray[i].Square == 8)
                    buttonWithInfoArray[i].button.Background = null;
                else
                    buttonWithInfoArray[i].button.Background = (SolidColorBrush)Resources["DefaultColor"];//Need to set background of other squares to remove highlighting
            }
            if (eraseToggle.IsOn)
            {
                if (buttonWithInfoArray[index].Editable)
                    (sender as Button).Content = " ";
            }
            else
            {
                if (buttonWithInfoArray[index].Editable && selectedNumber != " ")//Sets value to a selected number
                    (sender as Button).Content = selectedNumber;

            }
            HighlightSquares(index);
            (sender as Button).Background = (SolidColorBrush)Resources["GreenColor"];
            if (selectedNumber != " " && !Conflict(index) && hintToggle.IsOn)
                buttonWithInfoArray[index].button.Background = (SolidColorBrush)Resources["RedColor"];

        }
        public void HighlightSquares(int index)//Highlights relevant row square and column
        {
            for (int i = 0; i < buttonWithInfoArray.Length; i++)
            {
                if (buttonWithInfoArray[i].Row == buttonWithInfoArray[index].Row)//Highlights related row
                {
                    if (hintToggle.IsOn && selectedNumber != " " && buttonWithInfoArray[index].button.Content.ToString() == buttonWithInfoArray[i].button.Content.ToString() && i != index)
                        buttonWithInfoArray[i].button.Background = (SolidColorBrush)Resources["RedColor"];
                    else
                        buttonWithInfoArray[i].button.Background = (SolidColorBrush)Resources["BlueColor"];
                }
                if (buttonWithInfoArray[i].Column == buttonWithInfoArray[index].Column)//Highlights related column
                {
                    if (hintToggle.IsOn && selectedNumber != " " && buttonWithInfoArray[index].button.Content.ToString() == buttonWithInfoArray[i].button.Content.ToString() && i != index)
                        buttonWithInfoArray[i].button.Background = (SolidColorBrush)Resources["RedColor"];
                    else
                        buttonWithInfoArray[i].button.Background = (SolidColorBrush)Resources["BlueColor"];
                }
                if (buttonWithInfoArray[i].Square == buttonWithInfoArray[index].Square)//Highlights related square(9x9 area)
                {
                    if (hintToggle.IsOn && selectedNumber != " " && buttonWithInfoArray[index].button.Content.ToString() == buttonWithInfoArray[i].button.Content.ToString() && i != index)
                        buttonWithInfoArray[i].button.Background = (SolidColorBrush)Resources["RedColor"];
                    else
                        buttonWithInfoArray[i].button.Background = (SolidColorBrush)Resources["BlueColor"];
                }
            }
        }
        private void eraseToggle_Toggled(object sender, RoutedEventArgs e)
        {
            selectedNumber = " ";
        }
        private void Button_Click(object sender, RoutedEventArgs e) // Submits the user's solution for validation - Displays "Winner" as the button's content value.
        {
            Board submittedBoard = new Board();         
            bool valid = true;
            for (int count = 0; count < buttonWithInfoArray.Length; count++)// This loop assigns the button's text value to the new board
            {
                if (buttonWithInfoArray[count].button.Content.ToString() == " ")
                    submittedBoard.CellArray[count].Value = 0;
                else
                    submittedBoard.CellArray[count].Value = int.Parse(buttonWithInfoArray[count].button.Content.ToString());
            }
            for (int count = 0; count < buttonWithInfoArray.Length; count++) //Utilizes the Conflict method to check for errors on the submitted board
            {                                                        
                valid = submittedBoard.Conflict(submittedBoard.CellArray[count]);
                if (!valid)
                {
                    count = 80;
                }
            }
            if (valid)
            {
                ___SubmitButton_.IsEnabled = false;
                ___SubmitButton_.Content = "Winner";
                if (min == 0 && sec == 0)
                {
                    var score = new Score { Points = 0, Visible = true, Min = min, Sec = sec };
                    this.Frame.Navigate(typeof(ScorePage), score);
                }
                else if(DificultySetting == "easy")
                {
                    var score = new Score { Points = ((min * 100) + sec) * 2, Visible = true, Min = min, Sec = sec};
                    this.Frame.Navigate(typeof(ScorePage), score);
                }
                else if (DificultySetting == "medium")
                {
                    var score = new Score { Points = ((min * 100) + sec) * 3, Visible = true, Min = min, Sec = sec };
                    this.Frame.Navigate(typeof(ScorePage), score);
                }
                else if (DificultySetting == "hard")
                {
                    var score = new Score { Points = ((min * 100) + sec) * 4, Visible = true, Min = min, Sec = sec };
                    this.Frame.Navigate(typeof(ScorePage), score);
                }
            }
            else if (!valid)
            {
                ___SubmitButton_.Content = "Keep Trying";
            }
        }
        public void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            startTime = DateTimeOffset.Now;
            lastTime = startTime;
            dispatcherTimer.Start();
        }
        void dispatcherTimer_Tick(object sender, object e)
        {
            DateTimeOffset time = DateTimeOffset.Now;
            TimeSpan span = time - lastTime;
            lastTime = time;
            timesTicked++;
            if(sec == 0)
            {
                min -= 1;
                sec = 59;
            }
            else
            {
                sec -= 1;
            }

            if (sec < 10)
            {
                Timer.Text = min + ":0" + sec;
            }
            else
            {
                Timer.Text = min + ":" + sec;
            }
            if (timesTicked > timesToTick)
            {
                stopTime = time;
                dispatcherTimer.Stop();
                span = stopTime - startTime;
            }
        }
        private void TimerLog_TextChanged(object sender, TextChangedEventArgs e)
        {      }
        private void Timer_TextChanged(object sender, TextChangedEventArgs e)
        {   }
    }

}
