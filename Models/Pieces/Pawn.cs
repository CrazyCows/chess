namespace chess.Models.Pieces;

public class Pawn : Piece
{
    public override bool IsMoveValid((int x, int y) targetPosition)
    {
        return true;
    }
}