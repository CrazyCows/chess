using System.Drawing;
using System.Runtime.InteropServices.Swift;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace chess.Models;

public abstract class Piece(string Color)
{
    public string Color { get; set; } = Color;
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

    public (int, int) GetNewPosition((int x, int y) posToAdd, (int x, int y) Position)
    {
        return (Position.x + posToAdd.x, Position.y + posToAdd.y);
    }
}
    