namespace HexGrid.Tests.Models.Coordinates;

using HexGrid.Models.Coordinates;

[TestFixture]
public class CubHexCoordinateTests
{
    [Test]
    public void ConstructorWithValidCoordinatesCreatesInstance()
    {
        var cube = new CubHexCoordinate(1, -2, 1);
        
        Assert.That(cube.Q, Is.EqualTo(1));
        Assert.That(cube.R, Is.EqualTo(-2));
        Assert.That(cube.S, Is.EqualTo(1));
    }

    [Test]
    public void ConstructorWithInvalidCoordinatesThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new CubHexCoordinate(1, 1, 1));
    }

    [Test]
    public void ConstructorValidatesConstraintQPlusRPlusSEqualsZero()
    {
        var exception = Assert.Throws<ArgumentException>(() => new CubHexCoordinate(2, 2, 2));
        
        Assert.That(exception.Message, Does.Contain("Q + R + S must equal 0"));
    }

    [Test]
    public void LengthReturnsCorrectDistanceFromOrigin()
    {
        var coord = new CubHexCoordinate(3, -1, -2);
        
        var result = coord.Length;
        
        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void LengthReturnsZeroForOrigin()
    {
        var coord = new CubHexCoordinate(0, 0, 0);
        
        var result = coord.Length;
        
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void DistanceToCalculatesCorrectDistance()
    {
        var coord1 = new CubHexCoordinate(1, 1, -2);
        var coord2 = new CubHexCoordinate(4, -2, -2);
        
        var result = coord1.DistanceTo(coord2);
        
        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void DistanceToReturnsZeroForSameCoordinate()
    {
        var coord = new CubHexCoordinate(2, -1, -1);
        
        var result = coord.DistanceTo(coord);
        
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void FromAxialWithParametersConvertsCorrectly()
    {
        var cube = CubHexCoordinate.FromAxial(2, -1);
        
        Assert.That(cube.Q, Is.EqualTo(2));
        Assert.That(cube.R, Is.EqualTo(-1));
        Assert.That(cube.S, Is.EqualTo(-1));
    }

    [Test]
    public void FromAxialWithObjectConvertsCorrectly()
    {
        var axial = new AxialHexCoordinate(3, -2);
        
        var cube = CubHexCoordinate.FromAxial(axial);
        
        Assert.That(cube.Q, Is.EqualTo(3));
        Assert.That(cube.R, Is.EqualTo(-2));
        Assert.That(cube.S, Is.EqualTo(-1));
    }

    [Test]
    public void ToAxialConvertsCorrectly()
    {
        var cube = new CubHexCoordinate(2, -1, -1);
        
        var axial = cube.ToAxial();
        
        Assert.That(axial.Q, Is.EqualTo(2));
        Assert.That(axial.R, Is.EqualTo(-1));
    }

    [Test]
    public void AdditionOperatorCombinesCoordinatesCorrectly()
    {
        var a = new CubHexCoordinate(1, 2, -3);
        var b = new CubHexCoordinate(3, -1, -2);
        
        var result = a + b;
        
        Assert.That(result.Q, Is.EqualTo(4));
        Assert.That(result.R, Is.EqualTo(1));
        Assert.That(result.S, Is.EqualTo(-5));
    }

    [Test]
    public void SubtractionOperatorCalculatesDifferenceCorrectly()
    {
        var a = new CubHexCoordinate(5, 2, -7);
        var b = new CubHexCoordinate(2, 3, -5);
        
        var result = a - b;
        
        Assert.That(result.Q, Is.EqualTo(3));
        Assert.That(result.R, Is.EqualTo(-1));
        Assert.That(result.S, Is.EqualTo(-2));
    }

    [Test]
    public void MultiplicationOperatorScalesCoordinateCorrectly()
    {
        var coord = new CubHexCoordinate(2, -1, -1);
        
        var result = coord * 3;
        
        Assert.That(result.Q, Is.EqualTo(6));
        Assert.That(result.R, Is.EqualTo(-3));
        Assert.That(result.S, Is.EqualTo(-3));
    }

    [Test]
    public void MultiplicationByZeroCreatesOrigin()
    {
        var coord = new CubHexCoordinate(5, -3, -2);
        
        var result = coord * 0;
        
        Assert.That(result.Q, Is.EqualTo(0));
        Assert.That(result.R, Is.EqualTo(0));
        Assert.That(result.S, Is.EqualTo(0));
    }

    [Test]
    public void MultiplicationByNegativeScalarInvertsDirection()
    {
        var coord = new CubHexCoordinate(2, -1, -1);
        
        var result = coord * -1;
        
        Assert.That(result.Q, Is.EqualTo(-2));
        Assert.That(result.R, Is.EqualTo(1));
        Assert.That(result.S, Is.EqualTo(1));
    }
}
