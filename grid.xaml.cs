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
        private String selectedNumber = " ";
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
            //testOutput.Clear();
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
            Board newBoard = new Board();

            for (int counter = 0; counter < 81; counter++)
            {
                buttonArray[counter].Content = newBoard.CellArray[counter].Value.ToString();
            }

            if (DificultySetting == "easy")
            {
                List<int> blankCells = Difficulty(40);
                for (int item = 0; item < blankCells.Count; item++)
                {
                    buttonWithInfoArray[blankCells[item]].button.Content = " ";
                    buttonWithInfoArray[blankCells[item]].button.FontWeight = FontWeights.Bold;
                    buttonWithInfoArray[blankCells[item]].button.FontStyle = FontStyle.Italic;
                    buttonWithInfoArray[blankCells[item]].Editable = true;
                } // Will leave 41 numbers visible.

            }
            else if (DificultySetting == "medium")
            {
                List<int> blankCells = Difficulty(51);
                for (int item = 0; item < blankCells.Count; item++)
                {
                    buttonWithInfoArray[blankCells[item]].button.Content = " ";
                    buttonWithInfoArray[blankCells[item]].button.FontWeight = FontWeights.Bold;
                    buttonWithInfoArray[blankCells[item]].button.FontStyle = FontStyle.Italic;
                    buttonWithInfoArray[blankCells[item]].Editable = true;
                }// Will leave 30 numbers visible.
            }
            else if (DificultySetting == "hard")
            {
                List<int> blankCells = Difficulty(56);
                for (int item = 0; item < blankCells.Count; item++)
                {
                    buttonWithInfoArray[blankCells[item]].button.Content = " ";
                    buttonWithInfoArray[blankCells[item]].button.FontWeight = FontWeights.Bold;
                    buttonWithInfoArray[blankCells[item]].button.FontStyle = FontStyle.Italic;
                    buttonWithInfoArray[blankCells[item]].Editable = true;
                }
                // Will leave 25 numbers visible.
            }
        }
        public List<int> Difficulty(int blanks)
        {
            Random randNum = new Random();
            List<int> blankCells = new List<int>(); // To store the indexes of cells that will be blank.
            for (int i = 1; i <= 9; i++)
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
            for (int count = 0; count < blanks - 27; count++)
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
        public bool IsOneCellLeft(List<int> list, int square)
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
        public class Board
        {
            public Cell[] CellArray { get; set; }//Creates array for cells
            Random rand = new Random();//Creates a seeded random number
            public Board()
            {
                CellArray = new Cell[81];//Establishes length of cell Array
                for (int i = 0; i < CellArray.Length; i++)//Initializes all Cell's
                {
                    CellArray[i] = new Cell();
                }
                PopulateCellArray();
                PopulateBoard();


            }
            public void PopulateCellArray()//Assigns index, row, column, and square info to each cell
            {
                for (int i = 0; i < 81; i++)
                {
                    CellArray[i].Index = i;
                    CellArray[i].Row = i / 9 + 1;
                    CellArray[i].Column = i % 9 + 1;
                    //Top 3 Squares
                    if (i / 9 <= 2 && i % 9 <= 2)
                        CellArray[i].Square = 1;
                    else if (i / 9 <= 2 && i % 9 <= 5)
                        CellArray[i].Square = 2;
                    else if (i / 9 <= 2 && i % 9 <= 8)
                        CellArray[i].Square = 3;
                    //Middle 3 Squares
                    else if (i / 9 <= 5 && i % 9 <= 2)
                        CellArray[i].Square = 4;
                    else if (i / 9 <= 5 && i % 9 <= 5)
                        CellArray[i].Square = 5;
                    else if (i / 9 <= 5 && i % 9 <= 8)
                        CellArray[i].Square = 6;
                    //Bottom 3 Squares
                    else if (i / 9 <= 8 && i % 9 <= 2)
                        CellArray[i].Square = 7;
                    else if (i / 9 <= 8 && i % 9 <= 5)
                        CellArray[i].Square = 8;
                    else if (i / 9 <= 8 && i % 9 <= 8)
                        CellArray[i].Square = 9;
                }
            }
            public void PopulateBoard()//Allocates numbers to each cell
            {
                for (int i = 0; i < 81; i++)
                {
                    FixConflict(i);
                }
            }
            public void FixConflict(int i)//Uses recursion to backtrack if all available numbers fail
            {
                bool valid = false;//Prevents many checks
                while (!valid)
                {
                    if (CellArray[i].AvailableValues.Count == 0)
                    {
                        CellArray[i].PopulateAvailableValues();
                        CellArray[i].Value = 0;
                        FixConflict(i - 1);
                    }
                    int randomNumber = RandomNumber(0, CellArray[i].AvailableValues.Count);//Gets a random number from 0 to highest index available
                    CellArray[i].Value = CellArray[i].AvailableValues[randomNumber];//Uses random number for index for available numbers to assign value
                    valid = Conflict(CellArray[i]);
                    CellArray[i].AvailableValues.RemoveAt(randomNumber);
                }

            }
            public bool Conflict(Cell cell)//Checks if all Check functions returned true or not
            {
                return (RowCheck(cell) && ColumnCheck(cell) && SquareCheck(cell));
            }
            public bool RowCheck(Cell cell)//Return true if no conflicts
            {
                for (int i = 0; i < CellArray.Length; i++)//Refer to Square Check
                {
                    if (CellArray[i].Row == cell.Row && i != cell.Index)
                    {
                        if (CellArray[i].Value == cell.Value)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            public bool ColumnCheck(Cell cell)//Return true if no conflicts
            {
                for (int i = 0; i < CellArray.Length; i++)//Refer to SquareCheck
                {
                    if (CellArray[i].Column == cell.Column && i != cell.Index)
                    {
                        if (CellArray[i].Value == cell.Value)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            public bool SquareCheck(Cell cell)//Return true if no conflicts
            {
                for (int i = 0; i < CellArray.Length; i++)
                {
                    if (CellArray[i].Square == cell.Square && i != cell.Index)//If the checking cell is in the same square and not at the same index(itself)
                    {
                        if (CellArray[i].Value == cell.Value)//If the checking cell contains the same value it is not valid
                        {
                            return false;
                        }
                    }
                }
                return true;//Nothing triggered it is a valid number
            }
            public int RandomNumber(int minValue, int maxValue)//Returns a random number between minValue and maxValue - 1
            {
                return rand.Next(minValue, maxValue);
            }
        }
        public class Cell
        {
            public int Value { get; set; }//Stores the value that will show on the board
            public int Square { get; set; }//Stores the 3x3 region the cell is located in
            public List<int> AvailableValues { get; set; }//Stores the list of available values for the cell
            public int Row { get; set; }//Stores the cell's row
            public int Column { get; set; }//Stores the cell's Column
            public int Index { get; set; }//Stores the cell's index in the CellArray

            public Cell()
            {
                Value = 0;
                Square = 0;
                AvailableValues = new List<int>();
                PopulateAvailableValues();
                Row = 0;
                Column = 0;
                Index = 0;
            }
            public void PopulateAvailableValues()//Populates available values to contain 1-9
            {
                for (int i = 1; i < 10; i++)
                {
                    this.AvailableValues.Add(i);
                }
            }
        }
        public class ButtonWithInfo
        {
            public Button button;
            public int Row;
            public int Column;
            public int Square;
            public int Index;
            public bool Editable;
            public ButtonWithInfo()
            {
                button = null;
                Row = 0;
                Column = 0;
                Square = 0;
                Index = 0;
                Editable = false;
            }
        }
        public class CompletedBoard
        {
            public Cell[] CellArray { get; set; }//Creates array for cells
            public CompletedBoard()
            {
                CellArray = new Cell[81];//Establishes length of cell Array
                for (int i = 0; i < CellArray.Length; i++)//Initializes all Cell's
                {
                    CellArray[i] = new Cell();
                }
                PopulateCellArray();

            }
            public void PopulateCellArray()//Assigns index, row, column, and square info to each cell
            {
                for (int i = 0; i < 81; i++)
                {
                    CellArray[i].Index = i;
                    CellArray[i].Row = i / 9 + 1;
                    CellArray[i].Column = i % 9 + 1;
                    //Top 3 Squares
                    if (i / 9 <= 2 && i % 9 <= 2)
                        CellArray[i].Square = 1;
                    else if (i / 9 <= 2 && i % 9 <= 5)
                        CellArray[i].Square = 2;
                    else if (i / 9 <= 2 && i % 9 <= 8)
                        CellArray[i].Square = 3;
                    //Middle 3 Squares
                    else if (i / 9 <= 5 && i % 9 <= 2)
                        CellArray[i].Square = 4;
                    else if (i / 9 <= 5 && i % 9 <= 5)
                        CellArray[i].Square = 5;
                    else if (i / 9 <= 5 && i % 9 <= 8)
                        CellArray[i].Square = 6;
                    //Bottom 3 Squares
                    else if (i / 9 <= 8 && i % 9 <= 2)
                        CellArray[i].Square = 7;
                    else if (i / 9 <= 8 && i % 9 <= 5)
                        CellArray[i].Square = 8;
                    else if (i / 9 <= 8 && i % 9 <= 8)
                        CellArray[i].Square = 9;
                }
            }


            public bool Conflict(Cell cell)//Checks if all Check functions returned true or not
            {
                return (RowCheck(cell) && ColumnCheck(cell) && SquareCheck(cell));
            }
            public bool RowCheck(Cell cell)//Return true if no conflicts
            {
                for (int i = 0; i < CellArray.Length; i++)//Refer to Square Check
                {
                    if (CellArray[i].Row == cell.Row && i != cell.Index)
                    {
                        if (CellArray[i].Value == cell.Value)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            public bool ColumnCheck(Cell cell)//Return true if no conflicts
            {
                for (int i = 0; i < CellArray.Length; i++)//Refer to SquareCheck
                {
                    if (CellArray[i].Column == cell.Column && i != cell.Index)
                    {
                        if (CellArray[i].Value == cell.Value)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            public bool SquareCheck(Cell cell)//Return true if no conflicts
            {
                for (int i = 0; i < CellArray.Length; i++)
                {
                    if (CellArray[i].Square == cell.Square && i != cell.Index)//If the checking cell is in the same square and not at the same index(itself)
                    {
                        if (CellArray[i].Value == cell.Value)//If the checking cell contains the same value it is not valid
                        {
                            return false;
                        }
                    }
                }
                return true;//Nothing triggered it is a valid number
            }
        }
        public bool Conflict(int currentIndex)//Checks if all Check functions returned true or not
        {
            return (RowCheck2(currentIndex) && ColumnCheck2(currentIndex) && SquareCheck2(currentIndex));
        }
        public bool RowCheck2(int currentIndex)//Return true if no conflicts
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
        public bool ColumnCheck2(int currentIndex)//Return true if no conflicts
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
        public bool SquareCheck2(int currentIndex)//Return true if no conflicts
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
            int index = 0;
            //Removes Highlighting
            for (int i = 0; i < buttonWithInfoArray.Length; i++)
            {
                if (buttonWithInfoArray[i].button == (sender as Button))
                    index = i;
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
                //Sets value to a selected number
                if (buttonWithInfoArray[index].Editable && selectedNumber != " ")
                    (sender as Button).Content = selectedNumber;

                //Highlights relevant row square and column
                for (int i = 0; i < buttonWithInfoArray.Length; i++)
                {
                    if (buttonWithInfoArray[i].Row == buttonWithInfoArray[index].Row)//Highlights related row
                    {
                        if (selectedNumber != " " && buttonWithInfoArray[index].button.Content == buttonWithInfoArray[i].button.Content && i != index && hintToggle.IsOn)
                            buttonWithInfoArray[i].button.Background = (SolidColorBrush)Resources["RedColor"];
                        else
                            buttonWithInfoArray[i].button.Background = (SolidColorBrush)Resources["BlueColor"];
                    }
                    if (buttonWithInfoArray[i].Column == buttonWithInfoArray[index].Column)//Highlights related column
                    {
                        if (selectedNumber != " " && buttonWithInfoArray[index].button.Content == buttonWithInfoArray[i].button.Content && i != index && hintToggle.IsOn)
                            buttonWithInfoArray[i].button.Background = (SolidColorBrush)Resources["RedColor"];
                        else
                            buttonWithInfoArray[i].button.Background = (SolidColorBrush)Resources["BlueColor"];
                    }
                    if (buttonWithInfoArray[i].Square == buttonWithInfoArray[index].Square)//Highlights related square(9x9 area)
                    {
                        if (selectedNumber != " " && buttonWithInfoArray[index].button.Content == buttonWithInfoArray[i].button.Content && i != index && hintToggle.IsOn)
                            buttonWithInfoArray[i].button.Background = (SolidColorBrush)Resources["RedColor"];
                        else
                            buttonWithInfoArray[i].button.Background = (SolidColorBrush)Resources["BlueColor"];
                    }
                }
            }
            (sender as Button).Background = (SolidColorBrush)Resources["GreenColor"];
            if (selectedNumber != " " && !Conflict(index) && hintToggle.IsOn)
                buttonWithInfoArray[index].button.Background = (SolidColorBrush)Resources["RedColor"];
        }
        private void eraseToggle_Toggled(object sender, RoutedEventArgs e)
        {
            selectedNumber = " ";
        }
        private void Button_Click(object sender, RoutedEventArgs e) // Submits the user's solution for validation - Displays "Winner" as the button's content value.
        {
            CompletedBoard final = new CompletedBoard();            // A modified version of Board
            bool valid = true;
            Button[] buttonArray =   {
                                        c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12, c13, c14, c15, c16, c17, c18, c19, c20,
                                        c21, c22, c23, c24, c25, c26, c27, c28, c29, c30, c31, c32, c33, c34, c35, c36, c37, c38, c39,
                                        c40, c41, c42, c43, c44, c45, c46, c47, c48, c49, c50, c51, c52, c53, c54, c55, c56, c57, c58,
                                        c59, c60, c61, c62, c63, c64, c65, c66, c67, c68, c69, c70, c71, c72, c73, c74, c75, c76, c77,
                                        c78, c79, c80, c81
                                      };
            for (int count = 0; count < buttonArray.Length; count++) // This loop assigns the button's text value to the new
            {                                                        // CellArray[count], and uses the Conflict method to test validity. 
                if ((string)buttonArray[count].Content == (" "))     // If the cell's text value is blank, we insert a 0 to ensure
                {                                                    // valid is set to false.
                    buttonArray[count].Content = "0";
                }
                final.CellArray[count].Value = Convert.ToInt32(buttonArray[count].Content);
                valid = final.Conflict(final.CellArray[count]);
                if (!valid)
                {
                    buttonArray[count].Content = " ";
                    count = 80;
                }
            }
            if (valid)
            {
                ___SubmitButton_.IsEnabled = false;
                ___SubmitButton_.Content = "Winner";
                if (min == 0 && sec == 0)
                {
                    //dispatcherTimer.Stop();
                    var score = new Score { Points = 0, Visible = true, Min = min, Sec = sec };
                    this.Frame.Navigate(typeof(ScorePage), score);
                }
                else if(DificultySetting == "easy")
                {
                    //dispatcherTimer.Stop();
                    var score = new Score { Points = ((min * 100) + sec) * 2, Visible = true, Min = min, Sec = sec};
                    this.Frame.Navigate(typeof(ScorePage), score);
                }
                else if (DificultySetting == "medium")
                {
                    //dispatcherTimer.Stop();
                    var score = new Score { Points = ((min * 100) + sec) * 3, Visible = true, Min = min, Sec = sec };
                    this.Frame.Navigate(typeof(ScorePage), score);
                }
                else if (DificultySetting == "hard")
                {
                    //dispatcherTimer.Stop();
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
    class Score
    {
        public int Points { get; set; }
        public int Min { get; set; }
        public int Sec { get; set; }
        public bool Visible { get; set; }

    }
}
