using chess.Model;
using chess.Validators;
using chess.Helper;


namespace chess.Service;

public class GameLogicService
{
    public event Action? TurnSwitched;
    public Board Board { get; private set; }
    private MoveValidator MoveValidator { get; set; }
    public CheckValidator CheckValidator { get; set; }
    public TimeTaking TimeTaking { get; set; }
    public MinMax MinMax { get; set; }
    public GameState GameState { get; set; }
    

    public GameLogicService()
    {
        Board = new Board();
        MoveValidator = new MoveValidator();
        CheckValidator = new CheckValidator();
        TimeTaking = new TimeTaking();
        MinMax = new MinMax();
        GameState = new GameState();
        StartGame();
    }

    public event Action<bool>? IsCheck
    {
        add => GameState.IsCheck += value;
        remove => GameState.IsCheck -= value;
    }

    public event Action<bool>? IsCheckMate
    {
        add => GameState.IsCheckMate += value;
        remove => GameState.IsCheckMate -= value;
    }

    public bool Check()
    {
        if (CheckValidator.Check(Board, GameState.CurrentTurn))
        {
            GameState.InvokeCheck(true);
            return true;
        }
        GameState.InvokeCheck(false);
        return false;
    }

    public bool CheckMate()
    {
        var friendlyPieces = Board.GetPiecePositionsByColor(GameState.CurrentTurn);
        var allMoves = new List<(int x, int y, bool IsEnemy)>();
        foreach (var piece in friendlyPieces) {
            allMoves.AddRange(MoveValidator.GetValidMoves(piece.x, piece.y, Board, GameState.CurrentTurn));
        }
        if (CheckValidator.Checkmate(allMoves))
        {
            GameState.InvokeCheckMate(true);
            return true;
        }
        GameState.InvokeCheckMate(false);
        return false;
    }
    
    public List<(int x, int y, bool IsEnemy)> GetValidMoves(int x, int y){
        var moves = MoveValidator.GetValidMoves(x, y, Board, GameState.CurrentTurn);
        if (!Check())
        {
            moves.AddRange(MoveValidator.GetCastlingMoves(x,y, Board, GameState.CurrentTurn, 
                GameState.WhiteKingMoved, GameState.BlackKingMoved, 
                GameState.WhiteLeftRookMoved, GameState.WhiteRightRookMoved, 
                GameState.BlackLeftRookMoved, GameState.BlackRightRookMoved));
        }
        return moves;
    }

    public void MakeMove(int fromColumn, int fromRow, int toColumn, int toRow)
    {
        Board.MovePiece(fromColumn, fromRow, toColumn, toRow);
        GameState.CurrentTurn = GameState.CurrentTurn == "White" ? "Black" : "White";
        GameState.KingOrRookMoved(fromColumn, fromRow);
        TimeTaking.SwitchTurn(GameState.CurrentTurn);
        Check();
        CheckMate();
        TurnSwitched?.Invoke();
    }

    public void MakeAiMove()
    {
        if (GameState.CurrentTurn == "Black")
        {
            Console.WriteLine("WTF");
            var bestMove = MinMax.GetBestMove(Board, "Black");
            Board.MovePiece(bestMove.fromX, bestMove.fromY, bestMove.toX, bestMove.toY);
            GameState.CurrentTurn = "White";
            TimeTaking.SwitchTurn(GameState.CurrentTurn);
            Check();
            CheckMate();
        }
    }

    public string GetCurrentTurn()
    {
        return GameState.CurrentTurn;
    }
    
    private void StartGame()
    {
        TimeTaking.CurrentTurn = GameState.CurrentTurn;
        TimeTaking.StartTimer();
    }

    public void Restart()
    {
        Board = new Board(); 
        GameState.CurrentTurn = "White";
        TimeTaking.StartTimer();
    }
}
