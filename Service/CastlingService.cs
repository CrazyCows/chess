﻿using chess.Interfaces;
using chess.Model;

namespace chess.Service;

public class CastlingService : ICastlingService
{
    private static readonly Dictionary<(int column, int row), (PlayerColor color, PieceType type)>
        PositionToPieceMapping = new()
        {
            { (0, 0), (PlayerColor.Black, PieceType.LeftRook) },
            { (0, 7), (PlayerColor.Black, PieceType.RightRook) },
            { (7, 0), (PlayerColor.White, PieceType.LeftRook) },
            { (7, 7), (PlayerColor.White, PieceType.RightRook) },
            { (0, 4), (PlayerColor.Black, PieceType.King) },
            { (7, 4), (PlayerColor.White, PieceType.King) }
        };

    public CastlingService()
    {
        MovementStates = InitializeMovementStates();
    }

    public Dictionary<PlayerColor, Dictionary<PieceType, bool>> MovementStates { get; }

    private Dictionary<PlayerColor, Dictionary<PieceType, bool>> InitializeMovementStates()
    {
        return new Dictionary<PlayerColor, Dictionary<PieceType, bool>>
        {
            {
                PlayerColor.White, new Dictionary<PieceType, bool>
                {
                    { PieceType.King, false },
                    { PieceType.LeftRook, false },
                    { PieceType.RightRook, false }
                }
            },
            {
                PlayerColor.Black, new Dictionary<PieceType, bool>
                {
                    { PieceType.King, false },
                    { PieceType.LeftRook, false },
                    { PieceType.RightRook, false }
                }
            }
        };
    }

    public void MarkPieceMoved(int column, int row)
    {
        if (PositionToPieceMapping.TryGetValue((column, row), out var pieceInfo))
            MovementStates[pieceInfo.color][pieceInfo.type] = true;
    }

    public void ResetMovementStates()
    {
        var newStates = InitializeMovementStates();
        MovementStates.Clear();
        foreach (var kvp in newStates)
        {
            MovementStates[kvp.Key] = kvp.Value;
        }
    }


    public ICastlingService DeepCopy()
    {
        var copy = new CastlingService();
        foreach (var playerColor in MovementStates.Keys)
        foreach (var pieceType in MovementStates[playerColor].Keys)
            copy.MovementStates[playerColor][pieceType] = MovementStates[playerColor][pieceType];

        return copy;
    }
}