using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuApp
{
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
}
