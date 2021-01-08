using UnityEngine;
using System.Collections;

//<summary>
//Pure recursive maze generation.
//Use carefully for large mazes.
//</summary>
public class RecursiveMazeGenerator : BasicMazeGenerator {

	public RecursiveMazeGenerator(int rows, int columns):base(rows,columns){

	}

	public override void GenerateMaze ()
	{
		VisitCell (0, 0, MAzeDirection.Start);
	}

	private void VisitCell(int row, int column, MAzeDirection moveMade){
        MAzeDirection[] movesAvailable = new MAzeDirection[4];
		int movesAvailableCount = 0;

		do{
			movesAvailableCount = 0;

			//check move right
			if(column+1 < ColumnCount && !GetMazeCell(row,column+1).IsVisited){
				movesAvailable[movesAvailableCount] = MAzeDirection.Right;
				movesAvailableCount++;
			}else if(!GetMazeCell(row,column).IsVisited && moveMade != MAzeDirection.Left){
				GetMazeCell(row,column).WallRight = true;
			}
			//check move forward
			if(row+1 < RowCount && !GetMazeCell(row+1,column).IsVisited){
				movesAvailable[movesAvailableCount] = MAzeDirection.Front;
				movesAvailableCount++;
			}else if(!GetMazeCell(row,column).IsVisited && moveMade != MAzeDirection.Back){
				GetMazeCell(row,column).WallFront = true;
			}
			//check move left
			if(column > 0 && column-1 >= 0 && !GetMazeCell(row,column-1).IsVisited){
				movesAvailable[movesAvailableCount] = MAzeDirection.Left;
				movesAvailableCount++;
			}else if(!GetMazeCell(row,column).IsVisited && moveMade != MAzeDirection.Right){
				GetMazeCell(row,column).WallLeft = true;
			}
			//check move backward
			if(row > 0 && row-1 >= 0 && !GetMazeCell(row-1,column).IsVisited){
				movesAvailable[movesAvailableCount] = MAzeDirection.Back;
				movesAvailableCount++;
			}else if(!GetMazeCell(row,column).IsVisited && moveMade != MAzeDirection.Front){
				GetMazeCell(row,column).WallBack = true;
			}

			if(movesAvailableCount == 0 && !GetMazeCell(row,column).IsVisited){
				GetMazeCell(row,column).IsGoal = true;
			}

			GetMazeCell(row,column).IsVisited = true;

			if(movesAvailableCount > 0){
				switch (movesAvailable[Random.Range(0,movesAvailableCount)]) {
				case MAzeDirection.Start:
					break;
				case MAzeDirection.Right:
					VisitCell(row,column+1, MAzeDirection.Right);
					break;
				case MAzeDirection.Front:
					VisitCell(row+1,column, MAzeDirection.Front);
					break;
				case MAzeDirection.Left:
					VisitCell(row,column-1, MAzeDirection.Left);
					break;
				case MAzeDirection.Back:
					VisitCell(row-1,column, MAzeDirection.Back);
					break;
				}
			}

		}while(movesAvailableCount > 0);
	}
}
