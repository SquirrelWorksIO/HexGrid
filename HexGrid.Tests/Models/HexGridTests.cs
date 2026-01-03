namespace HexGrid.Tests.Models;

using HexGrid.Models;
using HexGrid.Models.Coordinates;
using HexGrid.Models.Layout;

[TestFixture]
public class HexGridTests
{
    [Test]
    public void ConstructorWithOriginShouldSetProperties()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = new List<AxialHexCoordinate> { new AxialHexCoordinate(0, 0) };
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
        var grid = new List<AxialHexCoordinate> { new AxialHexCoordinate(0, 0) };

        var hexGrid = new HexCoordinateGrid(layout, grid);

        Assert.That(hexGrid.Origin, Is.EqualTo(new AxialHexCoordinate(0, 0)));
    }

    #region GetNeighbor Tests

    [Test]
    public void GetNeighborShouldReturnCorrectNeighbor()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = HexGridGenerator.GenerateHexagonalGrid(layout, 1);
        var hexGrid = new HexCoordinateGrid(layout, grid);
        var center = new AxialHexCoordinate(0, 0);

        var neighbor = hexGrid.GetNeighbor(center, 0);

        Assert.That(neighbor, Is.EqualTo(new AxialHexCoordinate(1, -1)));
    }

    [Test]
    public void GetNeighborShouldReturnAllDirections()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = HexGridGenerator.GenerateHexagonalGrid(layout, 1);
        var hexGrid = new HexCoordinateGrid(layout, grid);
        var center = new AxialHexCoordinate(0, 0);

        var neighbor0 = hexGrid.GetNeighbor(center, 0);
        var neighbor1 = hexGrid.GetNeighbor(center, 1);
        var neighbor2 = hexGrid.GetNeighbor(center, 2);
        var neighbor3 = hexGrid.GetNeighbor(center, 3);
        var neighbor4 = hexGrid.GetNeighbor(center, 4);
        var neighbor5 = hexGrid.GetNeighbor(center, 5);

        Assert.That(neighbor0, Is.EqualTo(new AxialHexCoordinate(1, -1)));
        Assert.That(neighbor1, Is.EqualTo(new AxialHexCoordinate(1, 0)));
        Assert.That(neighbor2, Is.EqualTo(new AxialHexCoordinate(0, 1)));
        Assert.That(neighbor3, Is.EqualTo(new AxialHexCoordinate(-1, 1)));
        Assert.That(neighbor4, Is.EqualTo(new AxialHexCoordinate(-1, 0)));
        Assert.That(neighbor5, Is.EqualTo(new AxialHexCoordinate(0, -1)));
    }

    [Test]
    public void GetNeighborWithInvalidCoordinateShouldThrow()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = new List<AxialHexCoordinate> { new AxialHexCoordinate(0, 0) };
        var hexGrid = new HexCoordinateGrid(layout, grid);
        var invalidCoord = new AxialHexCoordinate(10, 10);

        Assert.Throws<ArgumentException>(() => hexGrid.GetNeighbor(invalidCoord, 0));
    }

    [Test]
    public void GetNeighborWithInvalidDirectionShouldThrow()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = new List<AxialHexCoordinate> { new AxialHexCoordinate(0, 0) };
        var hexGrid = new HexCoordinateGrid(layout, grid);
        var center = new AxialHexCoordinate(0, 0);

        Assert.Throws<ArgumentOutOfRangeException>(() => hexGrid.GetNeighbor(center, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => hexGrid.GetNeighbor(center, 6));
    }

    #endregion

    #region GetNeighbors Tests

    [Test]
    public void GetNeighborsShouldReturnAllValidNeighbors()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = HexGridGenerator.GenerateHexagonalGrid(layout, 1);
        var hexGrid = new HexCoordinateGrid(layout, grid);
        var center = new AxialHexCoordinate(0, 0);

        var neighbors = hexGrid.GetNeighbors(center);

        Assert.That(neighbors.Count, Is.EqualTo(6));
        Assert.That(neighbors, Does.Contain(new AxialHexCoordinate(1, -1)));
        Assert.That(neighbors, Does.Contain(new AxialHexCoordinate(1, 0)));
        Assert.That(neighbors, Does.Contain(new AxialHexCoordinate(0, 1)));
        Assert.That(neighbors, Does.Contain(new AxialHexCoordinate(-1, 1)));
        Assert.That(neighbors, Does.Contain(new AxialHexCoordinate(-1, 0)));
        Assert.That(neighbors, Does.Contain(new AxialHexCoordinate(0, -1)));
    }

    [Test]
    public void GetNeighborsShouldReturnOnlyNeighborsInGrid()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = new List<AxialHexCoordinate>
        {
            new AxialHexCoordinate(0, 0),
            new AxialHexCoordinate(1, 0),
            new AxialHexCoordinate(2, 0)
        };
        var hexGrid = new HexCoordinateGrid(layout, grid);
        var coord = new AxialHexCoordinate(1, 0);

        var neighbors = hexGrid.GetNeighbors(coord);

        Assert.That(neighbors.Count, Is.EqualTo(2));
        Assert.That(neighbors, Does.Contain(new AxialHexCoordinate(0, 0)));
        Assert.That(neighbors, Does.Contain(new AxialHexCoordinate(2, 0)));
    }

    [Test]
    public void GetNeighborsAtEdgeShouldReturnLimitedNeighbors()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = HexGridGenerator.GenerateHexagonalGrid(layout, 1);
        var hexGrid = new HexCoordinateGrid(layout, grid);
        var edgeCoord = new AxialHexCoordinate(1, 0);

        var neighbors = hexGrid.GetNeighbors(edgeCoord);

        // Edge hex in radius 1 hexagon should have 3 neighbors in the grid
        Assert.That(neighbors.Count, Is.EqualTo(3));
    }

    [Test]
    public void GetNeighborsWithInvalidCoordinateShouldThrow()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = new List<AxialHexCoordinate> { new AxialHexCoordinate(0, 0) };
        var hexGrid = new HexCoordinateGrid(layout, grid);
        var invalidCoord = new AxialHexCoordinate(10, 10);

        Assert.Throws<ArgumentException>(() => hexGrid.GetNeighbors(invalidCoord));
    }

    [Test]
    public void GetNeighborsForIsolatedHexShouldReturnEmpty()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = new List<AxialHexCoordinate> { new AxialHexCoordinate(0, 0) };
        var hexGrid = new HexCoordinateGrid(layout, grid);
        var center = new AxialHexCoordinate(0, 0);

        var neighbors = hexGrid.GetNeighbors(center);

        Assert.That(neighbors, Is.Empty);
    }

    #endregion

    #region LineDraw Tests

    [Test]
    public void LineDrawShouldReturnStraightLine()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = HexGridGenerator.GenerateRectangularGrid(layout, 5, 1);
        var hexGrid = new HexCoordinateGrid(layout, grid);
        var start = new AxialHexCoordinate(0, 0);
        var end = new AxialHexCoordinate(4, 0);

        var line = hexGrid.LineDraw(start, end);

        Assert.That(line.Count, Is.EqualTo(5));
        Assert.That(line, Does.Contain(new AxialHexCoordinate(0, 0)));
        Assert.That(line, Does.Contain(new AxialHexCoordinate(1, 0)));
        Assert.That(line, Does.Contain(new AxialHexCoordinate(2, 0)));
        Assert.That(line, Does.Contain(new AxialHexCoordinate(3, 0)));
        Assert.That(line, Does.Contain(new AxialHexCoordinate(4, 0)));
    }

    [Test]
    public void LineDrawWithSameStartAndEndShouldReturnSingleHex()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = HexGridGenerator.GenerateHexagonalGrid(layout, 1);
        var hexGrid = new HexCoordinateGrid(layout, grid);
        var coord = new AxialHexCoordinate(0, 0);

        var line = hexGrid.LineDraw(coord, coord);

        Assert.That(line.Count, Is.EqualTo(1));
        Assert.That(line.First(), Is.EqualTo(coord));
    }

    [Test]
    public void LineDrawShouldReturnDiagonalLine()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = HexGridGenerator.GenerateHexagonalGrid(layout, 2);
        var hexGrid = new HexCoordinateGrid(layout, grid);
        var start = new AxialHexCoordinate(-2, 0);
        var end = new AxialHexCoordinate(0, -2);

        var line = hexGrid.LineDraw(start, end);

        Assert.That(line.Count, Is.GreaterThan(0));
        Assert.That(line.First(), Is.EqualTo(start));
        Assert.That(line.Last(), Is.EqualTo(end));
    }

    [Test]
    public void LineDrawWithInvalidStartCoordinateShouldThrow()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = new List<AxialHexCoordinate> { new AxialHexCoordinate(0, 0) };
        var hexGrid = new HexCoordinateGrid(layout, grid);
        var invalidStart = new AxialHexCoordinate(10, 10);
        var validEnd = new AxialHexCoordinate(0, 0);

        Assert.Throws<ArgumentException>(() => hexGrid.LineDraw(invalidStart, validEnd));
    }

    [Test]
    public void LineDrawWithInvalidEndCoordinateShouldThrow()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = new List<AxialHexCoordinate> { new AxialHexCoordinate(0, 0) };
        var hexGrid = new HexCoordinateGrid(layout, grid);
        var validStart = new AxialHexCoordinate(0, 0);
        var invalidEnd = new AxialHexCoordinate(10, 10);

        Assert.Throws<ArgumentException>(() => hexGrid.LineDraw(validStart, invalidEnd));
    }

    [Test]
    public void LineDrawShouldOnlyReturnHexesInGrid()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = HexGridGenerator.GenerateHexagonalGrid(layout, 2);
        var hexGrid = new HexCoordinateGrid(layout, grid);
        var start = new AxialHexCoordinate(-2, 0);
        var end = new AxialHexCoordinate(2, 0);

        var line = hexGrid.LineDraw(start, end);

        // All returned hexes should be in the grid
        foreach (var hex in line)
        {
            Assert.That(grid.Contains(hex), Is.True);
        }
    }

    [Test]
    public void LineDrawWithAdjacentHexesShouldReturnBoth()
    {
        var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        var grid = HexGridGenerator.GenerateHexagonalGrid(layout, 1);
        var hexGrid = new HexCoordinateGrid(layout, grid);
        var start = new AxialHexCoordinate(0, 0);
        var end = new AxialHexCoordinate(1, 0);

        var line = hexGrid.LineDraw(start, end);

        Assert.That(line.Count, Is.EqualTo(2));
        Assert.That(line, Does.Contain(start));
        Assert.That(line, Does.Contain(end));
    }

    #endregion

    // [Test]
    // public void CreateRectangleShouldGenerateCorrectGrid()
    // {
    //     var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
    //     var width = 3;
    //     var height = 2;

    //     var hexGrid = HexCoordinateGrid.CreateRectangle(layout, width, height);

    //     Assert.That(hexGrid.Grid.Length, Is.EqualTo(height));
    //     Assert.That(hexGrid.Grid[0].Length, Is.EqualTo(width));
    //     Assert.That(hexGrid.Origin, Is.EqualTo(new AxialHexCoordinate(0, 0)));
    //     Assert.That(hexGrid.Grid[0][0], Is.EqualTo(new AxialHexCoordinate(0, 0)));
    //     Assert.That(hexGrid.Grid[1][2], Is.EqualTo(new AxialHexCoordinate(2, 1)));
    // }

    // [Test]
    // public void CreateRectangleWithOriginShouldOffsetGrid()
    // {
    //     var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
    //     var width = 2;
    //     var height = 2;
    //     var origin = new AxialHexCoordinate(10, 20);

    //     var hexGrid = HexCoordinateGrid.CreateRectangle(layout, width, height, origin);

    //     Assert.That(hexGrid.Origin, Is.EqualTo(origin));
    //     Assert.That(hexGrid.Grid[0][0], Is.EqualTo(new AxialHexCoordinate(10, 20)));
    //     Assert.That(hexGrid.Grid[0][1], Is.EqualTo(new AxialHexCoordinate(11, 20)));
    //     Assert.That(hexGrid.Grid[1][0], Is.EqualTo(new AxialHexCoordinate(10, 21)));
    //     Assert.That(hexGrid.Grid[1][1], Is.EqualTo(new AxialHexCoordinate(11, 21)));
    // }

    // [Test]
    // public void CreateRectangleWithNullOriginShouldDefaultToZeroZero()
    // {
    //     var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));

    //     var hexGrid = HexCoordinateGrid.CreateRectangle(layout, 2, 2, null);

    //     Assert.That(hexGrid.Origin, Is.EqualTo(new AxialHexCoordinate(0, 0)));
    // }

    // [Test]
    // public void CreateRectangleShouldWorkWithFlatOrientation()
    // {
    //     var layout = new GridLayout(LayoutOrientation.Flat, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
    //     var width = 2;
    //     var height = 2;

    //     var hexGrid = HexCoordinateGrid.CreateRectangle(layout, width, height);

    //     Assert.That(hexGrid.Layout, Is.EqualTo(layout));
    //     Assert.That(hexGrid.Grid.Length, Is.EqualTo(height));
    //     Assert.That(hexGrid.Grid[0].Length, Is.EqualTo(width));
    // }

    // [Test]
    // public void CreateRectangleWithSingleCellShouldWork()
    // {
    //     var layout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));

    //     var hexGrid = HexCoordinateGrid.CreateRectangle(layout, 1, 1);

    //     Assert.That(hexGrid.Grid.Length, Is.EqualTo(1));
    //     Assert.That(hexGrid.Grid[0].Length, Is.EqualTo(1));
    //     Assert.That(hexGrid.Grid[0][0], Is.EqualTo(new AxialHexCoordinate(0, 0)));
    // }
}
