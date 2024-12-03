namespace chess.Models.Pieces;

public class Queen(string Color, int x, int y) : Piece(Color, x, y)
{
    public override string Symbol => Color == "White" ? "♕" : "♛";

    public override List<(int x, int y, bool IsEnemy)> GetValidMoves(Board board)
    {
        List<(int x, int y, bool IsEnemy)> validList = new List<(int x, int y, bool IsEnemy)>();

        var directions = new[]
            {
                (1, 0), (-1, 0), (0, 1), (0, -1),
                (1, 1), (-1, 1), (1, -1), (-1, -1)
            };

        foreach (var dir in directions)
            {
            for (int i = 1; i <= 8; i++)
                {
                    var scaledDir = (dir.Item1 * i, dir.Item2 * i);
                    var newPos = GetNewPosition(scaledDir);
                    if (!IsMoveInsideBorder(scaledDir)) continue;

                    var piece = board.GetSquare(newPos);
                    if (piece == null)
                    {
                        validList.Add((newPos.Item1, newPos.Item2, false));
                    }
                    else
                    {
                        if (piece.Color != this.Color)
                        {
                            validList.Add((newPos.Item1, newPos.Item2, true));
                        }
                        break;
                    }
                }
            }

        return validList;
    }
}
