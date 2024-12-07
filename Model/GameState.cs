namespace chess.Model;

public class GameState
{
    public event Action<bool>? IsCheck;
    public event Action<bool>? IsCheckMate;
    public bool WhiteKingMoved { get; set; }
    public bool WhiteRightRookMoved { get; set; }
    public bool WhiteLeftRookMoved { get; set; }
    public bool BlackKingMoved { get; set; }
    public bool BlackRightRookMoved { get; set; }
    public bool BlackLeftRookMoved { get; set; }
    public string CurrentTurn { get; set; } = "White";
    
    public void KingOrRookMoved(int fromColumn, int fromRow)
    {
        if (fromColumn == 0 && fromRow == 0) {
            BlackLeftRookMoved = true;
        }
        if (fromColumn == 0 && fromRow == 7) {
            BlackRightRookMoved = true;
        }
        if (fromColumn == 7 && fromRow == 0) {
            WhiteLeftRookMoved = true;
        }
        if (fromColumn == 7 && fromRow == 7) {
            WhiteRightRookMoved = true;
        }
        if (fromColumn == 0 && fromRow == 4) {
            BlackKingMoved = true;
        }
        if (fromColumn == 7 && fromRow == 4) {
            WhiteKingMoved = true;
        }
    }

    public void InvokeCheck(bool isCheck) => IsCheck?.Invoke(isCheck);
    public void InvokeCheckMate(bool isCheckMate) => IsCheckMate?.Invoke(isCheckMate);
    
}