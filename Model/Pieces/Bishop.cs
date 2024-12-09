namespace chess.Model.Pieces;

public class Bishop(string Color) : Piece(Color)
{
    public override string Symbol => Color == "White" ? "♗" : "♝";
    public override double Weight => 0.5;

    public override List<(int x, int y, bool IsEnemy)> GetMoves((int x, int y) currentPosition,
        List<(int x, int y)> enemyPieces, List<(int x, int y)> friendlyPieces)
    {
        var moveList = new List<(int x, int y, bool IsEnemy)>();


        var directions = new[] { (1, 1), (-1, 1), (1, -1), (-1, -1) };

        foreach (var dir in directions)
            for (var i = 1; i <= 8; i++)
            {
                var scaledDir = (dir.Item1 * i, dir.Item2 * i);
                var newPos = GetNewPosition(scaledDir, currentPosition);
                if (!IsMoveInsideBorder(newPos)) continue;

                if (friendlyPieces.Contains(newPos)) break;

                if (enemyPieces.Contains(newPos))
                {
                    moveList.Add((newPos.Item1, newPos.Item2, true));
                    break;
                }

                moveList.Add((newPos.Item1, newPos.Item2, false));
                //
                //var piece = board.GetSquare(newPos);
                //if (piece == null)
                //{
                //    validList.Add((newPos.Item1, newPos.Item2, false));
                //}
                //else
                //{
                //    if (piece.Color != this.Color)
                //    {
                //        validList.Add((newPos.Item1, newPos.Item2, true));
                //    }
                //    break;
                //}
            }

        return moveList;
    }
}