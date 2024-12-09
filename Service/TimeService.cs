using System.Timers;
using chess.Interfaces;
using Timer = System.Timers.Timer;

namespace chess.Service;

public class TimeService : ITimeService
{
    private readonly Timer _timer;
    private int _blackTimeLeft = 300;
    private int _whiteTimeLeft = 300;

    public TimeService()
    {
        _timer = new Timer(1000);
        _timer.Elapsed += OnTimerElapsed!;
    }

    public string CurrentTurn { get; set; } = "White";
    public event Action? OnTimeUpdated;
    public event Action<string>? OnTimeExpired;

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

    public (int whiteTime, int blackTime) GetRemainingTimes()
    {
        return (_whiteTimeLeft, _blackTimeLeft);
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
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
}