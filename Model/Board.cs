using chess.Model.Pieces;

namespace chess.Model;

public class Board
{
    public Piece?[,] Squares { get; set; } 

    public Board() {
        Squares = new Piece[8,8];
        InitializeBoard();
    }

    public List<(int x, int y)> GetPiecePositionsByColor(string color){
        List<(int x, int y)> positions = new List<(int x, int y)>();

        for (int i = 0; i < Squares.GetLength(0); i++)
        {
            for (int j = 0; j < Squares.GetLength(1); j++) 
            {
                if (Squares[i,j] == null) continue;
                if (Squares[i,j]!.Color == color)
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

    private bool CastlingMove(int toRow, int toColumn, Piece piece) 
    {
        if (Squares[toRow, toColumn] == null)
        {
            return false;
        }
        if (piece.GetType() == typeof(King) && Squares[toRow, toColumn]!.GetType() == typeof(Rook)) {
            if (toColumn == 7) {
                if (piece.Color == "White") {
                    Squares[7, 7] = null;
                    Squares[7, 4] = null;
                    Squares[7, 5] = new Rook("White");
                    Squares[7, 6] = new King("White");
                } 
                else if (piece.Color == "Black")
                {
                    Squares[0, 7] = null;
                    Squares[0, 4] = null;
                    Squares[0, 5] = new Rook("Black");
                    Squares[0, 6] = new King("Black");
                }
            } 
            else if (toColumn == 0)
            {
                if (piece.Color == "White") {
                    Squares[7, 0] = null;
                    Squares[7, 4] = null;
                    Squares[7, 3] = new Rook("White");
                    Squares[7, 2] = new King("White");
                } 
                else if (piece.Color == "Black")
                {
                    Squares[0, 0] = null;
                    Squares[0, 4] = null;
                    Squares[0, 3] = new Rook("Black");
                    Squares[0, 2] = new King("Black");
                }
            }
            Console.WriteLine("TRIGGERED");
            return true;
        }
        return false;
    }

    private void TransformPawnToQueen(Piece piece, int toRow, int toColumn)
    {
        if (piece.GetType() == typeof(Pawn)) 
        {
            if (piece.Color == "White" && toRow == 0) 
            {
                Squares[toRow, toColumn] = new Queen("White");
            } 
            else if (piece.Color == "Black" && toRow == 7)
            {
                Squares[toRow, toColumn] = new Queen("Black");
            }
        }

    }

    public void MovePiece(int fromRow, int fromColumn, int toRow, int toColumn) 
    {
        
        Piece piece = Squares[fromRow, fromColumn]!;
        if (CastlingMove(toRow, toColumn, piece))
        {
            return;
        }

        Squares[fromRow, fromColumn] = null;
        Squares[toRow, toColumn] = piece;
        TransformPawnToQueen(piece, toRow, toColumn);
        
    }

    public (int x, int y)? GetKingPos(string color){
        for (int i = 0; i < Squares.GetLength(0); i++)
        {
            for (int j = 0; j < Squares.GetLength(1); j++) 
            {
                Piece? piece = Squares[i, j];
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
                    if (Squares[i, j]!.GetType() == typeof(King))
                    {
                        copy.Squares[i, j] = new King(Squares[i, j]!.Color);
                    } else if (Squares[i, j]!.GetType() == typeof(Rook))
                    {
                        copy.Squares[i, j] = new Rook(Squares[i, j]!.Color);
                    } else if (Squares[i, j]!.GetType() == typeof(Queen))
                    {
                        copy.Squares[i, j] = new Queen(Squares[i, j]!.Color);
                    }
                    else if (Squares[i, j]!.GetType() == typeof(Pawn))
                    {
                        copy.Squares[i, j] = new Pawn(Squares[i, j]!.Color);
                    }
                    else if (Squares[i, j]!.GetType() == typeof(Bishop))
                    {
                        copy.Squares[i, j] = new Bishop(Squares[i, j]!.Color);
                    }
                    else if (Squares[i, j]!.GetType() == typeof(Knight))
                    {
                        copy.Squares[i, j] = new Knight(Squares[i, j]!.Color);
                    }
                }
            }
        }
        return copy;
    }
    
}