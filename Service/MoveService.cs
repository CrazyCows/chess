using chess.Interfaces;
using chess.Model;

namespace chess.Service
{
    public class MoveService : IMoveService
    {
        private readonly IMoveValidator _moveValidator;

        public MoveService(IMoveValidator moveValidator)
        {
            _moveValidator = moveValidator;
        }

        public void MakeMove(Board board, int fromColumn, int fromRow, int toColumn, int toRow, 
            string currentTurn, CastlingState castlingState)
        {
            board.MovePiece(fromColumn, fromRow, toColumn, toRow);
            castlingState.MarkPieceMoved(fromColumn, fromRow);
        }

        

        public (int fromX, int fromY, int toX, int toY) MakeAiMove(Board board, string currentTurn, 
            CastlingState castlingState, IAiService aiStrategy)
        {
            var gameState = new GameState { CurrentTurn = currentTurn };
            var bestMove = aiStrategy.GetBestMove(board, gameState, castlingState, currentTurn);
            board.MovePiece(bestMove.fromX, bestMove.fromY, bestMove.toX, bestMove.toY);
            castlingState.MarkPieceMoved(bestMove.fromX, bestMove.fromY);
            return bestMove;
        }

        public List<(int x, int y, bool IsEnemy)> GetValidMoves(Board board, int x, int y, string currentTurn,
            CastlingState castlingState, bool isCheck)
        {
            return _moveValidator.GetValidMoves(x, y, board, currentTurn, castlingState.MovementStates, isCheck);
        }
    }
}