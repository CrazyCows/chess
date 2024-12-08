using chess.Interfaces;
using chess.Model;
using chess.Validators;

namespace chess.Service
{
    public class GameStateSerice : IGameStateSerice
    {
        private readonly ICheckValidator _checkValidator;
        private readonly IMoveValidator _moveValidator;

        public event Action<bool>? OnCheck;
        public event Action<bool>? OnCheckMate;

        public string CurrentTurn { get; set; } = "White";
        public bool IsCheck { get; private set; }
        public bool IsCheckMate { get; private set; }

        public GameStateSerice(ICheckValidator checkValidator, IMoveValidator moveValidator)
        {
            _checkValidator = checkValidator;
            _moveValidator = moveValidator;
        }

        public void StartGame(Board board)
        {
            board.InitializeBoard();
            CurrentTurn = "White";
            IsCheck = false;
            IsCheckMate = false;
        }

        public void UpdateGameState(Board board, Dictionary<PlayerColor, Dictionary<PieceType, bool>> movementStates)
        {
            IsCheck = _checkValidator.Check(board, CurrentTurn);
            OnCheck?.Invoke(IsCheck);

            var allValidMoves = _moveValidator.GetAllValidMoves(board, CurrentTurn, movementStates, IsCheck);
            IsCheckMate = _checkValidator.Checkmate(allValidMoves);
            OnCheckMate?.Invoke(IsCheckMate);
        }

        public void RestartGame(Board board)
        {
            StartGame(board);
        }
    }
}