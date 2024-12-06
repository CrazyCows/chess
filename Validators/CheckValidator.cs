using chess.Model;

namespace chess.Validators;

public class CheckValidator
{
    public event Action<bool>? IsCheck;
    public event Action<bool>? IsCheckMate;

    public bool Check(Board board, string friendlyColor)
    {
        var squares = board.Squares;
        var kingPos = board.GetKingPos(friendlyColor);
        var friendlyPieces = board.GetPiecePositionsByColor(friendlyColor);
        var enemyPieces = board.GetPiecePositionsByColor(friendlyColor == "White" ? "Black" : "White");
        if (kingPos == null)
        {
            IsCheck?.Invoke(true);
            return true;
        }

        if ((from ep in enemyPieces let piece = squares[ep.x, ep.y] select 
                piece.GetMoves((ep.x, ep.y), friendlyPieces, enemyPieces)).
            Any(moves => moves.Any(move => move.x == kingPos.Value.x && move.y == kingPos.Value.y)))
        {
            IsCheck?.Invoke(true);
            return true;
        }
        IsCheck?.Invoke(false);
        return false;
    }

    public bool Checkmate(List<(int x, int y, bool IsEnemy)> validMoves)
    {

        if (validMoves.Count == 0)
        {
            IsCheckMate?.Invoke(true);
            return true;
        }
        IsCheckMate?.Invoke(false);
        return false;
    }
}