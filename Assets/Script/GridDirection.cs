using System.Collections.Generic;
using UnityEngine;

public class GridDirection
{
    public readonly Vector2 Vector;

    private GridDirection(int x, int y)
    {
        Vector = new Vector2(x, y);
    }

    public static implicit operator Vector2(GridDirection direction)
    {
        return direction.Vector;
    }

    // Optimized GetDirectionFromV2I method without using FirstOrDefault
    public static GridDirection GetDirectionFromV2I(Vector2 vector)
    {
        // Check if the vector exists in the optimized dictionary
        return DirectionsDictionary.TryGetValue(vector, out var direction) ? direction : None;
    }

    public static readonly GridDirection None = new GridDirection(0, 0);
    public static readonly GridDirection North = new GridDirection(0, 1);
    public static readonly GridDirection South = new GridDirection(0, -1);
    public static readonly GridDirection East = new GridDirection(1, 0);
    public static readonly GridDirection West = new GridDirection(-1, 0);
    public static readonly GridDirection NorthEast = new GridDirection(1, 1);
    public static readonly GridDirection NorthWest = new GridDirection(-1, 1);
    public static readonly GridDirection SouthEast = new GridDirection(1, -1);
    public static readonly GridDirection SouthWest = new GridDirection(-1, -1);

    // Existing lists for compatibility
    public static readonly List<GridDirection> CardinalDirections = new List<GridDirection>
    {
        North,
        East,
        South,
        West
    };

    public static readonly List<GridDirection> CardinalAndIntercardinalDirections = new List<GridDirection>
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    };

    public static readonly List<GridDirection> AllDirections = new List<GridDirection>
    {
        None,
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    };

    // Internal dictionary for fast look-up
    private static readonly Dictionary<Vector2, GridDirection> DirectionsDictionary = new Dictionary<Vector2, GridDirection>
    {
        { North.Vector, North },
        { NorthEast.Vector, NorthEast },
        { East.Vector, East },
        { SouthEast.Vector, SouthEast },
        { South.Vector, South },
        { SouthWest.Vector, SouthWest },
        { West.Vector, West },
        { NorthWest.Vector, NorthWest }
    };
}
