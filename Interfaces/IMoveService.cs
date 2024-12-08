using chess.Model;

namespace chess.Interfaces
{
    public interface IMoveService
    {
        void MakeMove(Board board, int fromColumn, int fromRow, int toColumn, int toRow, 
            string currentTurn, CastlingState castlingState);
        (int fromX, int fromY, int toX, int toY) MakeAiMove(Board board, string currentTurn, 
            CastlingState castlingState, IAiService aiStrategy);
        List<(int x, int y, bool IsEnemy)> GetValidMoves(Board board, int x, int y, string currentTurn,
            CastlingState castlingState, bool isCheck);
    }
}