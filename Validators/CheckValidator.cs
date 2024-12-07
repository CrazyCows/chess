using chess.Model;

namespace chess.Validators;

public class CheckValidator
{

    public bool Check(Board board, string friendlyColor)
    {
        var squares = board.Squares;
        var kingPos = board.GetKingPos(friendlyColor);
        var friendlyPieces = board.GetPiecePositionsByColor(friendlyColor);
        var enemyPieces = board.GetPiecePositionsByColor(friendlyColor == "White" ? "Black" : "White");
        if (kingPos == null)
        {
            return true;
        }

        if ((from ep in enemyPieces let piece = squares[ep.x, ep.y] select 
                piece.GetMoves((ep.x, ep.y), friendlyPieces, enemyPieces)).
            Any(moves => moves.Any(move => move.x == kingPos.Value.x && move.y == kingPos.Value.y)))
        {
            return true;
        }
        return false;
    }

    public bool Checkmate(List<(int x, int y, bool IsEnemy)> validMoves)
    {

        if (validMoves.Count == 0)
        {
            return true;
        }
        return false;
    }
}