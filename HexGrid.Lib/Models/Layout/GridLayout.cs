namespace HexGrid.Lib.Models.Layout;
using Coordinates;

public record GridLayout(LayoutOrientation Orientation, PointD Size, FractionalHexCoordinate PixelOrigin)
{
    public PointD HexToPixel(AxialHexCoordinate hex)
    {
        double x = (Orientation.F[0] * hex.Q + Orientation.F[1] * hex.R) * Size.X;
        double y = (Orientation.F[2] * hex.Q + Orientation.F[3] * hex.R) * Size.Y;
        return new PointD(x + PixelOrigin.Q, y + PixelOrigin.R);
    }

    public FractionalHexCoordinate PixelToHex(PointD p)
    {
        PointD pt = new PointD((p.X - PixelOrigin.Q) / Size.X, (p.Y - PixelOrigin.R) / Size.Y);
        double q = Orientation.B[0] * pt.X + Orientation.B[1] * pt.Y;
        double r = Orientation.B[2] * pt.X + Orientation.B[3] * pt.Y;
        return new FractionalHexCoordinate(q, r, -q - r);
    }

    public PointD HexCornerOffset(int corner)
    {
        double angle = 2.0 * Math.PI * (Orientation.StartAngle + corner) / 6.0;
        return new PointD(Size.X * Math.Cos(angle), Size.Y * Math.Sin(angle));
    }

    public ICollection<PointD> PolygonCorners(AxialHexCoordinate hex)
    {
        var corners = new List<PointD>();
        PointD center = HexToPixel(hex);
        for (var i = 0; i < 6; i++)
        {
            PointD offset = HexCornerOffset(i);
            corners.Add(new PointD(center.X + offset.X, center.Y + offset.Y));
        }
        return corners;
    }
}