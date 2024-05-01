// Michael Banko - CPS3330 SP24 Project
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4.Models
{
    // Represents the Connect4 game grid
    public class Grid
    {
        // Constants for the number of columns and rows in the grid
        const int COLS = 7;
        const int ROWS = 6;

        // 2D array representing the game pieces on the grid
        public PieceEnum?[,] Pieces { get; protected set; }

        // Indicates whose turn it is (Orange or Green)
        public PieceEnum NextTurn { get; protected set; }

        // Represents the winner of the game (Orange or Green)
        public PieceEnum? Winner { get; protected set; }

        // Score counters for Orange and Green players
        public int Orange { get; set; }
        public int Green { get; set; }

        // Indicates whether the game is a draw
        public bool Draw { get; set; }

        // Constructor to initialize the game grid
        public Grid()
        {
            // Set the initial turn to Orange player
            NextTurn = PieceEnum.Orange;
            // Reset the game
            ResetGame();
        }

        // Resets the game grid and state
        public void ResetGame()
        {
            // Initialize the 2D array for game pieces
            Pieces = new PieceEnum?[COLS, ROWS];
            // Reset draw status
            Draw = false;

            // Switch turns between Orange and Green if no winner yet
            if (!Winner.HasValue)
            {
                NextTurn = (NextTurn == PieceEnum.Orange ? PieceEnum.Green : PieceEnum.Orange);
            }
            // If there is a winner, the next turn belongs to the winner
            else
            {
                NextTurn = Winner.Value;
            }

            // Reset the winner status
            Winner = null;
        }

        // Sets the next turn and updates the winner and scores
        public void SetNextTurn()
        {
            // Determine the winner
            Winner = GetWinner();

            // If there's no winner, switch turns
            if (!Winner.HasValue)
            {
                // Switch to the other player's turn
                if (NextTurn == PieceEnum.Orange)
                {
                    NextTurn = PieceEnum.Green;
                }
                else
                {
                    NextTurn = PieceEnum.Orange;
                }
            }
            // If there's a winner, update their score
            else
            {
                switch (Winner.Value)
                {
                    case PieceEnum.Orange:
                        Orange += 1;
                        break;
                    case PieceEnum.Green:
                        Green += 1;
                        break;
                }
            }
        }

        // Helper class to represent grid indices for checks
        public class CheckIndex
        {
            public int Column { get; }
            public int Row { get; }

            // Constructor to initialize indices
            public CheckIndex(int column, int row)
            {
                Column = column;
                Row = row;
            }
        }

        // Determines the winner by checking all possible winning conditions
        public PieceEnum? GetWinner()
        {
            // Variable to hold the winner
            PieceEnum? winner = null;

            // Iterate through all columns and rows
            for (var column = 0; column <= Pieces.GetUpperBound(0); column++)
            {
                for (var row = 0; row <= Pieces.GetUpperBound(1); row++)
                {
                    // Check horizontally, vertically, and diagonally
                    winner = CheckGroup(column, row, (column, row, checkIndex) => new CheckIndex(column + checkIndex, row));

                    // Return winner if found
                    if (winner.HasValue)
                    {
                        return winner;
                    }

                    // Check vertically
                    winner = CheckGroup(column, row, (column, row, checkIndex) => new CheckIndex(column, row + checkIndex));

                    // Return winner if found
                    if (winner.HasValue)
                    {
                        return winner;
                    }

                    // Check diagonals (both directions)
                    winner = CheckGroup(column, row, (column, row, checkIndex) => new CheckIndex(column + checkIndex, row + checkIndex));

                    // Return winner if found
                    if (winner.HasValue)
                    {
                        return winner;
                    }

                    winner = CheckGroup(column, row, (column, row, checkIndex) => new CheckIndex(column - checkIndex, row + checkIndex));

                    // Return winner if found
                    if (winner.HasValue)
                    {
                        return winner;
                    }
                }
            }

            // Return null if no winner is found
            return null;
        }

        // Checks a group of pieces in the grid to see if they match
        private PieceEnum? CheckGroup(int column, int row, Func<int, int, int, CheckIndex> check)
        {
            // Variable to hold the last checked piece
            PieceEnum? lastCheck = null;

            // Check each index in the group
            for (var checkIndex = 0; checkIndex <= 3; checkIndex++)
            {
                // Get the current index for checking
                var checkRowColIndex = check?.Invoke(column, row, checkIndex);

                // Return null if the index is invalid
                if (checkRowColIndex == null)
                {
                    return null;
                }

                // Check if the index is out of bounds
                if (checkRowColIndex.Column < Pieces.GetLowerBound(0) || checkRowColIndex.Column > Pieces.GetUpperBound(0) ||
                    checkRowColIndex.Row < Pieces.GetLowerBound(1) || checkRowColIndex.Row > Pieces.GetUpperBound(1))
                {
                    return null;
                }

                // Get the piece at the current index
                var thisCheck = Pieces[checkRowColIndex.Column, checkRowColIndex.Row];

                // If the piece is null or doesn't match the last check, return null
                if (thisCheck == null || (checkIndex > 0 && lastCheck != thisCheck))
                {
                    return null;
                }

                // Update the last checked piece
                lastCheck = thisCheck;
            }

            // If all pieces in the group match, return the matching piece
            return lastCheck;
        }
    }
}