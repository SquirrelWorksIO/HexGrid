namespace HexGrid.Models.Coordinates;

public record AxialHexCoordinate(int Q, int R)
{
    private static readonly AxialHexCoordinate[] _directions =
	[
		new AxialHexCoordinate(1, -1),
        new AxialHexCoordinate(1, 0),
        new AxialHexCoordinate(0, 1),
        new AxialHexCoordinate(-1, 1),
        new AxialHexCoordinate(-1, 0),
        new AxialHexCoordinate(0, -1)
    ];


    public int Length => (Math.Abs(Q) + Math.Abs(R) + Math.Abs(-Q - R)) / 2;

    public ICollection<AxialHexCoordinate> Neighbors => GetNeighbors();

    public int DistanceTo(AxialHexCoordinate other)
    {
        return (this - other).Length;
    }

    public CubHexCoordinate ToCube()
    {
        int s = -Q - R;
        return new CubHexCoordinate(Q, R, s);
    }

    public static AxialHexCoordinate FromCube(int q, int r, int s)
    {
        if (q + r + s != 0)
        {
            throw new ArgumentException("In cube coordinates, Q + R + S must equal 0.");
        }
        return new AxialHexCoordinate(q, r);
    }

    public static AxialHexCoordinate FromCube(CubHexCoordinate cube)
    {
        return new AxialHexCoordinate(cube.Q, cube.R);
    }

    public static AxialHexCoordinate operator +(AxialHexCoordinate a, AxialHexCoordinate b)
    {
        return new AxialHexCoordinate(a.Q + b.Q, a.R + b.R);
    }

    public static AxialHexCoordinate operator -(AxialHexCoordinate a, AxialHexCoordinate b)
    {
        return new AxialHexCoordinate(a.Q - b.Q, a.R - b.R);
    }

    public static AxialHexCoordinate operator *(AxialHexCoordinate a, int k)
    {
        return new AxialHexCoordinate(a.Q * k, a.R * k);
    }

    public AxialHexCoordinate RotateRight()
    {
        return new AxialHexCoordinate(-R, Q + R);
    }

    public AxialHexCoordinate RotateLeft()
    {
        return new AxialHexCoordinate(Q + R, -Q);
    }

    public AxialHexCoordinate GetNeighbor(int direction)
    {
        if (direction < 0 || direction > 5)
        {
            throw new ArgumentOutOfRangeException("Direction must be between 0 and 5.");
        }
        return this + _directions[direction];
    }

    private ICollection<AxialHexCoordinate> GetNeighbors()
	{
        return new List<AxialHexCoordinate>
        {
            this + _directions[0],
            this + _directions[1],
            this + _directions[2],
            this + _directions[3],
            this + _directions[4],
            this + _directions[5]
        };
	}
}