namespace HexGrid.Tests.Models;

using HexGrid.Lib.Models;
using HexGrid.Lib.Models.Coordinates;
using HexGrid.Lib.Models.Layout;

[TestFixture]
public class HexGridTests
{
    [Test]
    public void ConstructorWithOriginShouldSetProperties()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = new AxialHexCoordinate[1][];
        grid[0] = [new AxialHexCoordinate(0, 0)];
        var origin = new AxialHexCoordinate(5, 5);

        var hexGrid = new HexCoordinateGrid(layout, grid, origin);

        Assert.That(hexGrid.Layout, Is.EqualTo(layout));
        Assert.That(hexGrid.Grid, Is.EqualTo(grid));
        Assert.That(hexGrid.Origin, Is.EqualTo(origin));
    }

    [Test]
    public void ConstructorWithoutOriginShouldDefaultToZeroZero()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = new AxialHexCoordinate[1][];
        grid[0] = [new AxialHexCoordinate(0, 0)];

        var hexGrid = new HexCoordinateGrid(layout, grid);

        Assert.That(hexGrid.Origin, Is.EqualTo(new AxialHexCoordinate(0, 0)));
    }

    [Test]
    public void CreateRectangleShouldGenerateCorrectGrid()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var width = 3;
        var height = 2;

        var hexGrid = HexCoordinateGrid.CreateRectangle(layout, width, height);

        Assert.That(hexGrid.Grid.Length, Is.EqualTo(height));
        Assert.That(hexGrid.Grid[0].Length, Is.EqualTo(width));
        Assert.That(hexGrid.Origin, Is.EqualTo(new AxialHexCoordinate(0, 0)));
        Assert.That(hexGrid.Grid[0][0], Is.EqualTo(new AxialHexCoordinate(0, 0)));
        Assert.That(hexGrid.Grid[1][2], Is.EqualTo(new AxialHexCoordinate(2, 1)));
    }

    [Test]
    public void CreateRectangleWithOriginShouldOffsetGrid()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var width = 2;
        var height = 2;
        var origin = new AxialHexCoordinate(10, 20);

        var hexGrid = HexCoordinateGrid.CreateRectangle(layout, width, height, origin);

        Assert.That(hexGrid.Origin, Is.EqualTo(origin));
        Assert.That(hexGrid.Grid[0][0], Is.EqualTo(new AxialHexCoordinate(10, 20)));
        Assert.That(hexGrid.Grid[0][1], Is.EqualTo(new AxialHexCoordinate(11, 20)));
        Assert.That(hexGrid.Grid[1][0], Is.EqualTo(new AxialHexCoordinate(10, 21)));
        Assert.That(hexGrid.Grid[1][1], Is.EqualTo(new AxialHexCoordinate(11, 21)));
    }

    [Test]
    public void CreateRectangleWithNullOriginShouldDefaultToZeroZero()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));

        var hexGrid = HexCoordinateGrid.CreateRectangle(layout, 2, 2, null);

        Assert.That(hexGrid.Origin, Is.EqualTo(new AxialHexCoordinate(0, 0)));
    }

    [Test]
    public void CreateRectangleShouldWorkWithFlatOrientation()
    {
        var layout = new GridLayout(LayoutOrientation.Flat, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var width = 2;
        var height = 2;

        var hexGrid = HexCoordinateGrid.CreateRectangle(layout, width, height);

        Assert.That(hexGrid.Layout, Is.EqualTo(layout));
        Assert.That(hexGrid.Grid.Length, Is.EqualTo(height));
        Assert.That(hexGrid.Grid[0].Length, Is.EqualTo(width));
    }

    [Test]
    public void CreateRectangleWithSingleCellShouldWork()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));

        var hexGrid = HexCoordinateGrid.CreateRectangle(layout, 1, 1);

        Assert.That(hexGrid.Grid.Length, Is.EqualTo(1));
        Assert.That(hexGrid.Grid[0].Length, Is.EqualTo(1));
        Assert.That(hexGrid.Grid[0][0], Is.EqualTo(new AxialHexCoordinate(0, 0)));
    }
}
