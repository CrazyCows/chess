using chess.Model;
using chess.Model.Pieces;

namespace chess.Validators;

public class MoveValidator
{
    private bool ValidateMove((int x, int y) from, (int x, int y) to, Board board, string currentTurn)
    {
        Piece fromPiece = board.Squares[from.x, from.y]!;
        Piece toPiece = board.Squares[to.x, to.y]!;
        board.Squares[to.x, to.y] = fromPiece;
        board.Squares[from.x, from.y] = null;

        var squares = board.Squares;
        if (squares[to.x, to.y] == null) return false;
        var kingPos = board.GetKingPos(currentTurn);
        if (kingPos == null)
        {
            return false;
        }

        var friendlyPieces = board.GetPiecePositionsByColor(currentTurn);
        var enemyPieces = board.GetPiecePositionsByColor(currentTurn == "White" ? "Black" : "White");
        board.Squares[to.x, to.y] = toPiece;
        board.Squares[from.x, from.y] = fromPiece;


        foreach (var ep in enemyPieces) {
            Piece piece = squares[ep.x, ep.y]!;
            if (piece == null)
            {
                Console.WriteLine("PIECE IS NULL FOR SOME REASON?!");
            }
            try
            {
                var moves = piece.GetMoves((ep.x, ep.y), friendlyPieces, enemyPieces);
                if (moves == null) return false;
                if (moves.Any(move => move.x == kingPos.Value.x && move.y == kingPos.Value.y))
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("HELLO");
                Console.WriteLine(currentTurn);
                Console.WriteLine(ep);
                Console.WriteLine($"{kingPos.Value.x}, {kingPos.Value.y}");
                Console.WriteLine(e);
                return false;
            }
        }
        return true;
    }

    


    private List<(int x, int y, bool IsEnemy)> GetValidCastlingMoves(int x, int y, Board board, 
        string friendlyColor, Dictionary<GameState.PlayerColor, Dictionary<GameState.PieceType, bool>> movementStates)
    {
        List<(int x, int y, bool IsEnemy)> moves = new List<(int x, int y, bool IsEnemy)>();
        if (!(board.Squares[x,y]?.GetType() == typeof(King)))
        {
            return moves;
        }
        //Console.WriteLine($"x: {x}, y: {y}");
        if (!movementStates[GameState.PlayerColor.White][GameState.PieceType.King]){
            if (board.Squares[7,0] != null && board.Squares[7,0]!.Color == "White" 
                                           && friendlyColor == "White" && !movementStates[GameState.PlayerColor.White][GameState.PieceType.LeftRook] 
                                           && board.Squares[7,1] == null && board.Squares[7,2] == null 
                                           && board.Squares[7,3] == null) 
            {
                board.Squares[7,0] = null;
                board.Squares[7,3] = new Rook("White");
                
                if (ValidateMove((7,4),(7,2), board, friendlyColor))
                {
                    moves.Add((7, 0, false));
                }
                board.Squares[7,0] = new Rook("White");
                board.Squares[7,3] = null;
            }
            if (board.Squares[7,7] != null && board.Squares[7,7]!.Color == "White" 
                                           && friendlyColor == "White" && !movementStates[GameState.PlayerColor.White][GameState.PieceType.RightRook]
                                           && board.Squares[7,5] == null && board.Squares[7,6] == null)  
            {
                board.Squares[7,7] = null;
                board.Squares[7,5] = new Rook("White");

                if (ValidateMove((7,4), (7,6), board, friendlyColor))
                {
                    moves.Add((7, 7, false));
                }
                board.Squares[7,7] = new Rook("White");
                board.Squares[7,5] = null;
            }
        }
        
        if (!movementStates[GameState.PlayerColor.Black][GameState.PieceType.King])
        {
            if (board.Squares[0,0] != null && board.Squares[0,0]!.Color == "Black" 
                                           && friendlyColor == "Black" && !movementStates[GameState.PlayerColor.Black][GameState.PieceType.LeftRook]
                    && board.Squares[0,1] == null && board.Squares[0,2] == null && board.Squares[0,3] == null) 
            {
                board.Squares[0,0] = null;
                board.Squares[0,3] = new Rook("Black");

                if (ValidateMove((0,4),(0,2), board, friendlyColor))
                {
                    moves.Add((0, 0, false));
                }
                board.Squares[0,0] = new Rook("Black");
                board.Squares[0,3] = null;
            }
            if (board.Squares[0,7] != null && board.Squares[0,7]!.Color == "Black" 
                                           && friendlyColor == "Black" && !movementStates[GameState.PlayerColor.Black][GameState.PieceType.RightRook]
                    && board.Squares[0,5] == null && board.Squares[0,6] == null) 
            {

                board.Squares[0,7] = null;
                board.Squares[0,5] = new Rook("Black");
                
                if (ValidateMove((0,4), (0,6), board, friendlyColor))
                {
                    moves.Add((0, 7, false));
                }

                board.Squares[0,7] = new Rook("Black");
                board.Squares[0,5] = null;
            }
        }
        return moves;
    }
    


    public List<(int x, int y, bool IsEnemy)> GetValidMoves(int x, int y, Board board, string friendlyColor,
        Dictionary<GameState.PlayerColor, Dictionary<GameState.PieceType, bool>> movementStates, bool IsCheck)
    {
        var friendlyPieces = board.GetPiecePositionsByColor(friendlyColor);
        var enemyPieces = board.GetPiecePositionsByColor(friendlyColor == "White" ? "Black" : "White");
        Piece piece = board.Squares[x, y]!;

        List<(int x, int y, bool IsEnemy)> moves = piece.GetMoves((x, y), enemyPieces, friendlyPieces);
        // var finalMoves = new List<(int x, int y, bool IsEnemy)>();

        //foreach (var move in moves)
        //{
        //    if (ValidateMove((x, y), (move.x, move.y), board, friendlyColor))
        //    {
        //        finalMoves.Add(move);
        //    } 
        //}
        if (!IsCheck)
        {
            moves.AddRange(GetValidCastlingMoves(x,y, board, friendlyColor, movementStates));
        }
        return moves;
    }

    public List<(int x, int y, bool IsEnemy)> GetAllValidMoves(Board board, string friendlyColor,
        Dictionary<GameState.PlayerColor, Dictionary<GameState.PieceType, bool>> movementStates, bool IsCheck)
    {
        var friendlyPieces = board.GetPiecePositionsByColor(friendlyColor);
        var allMoves = new List<(int x, int y, bool IsEnemy)>();
        foreach (var piece in friendlyPieces) {
            allMoves.AddRange(GetValidMoves(piece.x, piece.y, board, friendlyColor, movementStates, IsCheck));
        }
        return allMoves;
    }
}