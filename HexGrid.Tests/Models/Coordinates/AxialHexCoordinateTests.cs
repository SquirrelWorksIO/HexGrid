namespace HexGrid.Tests.Models.Coordinates;

using HexGrid.Models.Coordinates;

[TestFixture]
public class AxialHexCoordinateTests
{
    [Test]
    public void LengthReturnsCorrectDistanceFromOrigin()
    {
        var coord = new AxialHexCoordinate(3, -1);
        
        var result = coord.Length;
        
        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void LengthReturnsZeroForOrigin()
    {
        var coord = new AxialHexCoordinate(0, 0);
        
        var result = coord.Length;
        
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void DistanceToCalculatesCorrectDistance()
    {
        var coord1 = new AxialHexCoordinate(1, -1);
        var coord2 = new AxialHexCoordinate(4, -2);
        
        var result = coord1.DistanceTo(coord2);
        
        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void DistanceToReturnZeroForSameCoordinate()
    {
        var coord = new AxialHexCoordinate(2, -1);
        
        var result = coord.DistanceTo(coord);
        
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void ToCubeConvertsCorrectly()
    {
        var axial = new AxialHexCoordinate(2, -1);
        
        var cube = axial.ToCube();
        
        Assert.That(cube.Q, Is.EqualTo(2));
        Assert.That(cube.R, Is.EqualTo(-1));
        Assert.That(cube.S, Is.EqualTo(-1));
    }

    [Test]
    public void FromCubeWithValidCoordinatesCreatesAxial()
    {
        var result = AxialHexCoordinate.FromCube(2, -1, -1);
        
        Assert.That(result.Q, Is.EqualTo(2));
        Assert.That(result.R, Is.EqualTo(-1));
    }

    [Test]
    public void FromCubeWithInvalidCoordinatesThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => AxialHexCoordinate.FromCube(1, 1, 1));
    }

    [Test]
    public void FromCubeObjectConvertsCorrectly()
    {
        var cube = new CubHexCoordinate(3, -2, -1);
        
        var axial = AxialHexCoordinate.FromCube(cube);
        
        Assert.That(axial.Q, Is.EqualTo(3));
        Assert.That(axial.R, Is.EqualTo(-2));
    }

    [Test]
    public void AdditionOperatorCombinesCoordinatesCorrectly()
    {
        var a = new AxialHexCoordinate(1, 2);
        var b = new AxialHexCoordinate(3, -1);
        
        var result = a + b;
        
        Assert.That(result.Q, Is.EqualTo(4));
        Assert.That(result.R, Is.EqualTo(1));
    }

    [Test]
    public void SubtractionOperatorCalculatesDifferenceCorrectly()
    {
        var a = new AxialHexCoordinate(5, 2);
        var b = new AxialHexCoordinate(2, 3);
        
        var result = a - b;
        
        Assert.That(result.Q, Is.EqualTo(3));
        Assert.That(result.R, Is.EqualTo(-1));
    }

    [Test]
    public void MultiplicationOperatorScalesCoordinateCorrectly()
    {
        var coord = new AxialHexCoordinate(2, -1);
        
        var result = coord * 3;
        
        Assert.That(result.Q, Is.EqualTo(6));
        Assert.That(result.R, Is.EqualTo(-3));
    }

    [Test]
    public void MultiplicationByZeroCreatesOrigin()
    {
        var coord = new AxialHexCoordinate(5, -3);
        
        var result = coord * 0;
        
        Assert.That(result.Q, Is.EqualTo(0));
        Assert.That(result.R, Is.EqualTo(0));
    }

    [Test]
    public void MultiplicationByNegativeScalarInvertsDirection()
    {
        var coord = new AxialHexCoordinate(2, -1);
        
        var result = coord * -1;
        
        Assert.That(result.Q, Is.EqualTo(-2));
        Assert.That(result.R, Is.EqualTo(1));
    }
}
