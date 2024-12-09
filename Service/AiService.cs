using chess.Interfaces;
using chess.Model;

namespace chess.Service;

public class AiService : IAiService
{
    private readonly int _maxDepth = 3;

    public AiService(IMoveValidator moveValidator, IBoard board,
        ICastlingService castlingState, ICheckValidator checkValidator)
    {
        MoveValidator = moveValidator;
        Board = board;
        CastlingService = castlingState;
        CheckValidator = checkValidator;
    }

    private IMoveValidator MoveValidator { get; }
    private IBoard Board { get; }
    private ICastlingService CastlingService { get; }
    private ICheckValidator CheckValidator { get; }


    public (int fromX, int fromY, int toX, int toY) GetBestMove(string currentTurn)
    {
        double bestValue = currentTurn == "Black" ? int.MinValue : int.MaxValue;
        (int fromX, int fromY, int toX, int toY) bestMove = (0, 0, 0, 0);

        var pieces = Board.GetPiecePositionsByColor(currentTurn);

        Parallel.ForEach(pieces, piece =>
        {
            var boardTemp = Board.DeepCopy();
            var castlingStateTemp = CastlingService.DeepCopy();
            bool isCheck = CheckValidator.Check(boardTemp, currentTurn);
            var validMoves = MoveValidator.GetValidMoves(piece.x, piece.y, boardTemp,
                currentTurn, castlingStateTemp.MovementStates, isCheck);

            foreach (var move in validMoves)
            {
                //Console.WriteLine(gameTemp.Check);
                //Console.WriteLine($"{piece.x} {piece.y} {move}");
                var clonedBoard = boardTemp.DeepCopy();
                var castlingStateTemp2 = CastlingService.DeepCopy();
                clonedBoard.MovePiece(piece.x, piece.y, move.x, move.y);
                isCheck = CheckValidator.Check(boardTemp, currentTurn);
                var boardValue = Minimax(clonedBoard, castlingStateTemp2, isCheck, _maxDepth,
                    currentTurn != "Black");

                if ((currentTurn == "Black" && boardValue > bestValue) ||
                    (currentTurn == "White" && boardValue < bestValue))
                {
                    bestValue = boardValue;
                    bestMove = (piece.x, piece.y, move.x, move.y);
                }
            }
        });

        return bestMove;
    }

    private double Minimax(Board board, ICastlingService castlingState, bool isCheck, int depth, bool isMaximizing)
    {
        if (depth == 0) return EvaluateBoard(board);

        var currentTurn = isMaximizing ? "Black" : "White";
        double bestValue = isMaximizing ? int.MinValue : int.MaxValue;

        var pieces = board.GetPiecePositionsByColor(currentTurn);

        foreach (var piece in pieces)
        {
            var validMoves = MoveValidator.GetValidMoves(piece.x, piece.y, board, currentTurn,
                castlingState.MovementStates, isCheck);
            foreach (var move in validMoves)
            {
                var clonedBoard = board.DeepCopy();
                var castlingStateTemp = castlingState.DeepCopy();
                clonedBoard.MovePiece(piece.x, piece.y, move.x, move.y);
                isCheck = CheckValidator.Check(clonedBoard, currentTurn);

                var boardValue = Minimax(clonedBoard, castlingStateTemp, isCheck, depth - 1, !isMaximizing);

                bestValue = isMaximizing ? Math.Max(bestValue, boardValue) : Math.Min(bestValue, boardValue);
            }
        }

        return bestValue;
    }

    private double EvaluateBoard(Board board)
    {
        double whiteScore = 0;
        double blackScore = 0;

        for (var x = 0; x < 8; x++)
        for (var y = 0; y < 8; y++)
        {
            var piece = board.Squares[x, y];
            if (piece != null)
            {
                if (piece.Color == "White")
                    whiteScore += piece.Weight;

                else if (piece.Color == "Black") blackScore += piece.Weight;
            }
        }

        return blackScore - whiteScore;
    }
}