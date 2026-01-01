namespace HexGrid.Lib.Models.Coordinates;
using System.Drawing;

public class OffsetHexCoordinate(int col, int row)
{
    public int Col { get; } = col;
    public int Row { get; } = row;

    public static OffsetHexCoordinate FromAxial(AxialHexCoordinate axial, OffsetHexCoordinateType type)
    {
        return type switch
        {
            OffsetHexCoordinateType.EvenQ => new OffsetHexCoordinate(
                axial.Q,
                axial.R + (axial.Q + Modulo(axial.Q, 2)) / 2),
            OffsetHexCoordinateType.OddQ => new OffsetHexCoordinate(
                axial.Q,
                axial.R + (axial.Q - Modulo(axial.Q, 2)) / 2),
            OffsetHexCoordinateType.EvenR => new OffsetHexCoordinate(
                axial.Q + (axial.R + Modulo(axial.R, 2)) / 2,
                axial.R),
            OffsetHexCoordinateType.OddR => new OffsetHexCoordinate(
                axial.Q + (axial.R - Modulo(axial.R, 2)) / 2,
                axial.R),
            _ => throw new ArgumentOutOfRangeException(nameof(type), "Invalid OffsetHexCoordinateType.")
        };
    }

    public AxialHexCoordinate ToAxial(OffsetHexCoordinateType type)
    {
        return type switch
        {
            OffsetHexCoordinateType.EvenQ => new AxialHexCoordinate(
                Col,
                Row - (Col + (Col & 1)) / 2),
            OffsetHexCoordinateType.OddQ => new AxialHexCoordinate(
                Col,
                Row - (Col - (Col & 1)) / 2),
            OffsetHexCoordinateType.EvenR => new AxialHexCoordinate(
                Col - (Row + (Row & 1)) / 2,
                Row),
            OffsetHexCoordinateType.OddR => new AxialHexCoordinate(
                Col - (Row - (Row & 1)) / 2,
                Row),
            _ => throw new ArgumentOutOfRangeException(nameof(type), "Invalid OffsetHexCoordinateType.")
        };
    }

    public static OffsetHexCoordinate FromCube(CubHexCoordinate cube, OffsetHexCoordinateType type)
    {
        return FromAxial(cube.ToAxial(), type);
    }

    public CubHexCoordinate ToCube(OffsetHexCoordinateType type)
    {
        return ToAxial(type).ToCube();
    }

    public static AxialHexCoordinate ToAxial(int col, int row, OffsetHexCoordinateType type)
    {
        return new OffsetHexCoordinate(col, row).ToAxial(type);
    }

    public static CubHexCoordinate ToCube(int col, int row, OffsetHexCoordinateType type)
    {
        return ToAxial(col, row, type).ToCube();
    }

    public static Point FromAxial(int q, int r, OffsetHexCoordinateType type)
    {
        var offset = FromAxial(new AxialHexCoordinate(q, r), type);
        return new Point(offset.Col, offset.Row);
    }

    public static Point FromCube(int q, int r, int s, OffsetHexCoordinateType type)
    {
        return FromAxial(q, r, type);
    }

    private static int Modulo(int a, int b)
    {
        return (a % b + b) % b;
    }
}

public enum OffsetHexCoordinateType
{
    EvenQ,
    OddQ,
    EvenR,
    OddR
}