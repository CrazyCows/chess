using System.Drawing;
using chess.Model;
using chess.Model.Pieces;
using chess.Validators;


namespace chess.Helper;

public class MinMax
    {
        private readonly int _maxDepth = 4;
        MoveValidator _moveValidator = new MoveValidator();

        public (int fromX, int fromY, int toX, int toY) GetBestMove(Board board, string currentTurn)
        {
            double bestValue = currentTurn == "Black" ? int.MinValue : int.MaxValue;
            (int fromX, int fromY, int toX, int toY) bestMove = (0, 0, 0, 0);

            var pieces = board.GetPiecePositionsByColor(currentTurn);

            Parallel.ForEach(pieces, piece =>
            {
                Board boardTemp = board.DeepCopy();
                
                var validMoves = _moveValidator.GetValidMoves(piece.x, piece.y, boardTemp, "Black");
                
                foreach (var move in validMoves)
                {
                    
                    Console.WriteLine($"{piece.x} {piece.y} {move}");
                    var clonedBoard = boardTemp.DeepCopy();
                    clonedBoard.MovePiece(piece.x, piece.y, move.x, move.y);

                    double boardValue = Minimax(clonedBoard, _maxDepth - 1, currentTurn == "Black" ? false : true);

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

        private double Minimax(Board board, int depth, bool isMaximizing)
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
                var validMoves = _moveValidator.GetValidMoves(piece.x, piece.y, board, currentTurn);
                foreach (var move in validMoves)
                {
                    var clonedBoard = board.DeepCopy();
                    
                    clonedBoard.MovePiece(piece.x, piece.y, move.x, move.y);

                    double boardValue = Minimax(clonedBoard, depth - 1, !isMaximizing);

                    if (isMaximizing)
                    {
                        bestValue = Math.Max(bestValue, boardValue);
                    }
                    else
                    {
                        bestValue = Math.Min(bestValue, boardValue);
                    }
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