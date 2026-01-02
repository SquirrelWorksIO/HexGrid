namespace HexGrid.Tests.Models.Layout;

using HexGrid.Models.Coordinates;
using HexGrid.Models.Layout;

[TestFixture]
public class GridLayoutTests
{
    private GridLayout _layout;

    [SetUp]
    public void Setup()
    {
        var orientation = LayoutOrientation.Pointy;
        var size = new PointD(10.0, 10.0);
        var origin = new FractionalHexCoordinate(0.0, 0.0, 0.0);
        _layout = new GridLayout(orientation, size, origin);
    }

    [Test]
    public void ConstructorSetsPropertiesCorrectly()
    {
        var orientation = LayoutOrientation.Flat;
        var size = new PointD(5.0, 7.0);
        var origin = new FractionalHexCoordinate(100.0, 200.0, -300.0);
        
        var layout = new GridLayout(orientation, size, origin);
        
        Assert.That(layout.Orientation, Is.EqualTo(orientation));
        Assert.That(layout.Size, Is.EqualTo(size));
        Assert.That(layout.PixelOrigin, Is.EqualTo(origin));
    }

    [Test]
    public void HexToPixelConvertsOriginCorrectly()
    {
        var hex = new AxialHexCoordinate(0, 0);
        
        var pixel = _layout.HexToPixel(hex);
        
        Assert.That(pixel.X, Is.EqualTo(0.0).Within(1e-6));
        Assert.That(pixel.Y, Is.EqualTo(0.0).Within(1e-6));
    }

    [Test]
    public void HexToPixelConvertsSimpleCoordinateCorrectly()
    {
        var hex = new AxialHexCoordinate(1, 0);
        
        var pixel = _layout.HexToPixel(hex);
        
        Assert.That(pixel.X, Is.EqualTo(Math.Sqrt(3.0) * 10.0).Within(1e-6));
        Assert.That(pixel.Y, Is.EqualTo(0.0).Within(1e-6));
    }

    [Test]
    public void PixelToHexConvertsOriginCorrectly()
    {
        var pixel = new PointD(0.0, 0.0);
        
        var hex = _layout.PixelToHex(pixel);
        
        Assert.That(hex.Q, Is.EqualTo(0.0).Within(1e-6));
        Assert.That(hex.R, Is.EqualTo(0.0).Within(1e-6));
        Assert.That(hex.S, Is.EqualTo(0.0).Within(1e-6));
    }

    [Test]
    public void PixelToHexRoundTripPreservesCoordinate()
    {
        var originalHex = new AxialHexCoordinate(3, -1);
        
        var pixel = _layout.HexToPixel(originalHex);
        var fractionalHex = _layout.PixelToHex(pixel);
        var roundedHex = fractionalHex.ToAxial();
        
        Assert.That(roundedHex, Is.EqualTo(originalHex));
    }

    [Test]
    public void HexCornerOffsetReturnsValidOffsets()
    {
        var offset = _layout.HexCornerOffset(0);
        
        Assert.That(offset.X, Is.Not.NaN);
        Assert.That(offset.Y, Is.Not.NaN);
    }

    [Test]
    public void HexCornerOffsetGeneratesSixDistinctCorners()
    {
        var corners = new List<PointD>();
        
        for (var i = 0; i < 6; i++)
        {
            corners.Add(_layout.HexCornerOffset(i));
        }
        
        Assert.That(corners, Has.Count.EqualTo(6));
    }

    [Test]
    public void PolygonCornersReturnsSixCorners()
    {
        var hex = new AxialHexCoordinate(0, 0);
        
        var corners = _layout.PolygonCorners(hex);
        
        Assert.That(corners, Has.Count.EqualTo(6));
    }

    [Test]
    public void PolygonCornersForOriginContainsValidPoints()
    {
        var hex = new AxialHexCoordinate(0, 0);
        
        var corners = _layout.PolygonCorners(hex);
        
        foreach (var corner in corners)
        {
            Assert.That(corner.X, Is.Not.NaN);
            Assert.That(corner.Y, Is.Not.NaN);
        }
    }

    [Test]
    public void PolygonCornersAreCenteredAroundHexCenter()
    {
        var hex = new AxialHexCoordinate(2, -1);
        var center = _layout.HexToPixel(hex);
        
        var corners = _layout.PolygonCorners(hex);
        var avgX = corners.Average(c => c.X);
        var avgY = corners.Average(c => c.Y);
        
        Assert.That(avgX, Is.EqualTo(center.X).Within(1e-6));
        Assert.That(avgY, Is.EqualTo(center.Y).Within(1e-6));
    }

    [Test]
    public void FlatLayoutConvertsHexToPixelDifferently()
    {
        var flatOrientation = LayoutOrientation.Flat;
        var flatLayout = new GridLayout(flatOrientation, new PointD(10.0, 10.0), new FractionalHexCoordinate(0.0, 0.0, 0.0));
        var hex = new AxialHexCoordinate(1, 0);
        
        var pointyPixel = _layout.HexToPixel(hex);
        var flatPixel = flatLayout.HexToPixel(hex);
        
        Assert.That(pointyPixel.X, Is.Not.EqualTo(flatPixel.X));
    }

    [Test]
    public void LayoutWithOffsetOriginAppliesCorrectly()
    {
        var offsetLayout = new GridLayout(
            LayoutOrientation.Pointy,
            new PointD(10.0, 10.0),
            new FractionalHexCoordinate(100.0, 200.0, -300.0));
        var hex = new AxialHexCoordinate(0, 0);
        
        var pixel = offsetLayout.HexToPixel(hex);
        
        Assert.That(pixel.X, Is.EqualTo(100.0).Within(1e-6));
        Assert.That(pixel.Y, Is.EqualTo(200.0).Within(1e-6));
    }
}
