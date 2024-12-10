using chess.Interfaces;

namespace chess.Model.Pieces;

public class King(string Color) : Piece(Color), ICopyableT<Piece>
{
    public override string Symbol => Color == "White" ? "♔" : "♚";

    public override double Weight => 1;

    public override List<(int x, int y, bool IsEnemy)> GetMoves((int x, int y) currentPosition,
        List<(int x, int y)> enemyPieces, List<(int x, int y)> friendlyPieces)
    {
        var moveList = new List<(int x, int y, bool IsEnemy)>();

        var moves = new[]
        {
            (1, 0), (-1, 0), (0, 1), (0, -1),
            (1, 1), (-1, 1), (1, -1), (-1, -1)
        };

        foreach (var move in moves)
        {
            var newPos = GetNewPosition(move, currentPosition);
            if (!IsMoveInsideBorder(newPos)) continue;
            if (friendlyPieces.Contains(newPos)) continue;
            if (enemyPieces.Contains(newPos))
                moveList.Add((newPos.Item1, newPos.Item2, true));
            else
                moveList.Add((newPos.Item1, newPos.Item2, false));
            // var piece = board.GetSquare(newPos);
            // if (piece == null)
            // {
            //     validList.Add((newPos.Item1, newPos.Item2, false));
            // }
            // else if (piece.Color != this.Color)
            // {
            //     validList.Add((newPos.Item1, newPos.Item2, true));
            // }
        }

        return moveList;
    }
    
    public Piece Copy()
    {
        return new King(Color);
    }
}