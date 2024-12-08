using chess.Interfaces;
using chess.Model;

namespace chess.Service
{
    public class GameLogicService
    {
        private readonly IGameStateSerice _gameStateManager;
        private readonly IMoveService _moveService;
        private readonly IAiService _aiStrategy;
        public readonly ITimeService TimeService;
        public readonly Board Board;
        private readonly CastlingState _castlingState;

        public event Action? TurnSwitched;
        public event Action<bool>? IsCheck
        {
            add => _gameStateManager.OnCheck += value;
            remove => _gameStateManager.OnCheck -= value;
        }

        public event Action<bool>? IsCheckMate
        {
            add => _gameStateManager.OnCheckMate += value;
            remove => _gameStateManager.OnCheckMate -= value;
        }

        public GameLogicService(IGameStateSerice gameStateManager, 
                                    IMoveService moveService, 
                                    IAiService aiStrategy, 
                                    ITimeService timeService)
        {
            _gameStateManager = gameStateManager;
            _moveService = moveService;
            _aiStrategy = aiStrategy;
            TimeService = timeService;

            Board = new Board();
            _castlingState = new CastlingState();
            
            StartGame();
        }

        public void StartGame()
        {
            _gameStateManager.StartGame(Board);
            TimeService.CurrentTurn = _gameStateManager.CurrentTurn;
            TimeService.StartTimer();
        }

        public List<(int x, int y, bool IsEnemy)> GetValidMoves(int x, int y)
        {
            return _moveService.GetValidMoves(Board, x, y, _gameStateManager.CurrentTurn, _castlingState, _gameStateManager.IsCheck);
        }

        public void MakeMove(int fromColumn, int fromRow, int toColumn, int toRow)
        {
            _moveService.MakeMove(Board, fromColumn, fromRow, toColumn, toRow, _gameStateManager.CurrentTurn, _castlingState);

            // Switch turn in GameState and TimeService
            _gameStateManager.CurrentTurn = _gameStateManager.CurrentTurn == "White" ? "Black" : "White";
            TimeService.SwitchTurn(_gameStateManager.CurrentTurn);
            
            // Update the game state after the move
            _gameStateManager.UpdateGameState(Board, _castlingState.MovementStates);

            TurnSwitched?.Invoke();
        }

        public void MakeAiMove()
        {
            if (_gameStateManager.CurrentTurn == "Black")
            {
                var bestMove = _moveService.MakeAiMove(Board, _gameStateManager.CurrentTurn, _castlingState, _aiStrategy);
                
                _gameStateManager.CurrentTurn = "White";
                TimeService.SwitchTurn(_gameStateManager.CurrentTurn);
                _gameStateManager.UpdateGameState(Board, _castlingState.MovementStates);
            }
        }

        public string GetCurrentTurn() => _gameStateManager.CurrentTurn;

        public void Restart()
        {
            _gameStateManager.RestartGame(Board);
            TimeService.ResetTimer();
            TimeService.StartTimer();
        }
    }
}
