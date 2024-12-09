namespace chess.Interfaces;

public interface IPieces
{
    string Color { get; set; }
    string Symbol { get; }
    double Weight { get; }

    List<(int x, int y, bool IsEnemy)> GetMoves((int x, int y) currentPosition,
        List<(int x, int y)> enemyPieces,
        List<(int x, int y)> friendlyPieces);

    bool IsMoveInsideBorder((int x, int y) currentPosition);
    (int x, int y) GetNewPosition((int x, int y) posToAdd, (int x, int y) currentPosition);
}