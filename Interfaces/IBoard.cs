using System.Collections.Generic;
using chess.Model;

namespace chess.Interfaces
{
    public interface IBoard
    {
        
        void InitializeBoard();
        void MovePiece(int fromRow, int fromColumn, int toRow, int toColumn);
        List<(int x, int y)> GetPiecePositionsByColor(string color);
        (int x, int y)? GetKingPos(string color);
        Board DeepCopy();
    }
}