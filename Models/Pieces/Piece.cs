using System.Drawing;
using System.Runtime.InteropServices.Swift;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace chess.Models;

public abstract class Piece(string Color, int x, int y)
{
    public string Color { get; set; } = Color;
    public (int x, int y) Position { get; set; } = (x, y);
    public abstract List<(int x, int y, bool IsEnemy)> GetValidMoves(Board board);
    public virtual string Symbol => " ";


    public bool IsMoveValid((int x, int y) targetPosition, Board board)
    {
        return GetValidMoves(board)
            .Any(move => move.x == targetPosition.x && move.y == targetPosition.y);
    }

    public void Move((int x, int y) targetPosition, Board board)
    {
        
        if (IsMoveValid(targetPosition, board))
        {
            Position = targetPosition;
        }
        else
        {
            throw new InvalidOperationException("Invalid target position");
        }
    }

    public bool IsMoveInsideBorder((int x, int y) move) 
    {
        if (Position.x + move.x > 7 || Position.x + move.x < 0) 
        {
            return false;
        }
        if (Position.y + move.y > 7 || Position.y + move.y < 0)
        {
            return false;
        }
        return true;
        
    }

    public (int, int) GetNewPosition((int x, int y) posToAdd)
    {
        return (Position.x + posToAdd.x, Position.y + posToAdd.y);
    }
}
    