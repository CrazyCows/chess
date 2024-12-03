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

    private void InitializeBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            Squares[1, i] = new Pawn("Black", 1, i);
            Squares[6, i] = new Pawn("White", 6, i);
        }

        Squares[0, 0] = new Rook("Black", 0, 0);
        Squares[0, 7] = new Rook("Black", 0, 7);
        Squares[7, 0] = new Rook("White", 7, 0);
        Squares[7, 7] = new Rook("White", 7, 7);

        Squares[0, 1] = new Knight("Black", 0, 1);
        Squares[0, 6] = new Knight("Black", 0, 6);
        Squares[7, 1] = new Knight("White", 7, 1);
        Squares[7, 6] = new Knight("White", 7, 6);

        Squares[0, 2] = new Bishop("Black", 0, 2);
        Squares[0, 5] = new Bishop("Black", 0, 5);
        Squares[7, 2] = new Bishop("White", 7, 2);
        Squares[7, 5] = new Bishop("White", 7, 5);

        Squares[0, 3] = new Queen("Black", 0, 3);
        Squares[7, 3] = new Queen("White", 7, 3);

        Squares[0, 4] = new King("Black", 0, 4);
        Squares[7, 4] = new King("White", 7, 4);
    }
    
    public List<(int x, int y, bool IsEnemy)>? GetMoves(int x, int y, string turn)
    {
        if (Squares[x,y].Color == turn)
        {
            return Squares[x, y].GetValidMoves(this);
        }
        return null;
    }

    public void MovePiece(int fromRow, int fromColumn, int toRow, int toColumn) 
    {
        
        Piece piece = Squares[fromRow, fromColumn];
        piece.Move((toRow, toColumn), this);

        Squares[fromRow, fromColumn] = null;
        Squares[toRow, toColumn] = piece;
        
    }

    

    public List<(int x, int y, bool IsEnemy)>? Check(string Color)
    {
        (int x, int y, bool IsEnemy) kingPos = (-1, -1, false);
        List<(int x, int y, bool IsEnemy)> CheckPos = new List<(int x, int y, bool IsEnemy)>();

        // Locate the king
        for (int i = 0; i < Squares.GetLength(0); i++)
        {
            for (int j = 0; j < Squares.GetLength(1); j++) 
            {
                Piece piece = Squares[i, j];
                if (piece != null && piece.GetType() == typeof(King) && piece.Color == Color)
                {
                    kingPos = (piece.Position.x, piece.Position.y, false);
                }
            }
        }

        // Check if any piece threatens the king
        for (int i = 0; i < Squares.GetLength(0); i++)
        {
            for (int j = 0; j < Squares.GetLength(1); j++) 
            {
                Piece piece = Squares[i, j];
                if (piece != null && piece.Color != Color)
                {
                    var moves = piece.GetValidMoves(this);
                    if (moves.Any(move => move.x == kingPos.x && move.y == kingPos.y))
                    {
                        CheckPos.Add((i, j, true)); // Threatening piece's position
                    }
                }
            }
        }
        if (CheckPos.Count > 0) {
            Console.WriteLine("CHECK");

        }
        return CheckPos.Count > 0 ? CheckPos : null;
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
                    copy.Squares[i, j] = Squares[i, j]; // Ensure Piece class has a Clone method
                }
            }
        }
        return copy;
    }


    public bool CheckMate(string kingColor)
    {
        // Check if the king is in check
        var checkPositions = Check(kingColor);
        if (checkPositions == null || checkPositions.Count == 0)
        {
            // The king is not in check, so it's not a checkmate
            return false;
        }

        // Iterate through all pieces of the king's color
        for (int i = 0; i < Squares.GetLength(0); i++)
        {
            for (int j = 0; j < Squares.GetLength(1); j++)
            {
                Piece piece = Squares[i, j];
                if (piece != null && piece.Color == kingColor)
                {
                    // Get all valid moves for this piece
                    var validMoves = piece.GetValidMoves(this);
                    foreach (var move in validMoves)
                    {
                        // Simulate the move
                        Board simulatedBoard = DeepCopy();
                        simulatedBoard.MovePiece(piece.Position.x, piece.Position.y, move.x, move.y);

                        // Check if the king is still in check
                        if (simulatedBoard.Check(kingColor) == null)
                        {
                            // Found a move that resolves the check
                            return false;
                        }
                    }
                }
            }
        }
        Console.WriteLine("WON");
        // No valid moves found to resolve the check
        return true;
    }

}