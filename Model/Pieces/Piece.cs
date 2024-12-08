
using chess.Interfaces;

namespace chess.Model.Pieces;

public abstract class Piece(string color) : IPieces
{
    public string Color { get; set; } = color;

    // public (int x, int y) Position { get; set; } = (x, y);
    public abstract List<(int x, int y, bool IsEnemy)> GetMoves((int x, int y) currentPosition, List<(int x, int y)> enemyPieces, List<(int x, int y)> friendlyPieces);
    public virtual string Symbol => " ";
    public virtual double Weight => 0;


    public bool IsMoveInsideBorder((int x, int y) currentPosition) 
    {
        if (currentPosition.Item1 > 7 || currentPosition.Item1 < 0) 
        {
            return false;
        }
        if (currentPosition.Item2 > 7 || currentPosition.Item2 < 0)
        {
            return false;
        }
        return true;
    }

    public (int x, int y) GetNewPosition((int x, int y) posToAdd, (int x, int y) position)
    {
        return (position.x + posToAdd.x, position.y + posToAdd.y);
    }
}
    