using chess.Models;

public class GameLogicService
{
    public string CurrentTurn { get; set; } = "White";
    public string enemyColor { get; set; } = "Black";

    public Board Board { get; private set; } = new Board();
    public MoveValidator MoveValidator { get; set; } = new MoveValidator();
    public CheckValidator CheckValidator { get; set; } = new CheckValidator();
    public Timer Timer { get; set; } = new Timer();

    public GameLogicService()
    {
        Timer.OnTimeExpired += Timer.HandleTimeExpired;
        StartGame();
    }

    
    public List<(int x, int y, bool IsEnemy)> GetValidMoves(int x, int y){
        var moves = MoveValidator.GetValidMoves(x, y, Board, CurrentTurn);
        return moves;
    }

    public void MakeMove(int fromColumn, int fromRow, int toColumn, int toRow)
    {
        Board.MovePiece(fromColumn, fromRow, toColumn, toRow);
        CurrentTurn = CurrentTurn == "White" ? "Black" : "White";
        enemyColor = CurrentTurn == "White" ? "Black" : "White";
        var friendlyPieces = Board.GetPiecePositionsByColor(CurrentTurn);
        bool check = CheckValidator.Check(Board, CurrentTurn);
        if (check) {
            var allMoves = new List<(int x, int y, bool IsEnemy)>();
            foreach (var piece in friendlyPieces) {
                allMoves.AddRange(MoveValidator.GetValidMoves(piece.x, piece.y, Board, CurrentTurn));
                CheckValidator.Checkmate(allMoves);
            }
        }
        MoveValidator.KingOrRookMoved(fromColumn, fromRow);
        Timer.SwitchTurn(CurrentTurn);
    }

    public void StartGame()
    {
        Timer.CurrentTurn = CurrentTurn;
        Timer.StartTimer();
    }

    public void Restart()
    {
        Board = new Board(); 
        CurrentTurn = "White";
        enemyColor = "Black";
        Timer.OnTimeExpired += Timer.HandleTimeExpired;
        Timer.StartTimer();
    }
}
