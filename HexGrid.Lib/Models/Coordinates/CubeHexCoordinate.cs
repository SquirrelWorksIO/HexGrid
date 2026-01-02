namespace HexGrid.Lib.Models.Coordinates;

public record CubHexCoordinate
{
    public int Q {get; set;}
    public int R {get; set; }
    public int S {get; set; }

    public CubHexCoordinate(int q, int r, int s)
    {
        if (q + r + s != 0)
        {
            throw new ArgumentException("In cube coordinates, Q + R + S must equal 0.");
        }

        Q = q;
        R = r;
        S = s;
    }

    public int Length => (Math.Abs(Q) + Math.Abs(R) + Math.Abs(S)) / 2;

    public int DistanceTo(CubHexCoordinate other)
    {
        return (this - other).Length;
    }

    public static CubHexCoordinate FromAxial(int q, int r)
    {
        int s = -q - r;
        return new CubHexCoordinate(q, r, s);
    }

    public static CubHexCoordinate FromAxial(AxialHexCoordinate axial)
    {
        return FromAxial(axial.Q, axial.R);
    }

    public AxialHexCoordinate ToAxial()
    {
        return new AxialHexCoordinate(Q, R);
    }

    public static CubHexCoordinate operator +(CubHexCoordinate a, CubHexCoordinate b)
    {
        return new CubHexCoordinate(a.Q + b.Q, a.R + b.R, a.S + b.S);
    }

    public static CubHexCoordinate operator -(CubHexCoordinate a, CubHexCoordinate b)
    {
        return new CubHexCoordinate(a.Q - b.Q, a.R - b.R, a.S - b.S);
    }

    public static CubHexCoordinate operator *(CubHexCoordinate a, int k)
    {
        return new CubHexCoordinate(a.Q * k, a.R * k, a.S * k);
    }
}