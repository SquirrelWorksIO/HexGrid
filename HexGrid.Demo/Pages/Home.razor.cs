using HexGrid.Lib.Models;
using HexGrid.Lib.Models.Coordinates;
using HexGrid.Lib.Models.Layout;

namespace HexGrid.Demo.Pages;

public partial class Home
{
    private string selectedGridType = "rectangular";
    private bool isPointy = true;
    private string orientationValue = "pointy";
    private int gridWidth = 6;
    private int gridHeight = 5;
    private int hexRadius = 3;
    private int triangleSize = 5;
    private int hexSize = 30;
    private bool showCoordinates = true;
    private HexGridModel? currentGrid;

    protected override void OnInitialized()
    {
        GenerateGrid();
    }

    private void OnOrientationChanged()
    {
        isPointy = orientationValue == "pointy";
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        var originX = gridWidth * hexSize / 2.0;
        var originY = gridHeight * hexSize / 2.0;
        if(selectedGridType == "hexagonal")
        {
            originX = hexRadius * 2.5 * hexSize;
            originY = hexRadius * 2.5 * hexSize;
        }
        else if(selectedGridType == "triangular")
        {
            originX = triangleSize * hexSize / 2.0;
            originY = triangleSize * hexSize / 2.0;
        }
        else if(selectedGridType == "rectangular")
        {
            if(isPointy)
            {
                originX = 2 * hexSize * Math.Sqrt(3) / 1.75;
                originY = hexSize*2.0;
            }
            else
            {
                originX = hexSize*2.0;
                originY = 2 * hexSize * Math.Sqrt(3) / 1.75;
            }
        }
        var orientation = isPointy ? LayoutOrientation.Pointy : LayoutOrientation.Flat;
        var layout = new GridLayout(
            orientation,
            new PointD(hexSize, hexSize),
            new FractionalHexCoordinate(originX, originY, 0)
        );

        currentGrid = selectedGridType switch
        {
            "rectangular" => HexGridModel.CreateRectangle(layout, gridWidth, gridHeight),
            "hexagonal" => new HexGridModel(
                layout,
                HexGridGenerator.GenerateHexagonalGrid(layout, hexRadius)
            ),
            "triangular" => new HexGridModel(
                layout,
                HexGridGenerator.GenerateTriangularGrid(layout, triangleSize)
            ),
            "parallelogram" => new HexGridModel(
                layout,
                HexGridGenerator.GenerateParallelogramGrid(layout, gridWidth, gridHeight, ParallelogramOrientation.QR)
            ),
            _ => global::HexGrid.Lib.Models.HexGridModel.CreateRectangle(layout, gridWidth, gridHeight)
        };

        StateHasChanged();
    }

    private int GetHexCount()
    {
        if (currentGrid?.Grid == null) return 0;
        return currentGrid.Grid.Sum(row => row.Length);
    }
}