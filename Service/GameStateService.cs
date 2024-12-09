using chess.Interfaces;
using chess.Model;

namespace chess.Service;

public class GameStateSerice : IGameStateSerice
{
    private readonly ICheckValidator _checkValidator;
    private readonly IMoveValidator _moveValidator;
    private readonly IBoard _board;
    private readonly ICastlingService _castlingService;

    public GameStateSerice(ICheckValidator checkValidator, 
        IMoveValidator moveValidator,
        IBoard board,
        ICastlingService castlingService)
    {
        _checkValidator = checkValidator;
        _moveValidator = moveValidator;
        _board = board;
        _castlingService = castlingService;
    }

    public event Action<bool>? OnCheck;
    public event Action<bool>? OnCheckMate;

    public string CurrentTurn { get; set; } = "White";
    public bool IsCheck { get; private set; }
    public bool IsCheckMate { get; private set; }

    public void StartGame()
    {
        _board.InitializeBoard();
        CurrentTurn = "White";
        IsCheck = false;
        IsCheckMate = false;
    }

    public void UpdateGameState()
    {
        IsCheck = _checkValidator.Check(_board, CurrentTurn);
        OnCheck?.Invoke(IsCheck);
        var allValidMoves = _moveValidator.GetAllValidMoves(_board, CurrentTurn, 
            _castlingService.MovementStates, IsCheck);
        IsCheckMate = _checkValidator.Checkmate(allValidMoves);
        OnCheckMate?.Invoke(IsCheckMate);
    }

    public void RestartGame()
    {
        StartGame();
    }
}