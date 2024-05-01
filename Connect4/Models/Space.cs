using System;

namespace Connect4.Models
{
    // Represents a single space in the Connect4 grid
    public class Space
    {
        // The piece (Orange, Green, or null) in the space
        public PieceEnum? Piece { get; protected set; }

        // Sets the piece in the space to the specified value
        public void SetPiece(PieceEnum piece)
        {
            Piece = piece;
        }
    }
}
