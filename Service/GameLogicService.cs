using chess.Model;
using chess.Validators;
using chess.Helper;


namespace chess.Service;

public class GameLogicService
{
    public string CurrentTurn { get; set; } = "White";
    public Board Board { get; private set; }
    private MoveValidator MoveValidator { get; set; }
    public CheckValidator CheckValidator { get; set; }
    public TimeTaking TimeTaking { get; set; }
    public MinMax MinMax { get; set; }
    private readonly object _lock = new object();

    public GameLogicService()
    {
        Board = new Board();
        MoveValidator = new MoveValidator();
        CheckValidator = new CheckValidator();
        TimeTaking = new TimeTaking();
        MinMax = new MinMax();
        StartGame();
    }

    
    public List<(int x, int y, bool IsEnemy)> GetValidMoves(int x, int y){
        var moves = MoveValidator.GetValidMoves(x, y, Board, CurrentTurn);
        if (!CheckValidator.Check(Board, CurrentTurn))
        {
            moves.AddRange(MoveValidator.GetCastlingMoves(x,y, Board, CurrentTurn));
        }
        return moves;
    }

    public void MakeMove(int fromColumn, int fromRow, int toColumn, int toRow)
    {
        Board.MovePiece(fromColumn, fromRow, toColumn, toRow);
        CurrentTurn = CurrentTurn == "White" ? "Black" : "White";
        var friendlyPieces = Board.GetPiecePositionsByColor(CurrentTurn);
        var allMoves = new List<(int x, int y, bool IsEnemy)>();
        foreach (var piece in friendlyPieces) {
            allMoves.AddRange(MoveValidator.GetValidMoves(piece.x, piece.y, Board, CurrentTurn));
        }
        CheckValidator.Checkmate(allMoves);
        MoveValidator.KingOrRookMoved(fromColumn, fromRow);
        TimeTaking.SwitchTurn(CurrentTurn);
        CheckValidator.Check(Board, CurrentTurn);
        
    }

    public void MakeAiMove()
    {
        lock (_lock)
        {
            if (CurrentTurn == "Black")
            {
                var bestMove = MinMax.GetBestMove(Board, "Black");
                Board.MovePiece(bestMove.fromX, bestMove.fromY, bestMove.toX, bestMove.toY);
                CurrentTurn = "White";
                TimeTaking.SwitchTurn(CurrentTurn);
                CheckValidator.Check(Board, CurrentTurn);
                var friendlyPieces = Board.GetPiecePositionsByColor(CurrentTurn);
                var allMoves = new List<(int x, int y, bool IsEnemy)>();
                foreach (var piece in friendlyPieces) {
                    allMoves.AddRange(MoveValidator.GetValidMoves(piece.x, piece.y, Board, CurrentTurn));
                }
                CheckValidator.Checkmate(allMoves);
            }
        }
    }
    
    private void StartGame()
    {
        TimeTaking.CurrentTurn = CurrentTurn;
        TimeTaking.StartTimer();
    }

    public void Restart()
    {
        Board = new Board(); 
        CurrentTurn = "White";
        TimeTaking.StartTimer();
    }
}
