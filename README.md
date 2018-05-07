# sudoku-uwp
A three person group project created in C#.
The application was required to use randomly generated boards and was to be a professional looking sudoku game.
The application passed certifaction for the windows store, but was not published.<br/>
I worked primarily on the grid.xaml.cs file<br/><br/>
Currently all of the code in that file was done by me with the EXCEPTION of<br/>

* CompletedBoard class, which was used to check if the board was complete and was based on my Board class<br/>
* Conflict, RowCheck2, ColumnCheck2, and SquareCheck2 methods which were copy pasted from the Board class with minor format changes<br/>
* button_Click method<br/>
* Anything timer related<br/><br/>

I intend to go back and heavily modify the grid.xaml.cs file to reduce copy paste code and make the project as a whole cleaner and more readable.
