using chess.Helper;
using chess.Model;
using chess.Validators;

namespace chess.Interfaces;

public interface IAiService
{
    (int fromX, int fromY, int toX, int toY) GetBestMove(Board board, GameState gameState, CastlingState castlingState, string color);
}

