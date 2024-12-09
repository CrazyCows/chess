using chess.Model;

namespace chess.Interfaces;

public interface IGameStateSerice
{
    string CurrentTurn { get; set; }
    bool IsCheck { get; }
    bool IsCheckMate { get; }
    void StartGame();
    void UpdateGameState();
    void RestartGame();
    event Action<bool>? OnCheck;
    event Action<bool>? OnCheckMate;
}