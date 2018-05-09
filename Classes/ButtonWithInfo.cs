using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace SudokuApp
{
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
}
