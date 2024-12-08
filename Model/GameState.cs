using chess.Validators;

namespace chess.Model;

public class GameState
{
    public event Action<bool>? IsCheck;
    public event Action<bool>? IsCheckMate;
    public bool Check { get; set; }
    public Dictionary<PlayerColor, Dictionary<PieceType, bool>> MovementStates { get; set; } 
        
    public string CurrentTurn { get; set; } = "White";
    public CheckValidator CheckValidator { get; set; }

    public enum PieceType
    {
        King,
        LeftRook,
        RightRook
    }

    public enum PlayerColor
    {
        White,
        Black
    }
    
    public GameState()
    {
        CheckValidator = new CheckValidator();
        MovementStates = new Dictionary<PlayerColor, Dictionary<PieceType, bool>>()
        {
            { PlayerColor.White, new Dictionary<PieceType, bool>
                {
                    { PieceType.King, false },
                    { PieceType.LeftRook, false },
                    { PieceType.RightRook, false }
                }
            },
            { PlayerColor.Black, new Dictionary<PieceType, bool>
                {
                    { PieceType.King, false },
                    { PieceType.LeftRook, false },
                    { PieceType.RightRook, false }
                }
            }
        };
        
    }

    public void KingOrRookMoved(int fromColumn, int fromRow)
    {
        var positionToState = new Dictionary<(int column, int row), (PlayerColor color, PieceType type)>
        {
            { (0, 0), (PlayerColor.Black, PieceType.LeftRook) },
            { (0, 7), (PlayerColor.Black, PieceType.RightRook) },
            { (7, 0), (PlayerColor.White, PieceType.LeftRook) },
            { (7, 7), (PlayerColor.White, PieceType.RightRook) },
            { (0, 4), (PlayerColor.Black, PieceType.King) },
            { (7, 4), (PlayerColor.White, PieceType.King) }
        };

        if (positionToState.TryGetValue((fromColumn, fromRow), out var movementKey))
        {
            MovementStates[movementKey.color][movementKey.type] = true;
        }
    }

    
    public void CheckTurn(Board board)
    {
        if (CheckValidator.Check(board, CurrentTurn))
        {
            IsCheck?.Invoke(true);
            Check = true;
            return;
        }
        IsCheck?.Invoke(false);
        Check = false;
    }

    public void CheckMateTurn(List<(int x, int y, bool IsEnemy)> allMoves)
    {
        if (CheckValidator.Checkmate(allMoves))
        {
            IsCheckMate?.Invoke(true);
            return;
        }
        IsCheckMate?.Invoke(false);
    }
    
    public GameState DeepCopy()
    {
        var newGameState = new GameState
        {
            Check = this.Check,
            CurrentTurn = this.CurrentTurn,
            MovementStates = new Dictionary<PlayerColor, Dictionary<PieceType, bool>>()
        };

        foreach (var playerColor in MovementStates.Keys)
        {
            newGameState.MovementStates[playerColor] = new Dictionary<PieceType, bool>();
            foreach (var pieceType in MovementStates[playerColor].Keys)
            {
                newGameState.MovementStates[playerColor][pieceType] = MovementStates[playerColor][pieceType];
            }
        }

        return newGameState;
    }
    
}