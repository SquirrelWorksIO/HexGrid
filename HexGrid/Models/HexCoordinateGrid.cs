namespace HexGrid.Models;

using Coordinates;
using Layout;

public class HexCoordinateGrid(GridLayout layout, ICollection<AxialHexCoordinate> grid, AxialHexCoordinate? origin = null)
{
    public GridLayout Layout { get; } = layout;
    public ICollection<AxialHexCoordinate> Grid = grid;
    public AxialHexCoordinate Origin { get; } = origin ?? new AxialHexCoordinate(0, 0);

    public AxialHexCoordinate GetNeighbor(AxialHexCoordinate coordinate, int direction)
    {
        if(!Grid.Contains(coordinate))
        {
            throw new ArgumentException("Coordinate is not part of the grid.");
        }
        if(direction < 0 || direction > 5)
        {
            throw new ArgumentOutOfRangeException("Direction must be between 0 and 5.");
        }
        return coordinate.GetNeighbor(direction);
    }

    public ICollection<AxialHexCoordinate> GetNeighbors(AxialHexCoordinate coordinate)
    {
        if(!Grid.Contains(coordinate))
        {
            throw new ArgumentException("Coordinate is not part of the grid.");
        }
        var neighbors = coordinate.Neighbors;
        
        return neighbors.Where(Grid.Contains).ToList();
    }

    public ICollection<AxialHexCoordinate> LineDraw(AxialHexCoordinate a, AxialHexCoordinate b)
    {
        if(!Grid.Contains(a) || !Grid.Contains(b))
        {
            throw new ArgumentException("Both coordinates must be part of the grid.");
        }
        int N = a.DistanceTo(b);
        var results = new List<AxialHexCoordinate>();
        var aFrac = new FractionalHexCoordinate(a.Q + 1e-6, a.R + 1e-6, -a.Q - a.R - 2e-6);
        var bFrac = new FractionalHexCoordinate(b.Q + 1e-6, b.R + 1e-6, -b.Q - b.R - 2e-6);
        for (int i = 0; i <= N; i++)
        {
            var lerped = HexLerp(aFrac, bFrac, 1.0 / Math.Max(N, 1) * i);
            var rounded = lerped.ToAxial();
            if (Grid.Contains(rounded))
            {
                results.Add(rounded);
            }
        }
        return results;
    }

    private double Lerp(double a, double b, double t)
    {
        return a * (1 - t) + b * t;
    }

    private FractionalHexCoordinate HexLerp(FractionalHexCoordinate a, FractionalHexCoordinate b, double t)
    {
        return new FractionalHexCoordinate(
            Lerp(a.Q, b.Q, t),
            Lerp(a.R, b.R, t),
            Lerp(a.S, b.S, t)
        );
    }
}
