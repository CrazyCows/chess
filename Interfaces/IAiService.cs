using chess.Model;

namespace chess.Interfaces;

public interface IAiService
{
    (int fromX, int fromY, int toX, int toY) GetBestMove(string color);
}