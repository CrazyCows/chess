using chess.Model;
using chess.Model.Pieces;

namespace chess.Validators;

public class MoveValidator
{
    public bool ValidateMove((int x, int y) from, (int x, int y) to, Board board)
    {
        Piece fromPiece = board.Squares[from.x, from.y]!;
        Piece toPiece = board.Squares[to.x, to.y]!;
        board.Squares[to.x, to.y] = fromPiece;
        board.Squares[from.x, from.y] = null;

        var squares = board.Squares;
        var kingPos = board.GetKingPos(fromPiece.Color);
        if (kingPos == null)
        {
            return true;
        }

        var friendlyPieces = board.GetPiecePositionsByColor(fromPiece.Color);
        var enemyPieces = board.GetPiecePositionsByColor(fromPiece.Color == "White" ? "Black" : "White");
        board.Squares[to.x, to.y] = toPiece;
        board.Squares[from.x, from.y] = fromPiece;


        foreach (var ep in enemyPieces) {
            Piece piece = squares[ep.x, ep.y]!;
            var moves = piece.GetMoves((ep.x, ep.y), friendlyPieces, enemyPieces);
            if (moves.Any(move => move.x == kingPos.Value.x && move.y == kingPos.Value.y))
            {
                return true;
            }
        }
        return false;
    }

    


    public List<(int x, int y, bool IsEnemy)> GetCastlingMoves(int x, int y, Board board, 
        string friendlyColor, bool whiteKingMoved, bool blackKingMoved,
        bool whiteLeftRookMoved, bool whiteRightRookMoved,
        bool blackLeftRookMoved, bool blackRightRookMoved)
    {
        

        List<(int x, int y, bool IsEnemy)> moves = new List<(int x, int y, bool IsEnemy)>();
        if (!(board.Squares[x,y]?.GetType() == typeof(King)))
        {
            return moves;
        }
        
        if (!whiteKingMoved){
            if (board.Squares[7,0] != null && board.Squares[7,0]!.Color == "White" 
                                           && friendlyColor == "White" && !whiteLeftRookMoved 
                                           && board.Squares[7,1] == null && board.Squares[7,2] == null 
                                           && board.Squares[7,3] == null) 
            {
                board.Squares[7,0] = null;
                board.Squares[7,3] = new Rook("White");
                
                if (!ValidateMove((7,4),(7,2), board))
                {
                    moves.Add((7, 0, false));
                }
                board.Squares[7,0] = new Rook("White");
                board.Squares[7,3] = null;
            }
            if (board.Squares[7,7] != null && board.Squares[7,7]!.Color == "White" 
                                           && friendlyColor == "White" && !whiteRightRookMoved
                                           && board.Squares[7,5] == null && board.Squares[7,6] == null)  
            {
                board.Squares[7,7] = null;
                board.Squares[7,5] = new Rook("White");

                if (!ValidateMove((7,4), (7,6), board))
                {
                    moves.Add((7, 7, false));
                }
                board.Squares[7,7] = new Rook("White");
                board.Squares[7,5] = null;
            }
        }
        
        if (!blackKingMoved)
        {
            if (board.Squares[0,0] != null && board.Squares[0,0]!.Color == "Black" 
                                           && friendlyColor == "Black" && !blackLeftRookMoved
                    && board.Squares[0,1] == null && board.Squares[0,2] == null && board.Squares[0,3] == null) 
            {
                board.Squares[0,0] = null;
                board.Squares[0,3] = new Rook("Black");

                if (!ValidateMove((0,4),(0,2), board))
                {
                    moves.Add((0, 0, false));
                }
                board.Squares[0,0] = new Rook("Black");
                board.Squares[0,3] = null;
            }
            if (board.Squares[0,7] != null && board.Squares[0,7]!.Color == "Black" 
                                           && friendlyColor == "Black" && !blackRightRookMoved
                    && board.Squares[0,5] == null && board.Squares[0,6] == null) 
            {

                board.Squares[0,7] = null;
                board.Squares[0,5] = new Rook("Black");
                
                if (!ValidateMove((0,4), (0,6), board))
                {
                    moves.Add((0, 7, false));
                }

                board.Squares[0,7] = new Rook("Black");
                board.Squares[0,5] = null;
            }
        }
        return moves;
    }

    public List<(int x, int y, bool IsEnemy)> GetValidMoves(int x, int y, Board board, string friendlyColor)
    {
        var friendlyPieces = board.GetPiecePositionsByColor(friendlyColor);
        var enemyPieces = board.GetPiecePositionsByColor(friendlyColor == "White" ? "Black" : "White");
        Piece piece = board.Squares[x, y]!;

        List<(int x, int y, bool IsEnemy)> moves = piece.GetMoves((x, y), enemyPieces, friendlyPieces);
        var finalMoves = new List<(int x, int y, bool IsEnemy)>();

        foreach (var move in moves)
        {
            if (!ValidateMove((x, y), (move.x, move.y), board))
            {
                finalMoves.Add(move);
                
            } 
        }
        return finalMoves;
    }
}