using System.Drawing;
using chess.Model;
using chess.Validators;


namespace chess.Helper;

public class MinMax
{
    public int depth { get; set; } = 3;
    public MoveValidator moveValidator { get; set; } = new MoveValidator();

    public (int fromX, int fromY, int toX, int toY) GetBestMove(Board board, string color)
    {
        double score = 0;
        (int fromX, int fromY, int toX, int toY)? bestMove = null;
        List<(int x, int y)> blackPieces = board.GetPiecePositionsByColor(color);
        // List<(int x, int y)> enemyPieces = board.GetPiecePositionsByColor(color == "White" ? "Black" : "White");
        
        foreach (var piece in blackPieces)
        {
            var validMoves = moveValidator.GetValidMoves(piece.x, piece.y, board, color);
            foreach (var move in validMoves)
            {

                var newBoard = board.DeepCopy();
                newBoard.MovePiece(piece.x, piece.y, move.x, move.y);
                var tempScore = MiniMax(newBoard, depth, false, color);
                
                if (score > tempScore)
                {
                    score = tempScore;
                    bestMove = (piece.x, piece.y, move.x, move.y);
                }
                
            }
        }

        return ((int fromX, int fromY, int toX, int toY))bestMove!;
    }
    
    private double MiniMax(Board board, int depth, bool isMaximizingPlayer, string currentTurn)
    {
        if (depth == 0 || IsGameOver(board, currentTurn))
        {
            return EvaluateBoard(board, currentTurn);
        }

        string opponentColor = currentTurn == "White" ? "Black" : "White";

        if (isMaximizingPlayer)
        {
            double maxEval = double.NegativeInfinity;
            var pieces = board.GetPiecePositionsByColor(currentTurn);
            foreach (var piece in pieces)
            {
                var validMoves = moveValidator.GetValidMoves(piece.x, piece.y, board, currentTurn);
                foreach (var move in validMoves)
                {
                    var newBoard = board.DeepCopy();
                    newBoard.MovePiece(piece.x, piece.y, move.x, move.y);
                    double eval = MiniMax(newBoard, depth - 1, false, opponentColor);
                    maxEval = Math.Max(maxEval, eval);
                }
            }
            return maxEval;
        }
        else
        {
            double minEval = double.PositiveInfinity;
            var pieces = board.GetPiecePositionsByColor(opponentColor);
            foreach (var piece in pieces)
            {
                var validMoves = moveValidator.GetValidMoves(piece.x, piece.y, board, opponentColor);
                foreach (var move in validMoves)
                {
                    var newBoard = board.DeepCopy();
                    newBoard.MovePiece(piece.x, piece.y, move.x, move.y);
                    double eval = MiniMax(newBoard, depth - 1, true, currentTurn);
                    minEval = Math.Min(minEval, eval);
                }
            }
            return minEval;
        }
    }
    
    private double EvaluateBoard(Board board, string currentTurn)
    {
        double score = 0;
        var pieces = board.GetPiecePositionsByColor(currentTurn);
        foreach (var piece in pieces)
        {
            score += board.Squares[piece.x, piece.y]?.Weight ?? 0;
        }
        var opponentColor = currentTurn == "White" ? "Black" : "White";
        var opponentPieces = board.GetPiecePositionsByColor(opponentColor);
        foreach (var piece in opponentPieces)
        {
            score -= board.Squares[piece.x, piece.y]?.Weight ?? 0;
        }
        return score;
    }

    private bool IsGameOver(Board board, string currentTurn)
    {
        var checkValidator = new CheckValidator();
        return checkValidator.Checkmate(board.GetPiecePositionsByColor(currentTurn).SelectMany(p => 
            moveValidator.GetValidMoves(p.x, p.y, board, currentTurn)).ToList());
    }
}