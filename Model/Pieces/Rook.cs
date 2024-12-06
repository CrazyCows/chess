namespace chess.Model.Pieces;

public class Rook(string Color) : Piece(Color)
{
    public override string Symbol => Color == "White" ? "♖" : "♜";
    public override double Weight => 0.65;

    public override List<(int x, int y, bool IsEnemy)> GetMoves((int x, int y) currentPosition, List<(int x, int y)> enemyPieces, List<(int x, int y)> friendlyPieces)
    {
        List<(int x, int y, bool IsEnemy)> moveList = new List<(int x, int y, bool IsEnemy)>();
        var directions = new[] { (1, 0), (-1, 0), (0, 1), (0, -1) };
        foreach (var dir in directions)
        {
            for (int i = 1; i <= 8; i++)
            {
                    var scaledDir = (dir.Item1 * i, dir.Item2 * i);
                    var newPos = GetNewPosition(scaledDir, currentPosition);
                    if (!IsMoveInsideBorder(newPos)) continue;

                    if (friendlyPieces.Contains(newPos)){
                        break;
                    } else if (enemyPieces.Contains(newPos)) {
                        moveList.Add((newPos.Item1, newPos.Item2, true));
                        break;
                    }

                    moveList.Add((newPos.Item1, newPos.Item2, false));
                    
                }
            }
            return moveList;
    }
}
