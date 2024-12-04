using System.Drawing;
using chess.Models.Pieces;

namespace chess.Models;

public class Board
{
    public bool IsCheck { get; set; }
    public bool IsCheckMate { get; set; }
    public Piece[,] Squares { get; set; } 

    public Board() {
        Squares = new Piece[8,8];
        InitializeBoard();
    }

    public Piece GetSquare((int x, int y) squarePosition){
        return Squares[squarePosition.x, squarePosition.y];
    }

    public List<(int x, int y)> GetPiecePositionsByColor(string color){
        List<(int x, int y)> positions = new List<(int x, int y)>();

        for (int i = 0; i < Squares.GetLength(0); i++)
        {
            for (int j = 0; j < Squares.GetLength(1); j++) 
            {
                if (Squares[i,j] == null) continue;
                if (Squares[i,j].Color == color)
                {
                    positions.Add((i,j));
                }
            }
        }
        return positions;
    }

    private void InitializeBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            Squares[1, i] = new Pawn("Black");
            Squares[6, i] = new Pawn("White");
        }

        Squares[0, 0] = new Rook("Black");
        Squares[0, 7] = new Rook("Black");
        Squares[7, 0] = new Rook("White");
        Squares[7, 7] = new Rook("White");

        Squares[0, 1] = new Knight("Black");
        Squares[0, 6] = new Knight("Black");
        Squares[7, 1] = new Knight("White");
        Squares[7, 6] = new Knight("White");

        Squares[0, 2] = new Bishop("Black");
        Squares[0, 5] = new Bishop("Black");
        Squares[7, 2] = new Bishop("White");
        Squares[7, 5] = new Bishop("White");

        Squares[0, 3] = new Queen("Black");
        Squares[7, 3] = new Queen("White");

        Squares[0, 4] = new King("Black");
        Squares[7, 4] = new King("White");
    }

    public void MovePiece(int fromRow, int fromColumn, int toRow, int toColumn) 
    {
        Piece piece = Squares[fromRow, fromColumn];
        Squares[fromRow, fromColumn] = null;
        Squares[toRow, toColumn] = piece;
    }

    public (int x, int y)? GetKingPos(string color){
        for (int i = 0; i < Squares.GetLength(0); i++)
        {
            for (int j = 0; j < Squares.GetLength(1); j++) 
            {
                Piece piece = Squares[i, j];
                if (piece != null && piece.GetType() == typeof(King) && piece.Color == color)
                {
                    return (i,j);
                }
            }
        }
        return null;
        
    }

    public Board DeepCopy()
    {
        Board copy = new Board();
        for (int i = 0; i < Squares.GetLength(0); i++)
        {
            for (int j = 0; j < Squares.GetLength(1); j++)
            {
                if (Squares[i, j] != null)
                {
                    copy.Squares[i, j] = Squares[i, j];
                }
            }
        }
        return copy;
    }
    
}