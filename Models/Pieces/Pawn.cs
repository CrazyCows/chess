using System.Runtime.CompilerServices;

namespace chess.Models.Pieces;

public class Pawn(string Color, int x, int y) : Piece(Color, x, y)
{
    public override string Symbol => Color == "White" ? "♙" : "♟";
    public override List<(int x, int y, bool IsEnemy)> GetValidMoves(Board board)
    {
        List<(int x, int y, bool IsEnemy)> validList = new List<(int x, int y, bool IsEnemy)>();

        var forward = Color == "White" ? (-1, 0) : (1, 0);
        var forward2 = Color == "White" ? (-2, 0) : (2, 0);
        var captureLeft = Color == "White" ? (-1, 1) : (1, -1);
        var captureRight = Color == "White" ? (-1, -1) : (1, 1);
        int startPos = Color == "White" ? 6 : 1;

        var forwardPos = GetNewPosition(forward);
        var forwardPos2 = GetNewPosition(forward2);
        if (IsMoveInsideBorder(forward) && board.GetSquare(forwardPos) == null)
        {
            
            validList.Add((forwardPos.Item1, forwardPos.Item2, false));
        }
        if (Position.x == startPos && board.GetSquare(forwardPos2) == null) {
            validList.Add((forwardPos2.Item1, forwardPos2.Item2, false));
        }

        var leftPos = GetNewPosition(captureLeft);
        if (IsMoveInsideBorder(captureLeft) && board.GetSquare(leftPos)?.Color != null &&
            board.GetSquare(leftPos)?.Color != this.Color)
        {
            validList.Add((leftPos.Item1, leftPos.Item2, true));
        }

        var rightPos = GetNewPosition(captureRight);
        if (IsMoveInsideBorder(captureRight) && board.GetSquare(rightPos)?.Color != null &&
            board.GetSquare(rightPos)?.Color != this.Color)
        {
            validList.Add((rightPos.Item1, rightPos.Item2, true));
        }

        return validList;
    }
}
