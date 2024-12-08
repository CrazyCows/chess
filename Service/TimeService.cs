using chess.Interfaces;

namespace chess.Helper;

public class TimeService : ITimeService
{
    private int _whiteTimeLeft = 300; 
    private int _blackTimeLeft = 300; 
    private readonly System.Timers.Timer _timer;
    public string CurrentTurn { get; set; } = "White";
    public event Action? OnTimeUpdated;
    public event Action<string>? OnTimeExpired;

    public TimeService()
    {
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += OnTimerElapsed!;
    }

    public void StartTimer()
    {
        Console.WriteLine("TIME STARTED");
        _timer.Start();
    }

    public void StopTimer()
    {
        _timer.Stop();
    }

    public void ResetTimer()
    {
        _whiteTimeLeft = 300;
        _blackTimeLeft = 300;
        CurrentTurn = "White";
    }

    public void SwitchTurn(string currentTurn)
    {
        CurrentTurn = currentTurn;
    }

    private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (CurrentTurn == "White")
        {
            _whiteTimeLeft--;
            if (_whiteTimeLeft <= 0)
            {
                _timer.Stop();
                Console.WriteLine("White ran out of time!");
                OnTimeExpired?.Invoke("White");
            }
        }
        else
        {
            _blackTimeLeft--;
            if (_blackTimeLeft <= 0)
            {
                _timer.Stop();
                Console.WriteLine("Black ran out of time!");
                OnTimeExpired?.Invoke("Black");
            }
        }

        OnTimeUpdated?.Invoke();
    }

    public (int whiteTime, int blackTime) GetRemainingTimes()
    {
        return (_whiteTimeLeft, _blackTimeLeft);
    }
}
