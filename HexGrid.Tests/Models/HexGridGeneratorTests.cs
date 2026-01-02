namespace HexGrid.Tests.Models;

using HexGrid.Lib.Models;
using HexGrid.Lib.Models.Coordinates;
using HexGrid.Lib.Models.Layout;

[TestFixture]
public class HexGridGeneratorTests
{
    private GridLayout _pointyLayout;
    private GridLayout _flatLayout;

    [SetUp]
    public void Setup()
    {
        _pointyLayout = new GridLayout(LayoutOrientation.Pointy, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
        _flatLayout = new GridLayout(LayoutOrientation.Flat, new PointD(10, 10), new FractionalHexCoordinate(0, 0, 0));
    }

    #region GenerateRectangularGrid Tests

    [Test]
    public void GenerateRectangularGridShouldCreateCorrectDimensions()
    {
        var grid = HexGridGenerator.GenerateRectangularGrid(_pointyLayout, 3, 2);

        Assert.That(grid.Length, Is.EqualTo(2));
        Assert.That(grid[0].Length, Is.EqualTo(3));
        Assert.That(grid[1].Length, Is.EqualTo(3));
    }

    [Test]
    public void GenerateRectangularGridShouldStartAtOrigin()
    {
        var grid = HexGridGenerator.GenerateRectangularGrid(_pointyLayout, 2, 2);

        Assert.That(grid[0][0], Is.EqualTo(new AxialHexCoordinate(0, 0)));
        Assert.That(grid[0][1], Is.EqualTo(new AxialHexCoordinate(1, 0)));
        Assert.That(grid[1][0], Is.EqualTo(new AxialHexCoordinate(0, 1)));
        Assert.That(grid[1][1], Is.EqualTo(new AxialHexCoordinate(1, 1)));
    }

    [Test]
    public void GenerateRectangularGridWithOriginShouldOffsetCoordinates()
    {
        var origin = new AxialHexCoordinate(5, 10);
        var grid = HexGridGenerator.GenerateRectangularGrid(_pointyLayout, 2, 2, null, origin);

        Assert.That(grid[0][0], Is.EqualTo(new AxialHexCoordinate(5, 10)));
        Assert.That(grid[0][1], Is.EqualTo(new AxialHexCoordinate(6, 10)));
        Assert.That(grid[1][0], Is.EqualTo(new AxialHexCoordinate(5, 11)));
        Assert.That(grid[1][1], Is.EqualTo(new AxialHexCoordinate(6, 11)));
    }

    [Test]
    public void GenerateRectangularGridWithNullOriginShouldDefaultToZeroZero()
    {
        var grid = HexGridGenerator.GenerateRectangularGrid(_pointyLayout, 1, 1, null);

        Assert.That(grid[0][0], Is.EqualTo(new AxialHexCoordinate(0, 0)));
    }

    #endregion

    #region GenerateRectangularOffsetGrid Tests

    [Test]
    public void GenerateRectangularOffsetGridShouldCreateCorrectDimensions()
    {
        var grid = HexGridGenerator.GenerateRectangularOffsetGrid(_pointyLayout, 3, 2, OffsetHexCoordinateType.EvenQ);

        Assert.That(grid.Length, Is.EqualTo(2));
        Assert.That(grid[0].Length, Is.EqualTo(3));
        Assert.That(grid[1].Length, Is.EqualTo(3));
    }

    [Test]
    public void GenerateRectangularOffsetGridWithEvenQShouldConvertCorrectly()
    {
        var grid = HexGridGenerator.GenerateRectangularOffsetGrid(_pointyLayout, 2, 2, OffsetHexCoordinateType.EvenQ);

        Assert.That(grid[0][0], Is.Not.Null);
        Assert.That(grid[1][1], Is.Not.Null);
    }

    [Test]
    public void GenerateRectangularOffsetGridWithOriginShouldOffsetCoordinates()
    {
        var origin = new PointI(5, 10);
        var grid = HexGridGenerator.GenerateRectangularOffsetGrid(_pointyLayout, 2, 2, OffsetHexCoordinateType.EvenQ, origin);

        var expectedFirstCell = new OffsetHexCoordinate(0, 0).ToAxial(OffsetHexCoordinateType.EvenQ);
        Assert.That(grid[0][0], Is.EqualTo(new AxialHexCoordinate(expectedFirstCell.Q + 5, expectedFirstCell.R + 10)));
    }

    [Test]
    public void GenerateRectangularOffsetGridWithNullOriginShouldDefaultToZeroZero()
    {
        var grid = HexGridGenerator.GenerateRectangularOffsetGrid(_pointyLayout, 1, 1, OffsetHexCoordinateType.EvenQ, null);

        var expectedCell = new OffsetHexCoordinate(0, 0).ToAxial(OffsetHexCoordinateType.EvenQ);
        Assert.That(grid[0][0], Is.EqualTo(expectedCell));
    }

    [TestCase(OffsetHexCoordinateType.EvenQ)]
    [TestCase(OffsetHexCoordinateType.OddQ)]
    [TestCase(OffsetHexCoordinateType.EvenR)]
    [TestCase(OffsetHexCoordinateType.OddR)]
    public void GenerateRectangularOffsetGridShouldWorkWithAllOffsetTypes(OffsetHexCoordinateType offsetType)
    {
        var grid = HexGridGenerator.GenerateRectangularOffsetGrid(_pointyLayout, 2, 2, offsetType);

        Assert.That(grid.Length, Is.EqualTo(2));
        Assert.That(grid[0].Length, Is.EqualTo(2));
    }

    #endregion

    #region GenerateHexagonalGrid Tests

    [Test]
    public void GenerateHexagonalGridShouldCreateCorrectDiameter()
    {
        var radius = 2;
        var grid = HexGridGenerator.GenerateHexagonalGrid(_pointyLayout, radius);

        var expectedDiameter = radius * 2 + 1;
        Assert.That(grid.Length, Is.EqualTo(expectedDiameter));
    }

    [Test]
    public void GenerateHexagonalGridWithRadiusZeroShouldCreateSingleHex()
    {
        var grid = HexGridGenerator.GenerateHexagonalGrid(_pointyLayout, 0);

        Assert.That(grid.Length, Is.EqualTo(1));
        Assert.That(grid[0].Length, Is.EqualTo(1));
        Assert.That(grid[0][0], Is.EqualTo(new AxialHexCoordinate(0, 0)));
    }

    [Test]
    public void GenerateHexagonalGridWithRadiusOneShouldCreateSevenHexes()
    {
        var grid = HexGridGenerator.GenerateHexagonalGrid(_pointyLayout, 1);

        Assert.That(grid.Length, Is.EqualTo(3));
        var totalHexes = grid.Sum(row => row.Length);
        Assert.That(totalHexes, Is.EqualTo(7));
    }

    [Test]
    public void GenerateHexagonalGridWithOriginShouldOffsetCoordinates()
    {
        var origin = new AxialHexCoordinate(10, 10);
        var grid = HexGridGenerator.GenerateHexagonalGrid(_pointyLayout, 1, origin);

        Assert.That(grid.SelectMany(row => row), Does.Contain(new AxialHexCoordinate(10, 10)));
    }

    [Test]
    public void GenerateHexagonalGridShouldHaveSymmetricRowWidths()
    {
        var grid = HexGridGenerator.GenerateHexagonalGrid(_pointyLayout, 2);

        Assert.That(grid[0].Length, Is.EqualTo(grid[4].Length));
        Assert.That(grid[1].Length, Is.EqualTo(grid[3].Length));
    }

    #endregion

    #region GenerateTriangularGrid Tests

    [Test]
    public void GenerateTriangularGridPointyNormalShouldCreateCorrectShape()
    {
        var grid = HexGridGenerator.GenerateTriangularGrid(_pointyLayout, 3, invert: false);

        Assert.That(grid.Length, Is.EqualTo(3));
        Assert.That(grid[0].Length, Is.EqualTo(3));
        Assert.That(grid[1].Length, Is.EqualTo(2));
        Assert.That(grid[2].Length, Is.EqualTo(1));
    }

    [Test]
    public void GenerateTriangularGridPointyInvertedShouldCreateCorrectShape()
    {
        var grid = HexGridGenerator.GenerateTriangularGrid(_pointyLayout, 3, invert: true);

        Assert.That(grid.Length, Is.EqualTo(3));
        Assert.That(grid[0].Length, Is.EqualTo(1));
        Assert.That(grid[1].Length, Is.EqualTo(2));
        Assert.That(grid[2].Length, Is.EqualTo(3));
    }

    [Test]
    public void GenerateTriangularGridFlatNormalShouldCreateCorrectShape()
    {
        var grid = HexGridGenerator.GenerateTriangularGrid(_flatLayout, 3, invert: false);

        Assert.That(grid.Length, Is.EqualTo(3));
    }

    [Test]
    public void GenerateTriangularGridFlatInvertedShouldCreateCorrectShape()
    {
        var grid = HexGridGenerator.GenerateTriangularGrid(_flatLayout, 3, invert: true);

        Assert.That(grid.Length, Is.EqualTo(3));
    }

    [Test]
    public void GenerateTriangularGridWithOriginShouldOffsetCoordinates()
    {
        var origin = new AxialHexCoordinate(5, 5);
        var grid = HexGridGenerator.GenerateTriangularGrid(_pointyLayout, 2, invert: false, origin);

        Assert.That(grid[0][0], Is.EqualTo(new AxialHexCoordinate(5, 5)));
    }

    [Test]
    public void GenerateTriangularGridWithSizeOneShouldCreateSingleHex()
    {
        var grid = HexGridGenerator.GenerateTriangularGrid(_pointyLayout, 1, invert: false);

        Assert.That(grid.Length, Is.EqualTo(1));
        Assert.That(grid[0].Length, Is.EqualTo(1));
    }

    #endregion

    #region GenerateParallelogramGrid Tests

    [Test]
    public void GenerateParallelogramGridQRShouldCreateCorrectDimensions()
    {
        var grid = HexGridGenerator.GenerateParallelogramGrid(_pointyLayout, 3, 2, ParallelogramOrientation.QR);

        Assert.That(grid.Length, Is.EqualTo(2));
        Assert.That(grid[0].Length, Is.EqualTo(3));
    }

    [Test]
    public void GenerateParallelogramGridQRWithOriginShouldOffsetCoordinates()
    {
        var origin = new AxialHexCoordinate(5, 10);
        var grid = HexGridGenerator.GenerateParallelogramGrid(_pointyLayout, 2, 2, ParallelogramOrientation.QR, origin);

        Assert.That(grid[0][0], Is.EqualTo(new AxialHexCoordinate(5, 10)));
        Assert.That(grid[0][1], Is.EqualTo(new AxialHexCoordinate(6, 10)));
    }

    [Test]
    public void GenerateParallelogramGridSQShouldCreateCorrectDimensions()
    {
        var grid = HexGridGenerator.GenerateParallelogramGrid(_pointyLayout, 3, 2, ParallelogramOrientation.SQ);

        Assert.That(grid.Length, Is.EqualTo(2));
        Assert.That(grid[0].Length, Is.EqualTo(3));
    }

    [Test]
    public void GenerateParallelogramGridSQShouldMaintainCubeConstraint()
    {
        var grid = HexGridGenerator.GenerateParallelogramGrid(_pointyLayout, 2, 2, ParallelogramOrientation.SQ);

        foreach (var row in grid)
        {
            foreach (var hex in row)
            {
                var cube = hex.ToCube();
                Assert.That(cube.Q + cube.R + cube.S, Is.EqualTo(0));
            }
        }
    }

    [Test]
    public void GenerateParallelogramGridRSShouldCreateCorrectDimensions()
    {
        var grid = HexGridGenerator.GenerateParallelogramGrid(_pointyLayout, 3, 2, ParallelogramOrientation.RS);

        Assert.That(grid.Length, Is.EqualTo(2));
        Assert.That(grid[0].Length, Is.EqualTo(3));
    }

    [Test]
    public void GenerateParallelogramGridRSShouldMaintainCubeConstraint()
    {
        var grid = HexGridGenerator.GenerateParallelogramGrid(_pointyLayout, 2, 2, ParallelogramOrientation.RS);

        foreach (var row in grid)
        {
            foreach (var hex in row)
            {
                var cube = hex.ToCube();
                Assert.That(cube.Q + cube.R + cube.S, Is.EqualTo(0));
            }
        }
    }

    [Test]
    public void GenerateParallelogramGridWithInvalidOrientationShouldThrow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            HexGridGenerator.GenerateParallelogramGrid(_pointyLayout, 2, 2, (ParallelogramOrientation)999));
    }

    #endregion

    #region GenerateWorldMap Tests

    [Test]
    public void GenerateWorldMapShouldCreateNonEmptyGrid()
    {
        var grid = HexGridGenerator.GenerateWorldMap(_pointyLayout, 3);

        Assert.That(grid, Is.Not.Empty);
        Assert.That(grid, Has.All.Not.Empty);
    }

    [Test]
    public void GenerateWorldMapShouldRemoveDuplicates()
    {
        var grid = HexGridGenerator.GenerateWorldMap(_pointyLayout, 2);

        var allHexes = grid.SelectMany(row => row).ToList();
        var uniqueHexes = allHexes.Distinct().ToList();

        Assert.That(allHexes.Count, Is.EqualTo(uniqueHexes.Count));
    }

    [Test]
    public void GenerateWorldMapWithOriginShouldOffsetCoordinates()
    {
        var origin = new AxialHexCoordinate(10, 10);
        var grid = HexGridGenerator.GenerateWorldMap(_pointyLayout, 2, origin);

        var allHexes = grid.SelectMany(row => row);
        foreach (var hex in allHexes)
        {
            Assert.That(hex.Q >= origin.Q - 10 || hex.R >= origin.R - 10, Is.True);
        }
    }

    [Test]
    public void GenerateWorldMapWithSizeOneShouldCreateSmallGrid()
    {
        var grid = HexGridGenerator.GenerateWorldMap(_pointyLayout, 1);

        Assert.That(grid, Is.Not.Empty);
    }

    [Test]
    public void GenerateWorldMapShouldBeSortedByQThenR()
    {
        var grid = HexGridGenerator.GenerateWorldMap(_pointyLayout, 2);

        for (var i = 1; i < grid.Length; i++)
        {
            Assert.That(grid[i][0].Q, Is.GreaterThanOrEqualTo(grid[i - 1][0].Q));
        }

        foreach (var row in grid)
        {
            for (var j = 1; j < row.Length; j++)
            {
                Assert.That(row[j].R, Is.GreaterThanOrEqualTo(row[j - 1].R));
            }
        }
    }

    #endregion

    #region ParallelogramOrientation Enum Tests

    [Test]
    public void ParallelogramOrientationShouldHaveThreeValues()
    {
        var values = Enum.GetValues(typeof(ParallelogramOrientation));
        Assert.That(values.Length, Is.EqualTo(3));
    }

    [Test]
    public void ParallelogramOrientationShouldContainExpectedValues()
    {
        Assert.That(Enum.IsDefined(typeof(ParallelogramOrientation), ParallelogramOrientation.QR), Is.True);
        Assert.That(Enum.IsDefined(typeof(ParallelogramOrientation), ParallelogramOrientation.SQ), Is.True);
        Assert.That(Enum.IsDefined(typeof(ParallelogramOrientation), ParallelogramOrientation.RS), Is.True);
    }

    #endregion
}
