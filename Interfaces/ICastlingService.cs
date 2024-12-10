
using chess.Model;

namespace chess.Interfaces
{
    public interface ICastlingService
    {
        Dictionary<PlayerColor, Dictionary<PieceType, bool>> MovementStates { get; }

        void MarkPieceMoved(int column, int row);
        void ResetMovementStates();

        ICastlingService DeepCopy();
    }
}
