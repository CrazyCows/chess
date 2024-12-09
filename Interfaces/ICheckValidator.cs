using chess.Model;

namespace chess.Interfaces;

public interface ICheckValidator
{
    bool Check(IBoard board, string friendlyColor);
    bool Checkmate(List<(int x, int y, bool IsEnemy)> validMoves);
}