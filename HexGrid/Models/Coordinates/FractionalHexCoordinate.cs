namespace HexGrid.Models.Coordinates;

public record FractionalHexCoordinate(double Q, double R, double S)
{
    public static FractionalHexCoordinate FromAxial(int q, int r)
    {
        double s = -q - r;
        return new FractionalHexCoordinate(q, r, s);
    }

    public AxialHexCoordinate ToAxial()
    {
        return ToCube().ToAxial();
    }

    public static FractionalHexCoordinate FromCube(int q, int r, int s)
    {
        if (Math.Abs(q + r + s) > 1e-6)
        {
            throw new ArgumentException("In cube coordinates, Q + R + S must equal 0.");
        }
        return new FractionalHexCoordinate(q, r, s);
    }

    public CubHexCoordinate ToCube()
    {
        return RoundToCube();
    }

    private CubHexCoordinate RoundToCube()
    {
        int rq = (int)Math.Round(Q);
        int rr = (int)Math.Round(R);
        int rs = (int)Math.Round(S);

        double qDiff = Math.Abs(rq - Q);
        double rDiff = Math.Abs(rr - R);
        double sDiff = Math.Abs(rs - S);

        if (qDiff > rDiff && qDiff > sDiff)
        {
            rq = -rr - rs;
        }
        else if (rDiff > sDiff)
        {
            rr = -rq - rs;
        }
        else
        {
            rs = -rq - rr;
        }

        return new CubHexCoordinate(rq, rr, rs);
    }
}