namespace HexGrid.Lib.Models;

using Coordinates;
using Layout;

public class HexGrid(GridLayout layout, AxialHexCoordinate[][] grid, AxialHexCoordinate? origin = null)
{
    public GridLayout Layout { get; } = layout;
    public AxialHexCoordinate[][] Grid = grid;
    public AxialHexCoordinate Origin { get; } = origin ?? new AxialHexCoordinate(0, 0);
    public HexGrid(GridLayout layout, AxialHexCoordinate[][] grid)
        : this(layout, grid, new AxialHexCoordinate(0, 0))
    {
    }

    public static HexGrid CreateRectangle(GridLayout layout, int width, int height, AxialHexCoordinate? origin = null)
    {
        var originCoord = origin ?? new AxialHexCoordinate(0, 0);
        var grid = HexGridGenerator.GenerateRectangularGrid(layout, width, height, originCoord);
        return new HexGrid(layout, grid, originCoord);
    }

}