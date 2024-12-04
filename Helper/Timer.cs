using System;

public class Timer
{
    private int whiteTimeLeft = 300; 
    private int blackTimeLeft = 300; 
    private System.Timers.Timer timer;
    public string CurrentTurn { get; set; } = "White";
    public event Action OnTimeUpdated;
    public event Action<string> OnTimeExpired;

    public Timer()
    {
        timer = new System.Timers.Timer(1000);
        timer.Elapsed += OnTimerElapsed;
    }

    public void StartTimer()
    {
        Console.WriteLine("TIME STARTED");
        timer.Start();
    }

    public void StopTimer()
    {
        timer.Stop();
    }

    public void SwitchTurn(string currentTurn)
    {
        CurrentTurn = currentTurn;
    }

    private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (CurrentTurn == "White")
        {
            whiteTimeLeft--;
            if (whiteTimeLeft <= 0)
            {
                timer.Stop();
                Console.WriteLine("White ran out of time!");
                OnTimeExpired?.Invoke("White");
            }
        }
        else
        {
            blackTimeLeft--;
            if (blackTimeLeft <= 0)
            {
                timer.Stop();
                Console.WriteLine("Black ran out of time!");
                OnTimeExpired?.Invoke("Black");
            }
        }

        OnTimeUpdated?.Invoke();
    }

    public (int whiteTime, int blackTime) GetRemainingTimes()
    {
        return (whiteTimeLeft, blackTimeLeft);
    }
}
