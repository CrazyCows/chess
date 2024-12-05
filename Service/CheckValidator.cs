using chess.Models;

public class CheckValidator
{
    public event Action<bool>? IsCheck;
    public event Action<bool>? IsCheckMate;
    public event Action<string>? GameOverDueToTime;

    public bool Check(Board Board, string friendlyColor)
    {
        var Squares = Board.Squares;
        var kingPos = Board.GetKingPos(friendlyColor);
        var friendlyPieces = Board.GetPiecePositionsByColor(friendlyColor);
        var enemyPieces = Board.GetPiecePositionsByColor(friendlyColor == "White" ? "Black" : "White");
        if (kingPos == null)
        {
            IsCheck?.Invoke(true);
            return true;
        }

        foreach (var ep in enemyPieces) {
            Piece piece = Squares[ep.x, ep.y];
            var moves = piece.GetMoves((ep.x, ep.y), friendlyPieces, enemyPieces);
            if (moves.Any(move => move.x == kingPos.Value.x && move.y == kingPos.Value.y))
            {
                IsCheck?.Invoke(true);
                return true;
            }
        }
        IsCheck?.Invoke(false);
        return false;
    }

    public void Checkmate(List<(int x, int y, bool IsEnemy)> validMoves)
    {

        if (validMoves.Count == 0)
        {
            IsCheckMate?.Invoke(true);
            return;
        }
        IsCheckMate?.Invoke(false);
        return;
    }
}