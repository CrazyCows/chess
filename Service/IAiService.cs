using chess.Interfaces;
using chess.Model;

namespace chess.Service;

public class AiService : IAiService
    {
        private readonly int _maxDepth = 3;
        private IMoveValidator MoveValidator { get; }

        public AiService(IMoveValidator moveValidator)
        {
            MoveValidator = moveValidator;
        }


        public (int fromX, int fromY, int toX, int toY) GetBestMove(Board board, GameState gameState, 
            CastlingState castlingState, string currentTurn)
        {
            double bestValue = currentTurn == "Black" ? int.MinValue : int.MaxValue;
            (int fromX, int fromY, int toX, int toY) bestMove = (0, 0, 0, 0);

            var pieces = board.GetPiecePositionsByColor(currentTurn);

            Parallel.ForEach(pieces, piece =>
            {
                Board boardTemp = board.DeepCopy();
                GameState gameTemp = gameState.DeepCopy();
                CastlingState castlingStateTemp = castlingState.DeepCopy();
                var validMoves = MoveValidator.GetValidMoves(piece.x, piece.y, boardTemp, 
                    currentTurn, castlingStateTemp.MovementStates, gameTemp.Check);
                
                foreach (var move in validMoves)
                {
                    //Console.WriteLine(gameTemp.Check);
                    //Console.WriteLine($"{piece.x} {piece.y} {move}");
                    Board clonedBoard = boardTemp.DeepCopy();
                    GameState gameTemp2 = gameState.DeepCopy();
                    CastlingState castlingStateTemp2 = castlingState.DeepCopy();
                    clonedBoard.MovePiece(piece.x, piece.y, move.x, move.y);

                    double boardValue = Minimax(clonedBoard, gameTemp2, castlingStateTemp2, _maxDepth - 1, 
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

        private double Minimax(Board board, GameState gameState, CastlingState castlingState, int depth, bool isMaximizing)
        {
            if (depth == 0)
            {
                return EvaluateBoard(board);
            }

            string currentTurn = isMaximizing ? "Black" : "White";
            double bestValue = isMaximizing ? int.MinValue : int.MaxValue;

            var pieces = board.GetPiecePositionsByColor(currentTurn);

            foreach (var piece in pieces)
            {
                var validMoves = MoveValidator.GetValidMoves(piece.x, piece.y, board, currentTurn, 
                    castlingState.MovementStates, gameState.Check);
                foreach (var move in validMoves)
                {
                    var clonedBoard = board.DeepCopy();
                    GameState gameTemp = gameState.DeepCopy();
                    CastlingState castlingStateTemp = castlingState.DeepCopy();
                    clonedBoard.MovePiece(piece.x, piece.y, move.x, move.y);

                    double boardValue = Minimax(clonedBoard, gameTemp, castlingStateTemp, depth - 1, !isMaximizing);

                    bestValue = isMaximizing ? Math.Max(bestValue, boardValue) : Math.Min(bestValue, boardValue);
                }
            }

            return bestValue;
        }

        private double EvaluateBoard(Board board)
        {
            double whiteScore = 0;
            double blackScore = 0;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var piece = board.Squares[x, y];
                    if (piece != null)
                    {
                        if (piece.Color == "White")
                        {
                            whiteScore += piece.Weight;
                        }
                        
                        else if (piece.Color == "Black")
                        {
                            blackScore += piece.Weight;
                        }
                    }
                }
            }

            return blackScore - whiteScore;
        }
    }