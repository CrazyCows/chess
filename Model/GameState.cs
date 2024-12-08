using chess.Validators;

namespace chess.Model;

public class GameState
{
    public event Action<bool>? IsCheck;
    public event Action<bool>? IsCheckMate;
    public bool Check { get; set; }
    public string CurrentTurn { get; set; } = "White";
    public CheckValidator CheckValidator { get; set; }


    
    public GameState()
    {
        CheckValidator = new CheckValidator();
    }
    
    public void CheckTurn(Board board)
    {
        if (CheckValidator.Check(board, CurrentTurn))
        {
            IsCheck?.Invoke(true);
            Check = true;
            return;
        }
        IsCheck?.Invoke(false);
        Check = false;
    }

    public void CheckMateTurn(List<(int x, int y, bool IsEnemy)> allMoves)
    {
        if (CheckValidator.Checkmate(allMoves))
        {
            IsCheckMate?.Invoke(true);
            return;
        }
        IsCheckMate?.Invoke(false);
    }
    
    public GameState DeepCopy()
    {
        var newGameState = new GameState
        {
            Check = this.Check,
            CurrentTurn = this.CurrentTurn,
        };
        
        return newGameState;
    }
    
}