using chess.Model;

namespace chess.Interfaces
{
    public interface IGameStateSerice
    {
        string CurrentTurn { get; set; }
        bool IsCheck { get; }
        bool IsCheckMate { get; }
        void StartGame(Board board);
        void UpdateGameState(Board board, Dictionary<PlayerColor, Dictionary<PieceType, bool>> movementStates);
        void RestartGame(Board board);
        event Action<bool>? OnCheck;
        event Action<bool>? OnCheckMate;
    }
}