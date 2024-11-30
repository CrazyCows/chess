namespace chess.Models;

public abstract class Piece
{
    public string Color { get; set; }
    public (int x, int y) Position { get; set; }
    public abstract bool IsMoveValid((int x, int y) targetPosition);

    public void Move((int x, int y) targetPosition)
    {
        if (IsMoveValid(targetPosition))
        {
            Position = targetPosition;
            
        }
        else
        {
            throw new InvalidOperationException("Invalid target position");
        }
    }
    
}