namespace HexGrid.Lib.Models.Layout;

public struct PointD
{
    public double X { get; set; }
    public double Y { get; set; }

    public PointD(double x, double y)
    {
        X = x;
        Y = y;
    }
}

public struct PointI
{
    public int X { get; set; }
    public int Y { get; set; }

    public PointI(int x, int y)
    {
        X = x;
        Y = y;
    }
}