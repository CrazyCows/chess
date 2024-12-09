using chess.Interfaces;
using chess.Model;

namespace chess.Service;

public class MoveService : IMoveService
{
    private readonly IMoveValidator _moveValidator;
    private readonly IBoard _board;
    private readonly ICastlingService _castlingService;
    private readonly IAiService _aiService;

    public MoveService(IMoveValidator moveValidator, IBoard board, ICastlingService castlingService, IAiService aiService)
    {
        _moveValidator = moveValidator;
        _board = board;
        _castlingService = castlingService;
        _aiService = aiService;
    }

    public void MakeMove(int fromColumn, int fromRow, int toColumn, int toRow)
    {
        _board.MovePiece(fromColumn, fromRow, toColumn, toRow);
        _castlingService.MarkPieceMoved(fromColumn, fromRow);
    }


    public (int fromX, int fromY, int toX, int toY) MakeAiMove(string currentTurn)
    {
        var bestMove = _aiService.GetBestMove(currentTurn);
        _board.MovePiece(bestMove.fromX, bestMove.fromY, bestMove.toX, bestMove.toY);
        _castlingService.MarkPieceMoved(bestMove.fromX, bestMove.fromY);
        return bestMove;
    }

    public List<(int x, int y, bool IsEnemy)> GetValidMoves(int x, int y, string currentTurn,
        bool isCheck)
    {
        return _moveValidator.GetValidMoves(x, y, _board, currentTurn, _castlingService.MovementStates, isCheck);
    }
}