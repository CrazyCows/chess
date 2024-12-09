using chess.Model;

namespace chess.Interfaces;

public interface IMoveService
{
    void MakeMove(int fromColumn, int fromRow, int toColumn, int toRow);

    (int fromX, int fromY, int toX, int toY) MakeAiMove(string currentTurn);

    List<(int x, int y, bool IsEnemy)> GetValidMoves(int x, int y, string currentTurn, bool isCheck);
}