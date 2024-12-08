using chess.Model;

namespace chess.Interfaces;

public interface IMoveValidator
{
    List<(int x, int y, bool IsEnemy)> GetValidMoves(int x, int y, Board board, string friendlyColor,
        Dictionary<PlayerColor, Dictionary<PieceType, bool>> movementStates, bool isCheck);
    List<(int x, int y, bool IsEnemy)> GetAllValidMoves(Board board, string friendlyColor,
        Dictionary<PlayerColor, Dictionary<PieceType, bool>> movementStates, bool isCheck);
}