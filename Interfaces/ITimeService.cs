namespace chess.Interfaces;

public interface ITimeService
{
    string CurrentTurn { get; set; }
    void StartTimer();
    void StopTimer();
    void SwitchTurn(string currentTurn);
    void ResetTimer();
    (int whiteTime, int blackTime) GetRemainingTimes();
    event Action? OnTimeUpdated;
    event Action<string>? OnTimeExpired;
}