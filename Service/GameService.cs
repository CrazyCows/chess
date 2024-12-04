using chess.Models;
using chess.Models.Pieces;

public class GameService
{
    public string CurrentTurn { get; set; } = "White";
    public string enemyColor { get; set; } = "Black";
    public event Action<bool>? IsCheck;
    public event Action<bool>? IsCheckMate;
    public event Action<string>? GameOverDueToTime;

    public Board Board { get; private set; } = new Board();
    public Timer Timer { get; private set; }
    public GameService()
    {
        Timer = new Timer();
        Timer.OnTimeExpired += HandleTimeExpired;
        StartGame();
    }

    private void HandleTimeExpired(string playerColor)
    {
        GameOverDueToTime?.Invoke(playerColor);
    }

    private bool Check((int x, int y) from, (int x, int y) to)
    {
        Piece fromPiece = Board.Squares[from.x, from.y];
        Piece toPiece = Board.Squares[to.x, to.y];
        Board.Squares[to.x, to.y] = fromPiece;
        Board.Squares[from.x, from.y] = null;

        var Squares = Board.Squares;
        var kingPos = Board.GetKingPos(CurrentTurn);
        if (kingPos == null)
        {
            return true;
        }

        var friendlyPieces = Board.GetPiecePositionsByColor(CurrentTurn);
        var enemyPieces = Board.GetPiecePositionsByColor(enemyColor);

        Board.Squares[to.x, to.y] = toPiece;
        Board.Squares[from.x, from.y] = fromPiece;

        foreach (var ep in enemyPieces) {
            Piece piece = Squares[ep.x, ep.y];
            var moves = piece.GetMoves((ep.x, ep.y), friendlyPieces, enemyPieces);
            if (moves.Any(move => move.x == kingPos.Value.x && move.y == kingPos.Value.y))
            {
                return true;
            }
        }
        return false;
    }

    public void CheckTurn()
    {
        var Squares = Board.Squares;
        var kingPos = Board.GetKingPos(CurrentTurn);
        if (kingPos == null)
        {
            IsCheck?.Invoke(true);
            return;
        }

        var friendlyPieces = Board.GetPiecePositionsByColor(CurrentTurn);
        var enemyPieces = Board.GetPiecePositionsByColor(enemyColor);


        foreach (var ep in enemyPieces) {
            Piece piece = Squares[ep.x, ep.y];
            var moves = piece.GetMoves((ep.x, ep.y), friendlyPieces, enemyPieces);
            if (moves.Any(move => move.x == kingPos.Value.x && move.y == kingPos.Value.y))
            {
                IsCheck?.Invoke(true);
                return;
            }
        }
        IsCheck?.Invoke(false);
        return;
    }

    public void Checkmate()
    {
        var friendlyPieces = Board.GetPiecePositionsByColor(CurrentTurn);

        foreach (var piecePosition in friendlyPieces)
        {
            var piece = Board.Squares[piecePosition.x, piecePosition.y];
            var validMoves = GetValidMoves(piecePosition.x, piecePosition.y);

            if (validMoves.Count > 0)
            {
                IsCheckMate?.Invoke(false);
                return;
            }
        }

        // No valid moves for any piece, and king is in check
        var kingPos = Board.GetKingPos(CurrentTurn);
        if (kingPos != null && Check(kingPos.Value, kingPos.Value))
        {
            Console.WriteLine("CHECKMATE");
            IsCheckMate?.Invoke(true);
            return;
            
        }
        IsCheckMate?.Invoke(false);
        return;
    }

    public List<(int x, int y, bool IsEnemy)> GetValidMoves(int x, int y)
    {
        Piece piece = Board.Squares[x, y];
        //Console.WriteLine(CurrentTurn);
        var friendlyPieces = Board.GetPiecePositionsByColor(CurrentTurn);
        var enemyPieces = Board.GetPiecePositionsByColor(enemyColor);

        var moves = piece.GetMoves((x, y), enemyPieces, friendlyPieces);
        var finalMoves = new List<(int x, int y, bool IsEnemy)>();

        foreach (var move in moves)
        {
            // Console.WriteLine(move);
            // finalMoves.Add(move);
            // Only add the move if it doesn't leave the king in check
            if (!Check((x, y), (move.x, move.y)))
            {
                finalMoves.Add(move);
                
            } 
        }
        
        return finalMoves;
    }

    public void MakeMove(int fromColumn, int fromRow, int toColumn, int toRow)
    {
        Board.MovePiece(fromColumn, fromRow, toColumn, toRow);

        CurrentTurn = CurrentTurn == "White" ? "Black" : "White";
        enemyColor = CurrentTurn == "White" ? "Black" : "White";
        
        CheckTurn();
        Checkmate();
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
        Timer = new Timer();
        Timer.OnTimeExpired += HandleTimeExpired;
        Timer.StartTimer();
    }

}
