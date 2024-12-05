using chess.Models;
using chess.Models.Pieces;


public class MoveValidator
{
    private bool WhiteKingMoved { get; set; }
    private bool WhiteRightRookMoved { get; set; }
    private bool WhiteLeftRookMoved { get; set; }
    private bool BlackKingMoved { get; set; }
    private bool BlackRightRookMoved { get; set; }
    private bool BlackLeftRookMoved { get; set; }



    public bool ValidateMove((int x, int y) from, (int x, int y) to, Board Board)
    {
        Piece fromPiece = Board.Squares[from.x, from.y];
        Piece toPiece = Board.Squares[to.x, to.y];
        Board.Squares[to.x, to.y] = fromPiece;
        Board.Squares[from.x, from.y] = null;

        var Squares = Board.Squares;
        var kingPos = Board.GetKingPos(fromPiece.Color);
        if (kingPos == null)
        {
            return true;
        }

        var friendlyPieces = Board.GetPiecePositionsByColor(fromPiece.Color);
        var enemyPieces = Board.GetPiecePositionsByColor(fromPiece.Color == "White" ? "Black" : "White");
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

    public void KingOrRookMoved(int fromColumn, int fromRow)
    {
        if (fromColumn == 0 && fromRow == 0) {
            BlackLeftRookMoved = true;
        }
        if (fromColumn == 0 && fromRow == 7) {
            BlackRightRookMoved = true;
        }
        if (fromColumn == 7 && fromRow == 0) {
            WhiteLeftRookMoved = true;
        }
        if (fromColumn == 7 && fromRow == 7) {
            WhiteRightRookMoved = true;
        }
        if (fromColumn == 0 && fromRow == 4) {
            BlackKingMoved = true;
        }
        if (fromColumn == 7 && fromRow == 4) {
            WhiteKingMoved = true;
        }
    }


    public List<(int x, int y, bool IsEnemy)> GetCastlingMoves(int x, int y, Board Board, string friendlyColor, (int x, int y)? kingPos, List<(int x, int y)> friendlyPieces, List<(int x, int y)> enemyPieces) {

        List<(int x, int y, bool IsEnemy)> moves = new List<(int x, int y, bool IsEnemy)>();
        if (!(Board.Squares[x,y].GetType() == typeof(King)))
        {
            return moves;
        }
        
        if (!WhiteKingMoved){
            if (Board.Squares[7,0] != null && Board.Squares[7,0].Color == "White" && friendlyColor == "White" && !WhiteLeftRookMoved 
                    && Board.Squares[7,1] == null && Board.Squares[7,2] == null && Board.Squares[7,3] == null) 
            {
                Board.Squares[7,0] = null;
                Board.Squares[7,3] = new Rook("White");
                
                if (!ValidateMove((7,4),(7,2), Board))
                {
                    moves.Add((7, 0, false));
                }
                Board.Squares[7,0] = new Rook("White");
                Board.Squares[7,3] = null;
            }
            if (Board.Squares[7,7] != null && Board.Squares[7,7].Color == "White" && friendlyColor == "White" && !WhiteRightRookMoved
                    && Board.Squares[7,5] == null && Board.Squares[7,6] == null)  
            {
                Board.Squares[7,7] = null;
                Board.Squares[7,5] = new Rook("White");

                if (!ValidateMove((7,4), (7,6), Board))
                {
                    moves.Add((7, 7, false));
                }
                Board.Squares[7,7] = new Rook("White");
                Board.Squares[7,5] = null;
            }
        }
        
        if (!BlackKingMoved)
        {
            if (Board.Squares[0,0] != null && Board.Squares[0,0].Color == "Black" && friendlyColor == "Black" && !BlackLeftRookMoved
                    && Board.Squares[0,1] == null && Board.Squares[0,2] == null && Board.Squares[0,3] == null) 
            {
                Board.Squares[0,0] = null;
                Board.Squares[0,3] = new Rook("Black");

                if (!ValidateMove((0,4),(0,2), Board))
                {
                    moves.Add((0, 0, false));
                }
                Board.Squares[0,0] = new Rook("Black");
                Board.Squares[0,3] = null;
            }
            if (Board.Squares[0,7] != null && Board.Squares[0,7].Color == "Black" && friendlyColor == "Black" && !BlackRightRookMoved
                    && Board.Squares[0,5] == null && Board.Squares[0,6] == null) 
            {

                Board.Squares[0,7] = null;
                Board.Squares[0,5] = new Rook("Black");
                
                if (!ValidateMove((0,4), (0,6), Board))
                {
                    moves.Add((0, 7, false));
                }

                Board.Squares[0,7] = new Rook("Black");
                Board.Squares[0,5] = null;
            }
        }
        return moves;
    }

    public List<(int x, int y, bool IsEnemy)> GetValidMoves(int x, int y, Board Board, string friendlyColor)
    {
        var friendlyPieces = Board.GetPiecePositionsByColor(friendlyColor);
        var enemyPieces = Board.GetPiecePositionsByColor(friendlyColor == "White" ? "Black" : "White");
        Piece piece = Board.Squares[x, y];

        List<(int x, int y, bool IsEnemy)> moves = piece.GetMoves((x, y), enemyPieces, friendlyPieces);
        var finalMoves = new List<(int x, int y, bool IsEnemy)>();

        foreach (var move in moves)
        {
            if (!ValidateMove((x, y), (move.x, move.y), Board))
            {
                finalMoves.Add(move);
                
            } 
        }
        return finalMoves;
    }
}