namespace HexGrid.Models.Coordinates;

public record AxialHexCoordinate(int Q, int R)
{
    public int Length => (Math.Abs(Q) + Math.Abs(R) + Math.Abs(-Q - R)) / 2;

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
}