namespace HexGrid.Lib.Models.Layout;

public record LayoutOrientation
{
    public double[] F { get; init; }
    public double[] B { get; init; }
    public double StartAngle { get; init; }
    public bool IsPointy => Math.Abs(StartAngle - 0.5) < 1e-6;

    private LayoutOrientation(double[] f, double[] b, double startAngle)
    {
        F = f;
        B = b;
        StartAngle = startAngle;
    }

    public static LayoutOrientation Pointy => new LayoutOrientation(
        new double[] { Math.Sqrt(3.0), Math.Sqrt(3.0) / 2.0, 0.0, 3.0 / 2.0 },
        new double[] { Math.Sqrt(3.0) / 3.0, -1.0 / 3.0, 0.0, 2.0 / 3.0 },
        0.5);

    public static LayoutOrientation Flat => new LayoutOrientation(
        new double[] { 3.0 / 2.0, 0.0, Math.Sqrt(3.0) / 2.0, Math.Sqrt(3.0) },
        new double[] { 2.0 / 3.0, 0.0, -1.0 / 3.0, Math.Sqrt(3.0) / 3.0 },
        0.0);
}