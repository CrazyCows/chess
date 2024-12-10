using chess.Interfaces;
using chess.Model;

namespace chess.Service;

public class GameLogicService
{
    private readonly IGameStateSerice _gameStateService;
    private readonly IMoveService _moveService;
    public readonly IBoard Board;
    public readonly ITimeService TimeService;
    private readonly ICastlingService _castlingService;

    public GameLogicService(IGameStateSerice gameStateService,
        IMoveService moveService,
        ITimeService timeService,
        IBoard board,
        ICastlingService castlingService)
    {
        _gameStateService = gameStateService;
        _moveService = moveService;
        TimeService = timeService;
        _castlingService = castlingService;
        Board = board;

        StartGame();
    }

    public event Action? TurnSwitched;

    public event Action<bool>? IsCheck
    {
        add => _gameStateService.OnCheck += value;
        remove => _gameStateService.OnCheck -= value;
    }

    public event Action<bool>? IsCheckMate
    {
        add => _gameStateService.OnCheckMate += value;
        remove => _gameStateService.OnCheckMate -= value;
    }

    public void StartGame()
    {
        _gameStateService.StartGame();
        TimeService.CurrentTurn = _gameStateService.CurrentTurn;
        TimeService.StartTimer();
    }

    public List<(int x, int y, bool IsEnemy)> GetValidMoves(int x, int y)
    {
        return _moveService.GetValidMoves(x, y, _gameStateService.CurrentTurn,
            _gameStateService.IsCheck);
    }

    public void MakeMove(int fromColumn, int fromRow, int toColumn, int toRow)
    {
        _moveService.MakeMove(fromColumn, fromRow, toColumn, toRow);

        _gameStateService.CurrentTurn = _gameStateService.CurrentTurn == "White" ? "Black" : "White";
        TimeService.SwitchTurn(_gameStateService.CurrentTurn);

        _gameStateService.UpdateGameState();

        TurnSwitched?.Invoke();
    }

    public void MakeAiMove()
    {
        if (_gameStateService.CurrentTurn == "Black")
        {
            var bestMove = _moveService.MakeAiMove(_gameStateService.CurrentTurn);

            _gameStateService.CurrentTurn = "White";
            TimeService.SwitchTurn(_gameStateService.CurrentTurn);
            _gameStateService.UpdateGameState();
        }
    }

    public string GetCurrentTurn()
    {
        return _gameStateService.CurrentTurn;
    }

    public void Restart()
    {
        _gameStateService.RestartGame();
        _castlingService.ResetMovementStates();
        TimeService.ResetTimer();
        TimeService.StartTimer();
    }
}