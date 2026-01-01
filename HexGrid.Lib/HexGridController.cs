namespace HexGrid.Lib.Models;

using Coordinates;
using Layout;

public class HexCoordinateGrid(GridLayout layout, AxialHexCoordinate[][] grid, AxialHexCoordinate? origin = null)
{
    public GridLayout Layout { get; } = layout;
    public AxialHexCoordinate[][] Grid = grid;
    public AxialHexCoordinate Origin { get; } = origin ?? new AxialHexCoordinate(0, 0);
    public HexCoordinateGrid(GridLayout layout, AxialHexCoordinate[][] grid)
        : this(layout, grid, new AxialHexCoordinate(0, 0))
    {
    }

    public static HexCoordinateGrid CreateRectangle(GridLayout layout, int width, int height, AxialHexCoordinate? origin = null)
    {
        var originCoord = origin ?? new AxialHexCoordinate(0, 0);
        var grid = HexGridGenerator.GenerateRectangularGrid(layout, width, height, null, originCoord);
        return new HexCoordinateGrid(layout, grid, originCoord);
    }

}