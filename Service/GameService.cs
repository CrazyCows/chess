using chess.Models;

public class GameService
{
    public string CurrentTurn { get; set; } = "White";
    public Board Board { get; private set; } = new Board();

    public List<(int x, int y, bool IsEnemy)>? GetValidMoves(int x, int y) {
        return Board.GetMoves(x, y, CurrentTurn);
    }

    public void MakeMove(int fromColumn, int fromRow, int toColumn, int toRow) {
        Board.MovePiece(fromColumn, fromRow, toColumn, toRow);
    }

    public bool IsCheckMate() {
        return Board.CheckMate(CurrentTurn);
    }

    public List<(int, int, bool)>? IsCheck() {
        return Board.Check(CurrentTurn);
    }
}
