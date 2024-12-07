using System.Runtime.CompilerServices;

namespace chess.Model.Pieces;

public class Pawn(string Color) : Piece(Color)
{
    public override string Symbol => Color == "White" ? "♙" : "♟";
    public override double Weight => 0.2;

    public override List<(int x, int y, bool IsEnemy)> GetMoves((int x, int y) currentPosition, List<(int x, int y)> enemyPieces, List<(int x, int y)> friendlyPieces)
    {
        List<(int x, int y, bool IsEnemy)> moveList = new List<(int x, int y, bool IsEnemy)>();

        var forward = Color == "White" ? (-1, 0) : (1, 0);
        var forward2 = Color == "White" ? (-2, 0) : (2, 0);
        var captureLeft = Color == "White" ? (-1, 1) : (1, -1);
        var captureRight = Color == "White" ? (-1, -1) : (1, 1);
        int startPos = Color == "White" ? 6 : 1;

        var forwardPos = GetNewPosition(forward, currentPosition);
        var forwardPos2 = GetNewPosition(forward2, currentPosition);
        var leftPos = GetNewPosition(captureLeft, currentPosition);
        var rightPos = GetNewPosition(captureRight, currentPosition);
        

        if (IsMoveInsideBorder(forwardPos) && !enemyPieces.Contains(forwardPos) && !friendlyPieces.Contains(forwardPos)) 
        {
            moveList.Add((forwardPos.Item1, forwardPos.Item2, false));
            if (currentPosition.x == startPos && !enemyPieces.Contains(forwardPos2) && !friendlyPieces.Contains(forwardPos2)) {
                moveList.Add((forwardPos2.Item1, forwardPos2.Item2, false));
            }
        }
        
        if (IsMoveInsideBorder(leftPos) && enemyPieces.Contains(leftPos)) {
            moveList.Add((leftPos.Item1, leftPos.Item2, true));
        }
        if (IsMoveInsideBorder(rightPos) && enemyPieces.Contains(rightPos)) {
            moveList.Add((rightPos.Item1, rightPos.Item2, true));
        }

        return moveList;
    }
}
