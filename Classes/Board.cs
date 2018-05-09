using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuApp
{
    public class Board
    {
        public Cell[] CellArray { get; set; }//Creates array for cells
        Random rand = new Random();//Creates a seeded random number
        public Board(bool fillBoard)
        {
            CellArray = new Cell[81];//Establishes length of cell Array
            for (int i = 0; i < CellArray.Length; i++)//Initializes all Cell's
            {
                CellArray[i] = new Cell();
            }
            PopulateCellArray();
            if(fillBoard)
                PopulateBoard();
        }
        public Board()
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
}
