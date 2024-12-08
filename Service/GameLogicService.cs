using chess.Model;
using chess.Validators;
using chess.Helper;


namespace chess.Service;

public class GameLogicService
{
    public event Action? TurnSwitched;
    public Board Board { get; private set; }
    public MoveValidator MoveValidator { get; set; }
    public CheckValidator CheckValidator { get; set; }
    public TimeTaking TimeTaking { get; set; }
    public MinMax MinMax { get; set; }
    public GameState GameState { get; set; }


    public GameLogicService()
    {
        Board = new Board();
        Board.InitializeBoard();
        MoveValidator = new MoveValidator();
        CheckValidator = new CheckValidator();
        TimeTaking = new TimeTaking();
        MinMax = new MinMax();
        GameState = new GameState();
        StartGame();
    }

    public List<(int x, int y, bool IsEnemy)> GetValidMoves(int x, int y)
    {
        var moves = MoveValidator.GetValidMoves(x, y, Board, GameState.CurrentTurn, GameState.MovementStates,
            GameState.Check);

        return moves;
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

    public void MakeMove(int fromColumn, int fromRow, int toColumn, int toRow)
    {
        Board.MovePiece(fromColumn, fromRow, toColumn, toRow);
        GameState.CurrentTurn = GameState.CurrentTurn == "White" ? "Black" : "White";
        GameState.KingOrRookMoved(fromColumn, fromRow);
        TimeTaking.SwitchTurn(GameState.CurrentTurn);
        GameState.CheckTurn(Board);
        GameState.CheckMateTurn(MoveValidator.GetAllValidMoves(Board, GameState.CurrentTurn,
            GameState.MovementStates, GameState.Check));
        TurnSwitched?.Invoke();
    }

    public void MakeAiMove()
    {
        if (GameState.CurrentTurn == "Black")
        {
            Console.WriteLine("WTF");
            var bestMove = MinMax.GetBestMove(Board, GameState, "Black");
            Board.MovePiece(bestMove.fromX, bestMove.fromY, bestMove.toX, bestMove.toY);
            GameState.CurrentTurn = "White";
            GameState.KingOrRookMoved(bestMove.fromX, bestMove.fromY);
            TimeTaking.SwitchTurn(GameState.CurrentTurn);
            GameState.CheckTurn(Board);
            GameState.CheckMateTurn(MoveValidator.GetAllValidMoves(Board, GameState.CurrentTurn,
                GameState.MovementStates, GameState.Check));
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
        Board.InitializeBoard();
        GameState.CurrentTurn = "White";
        TimeTaking.StartTimer();
    }
}
