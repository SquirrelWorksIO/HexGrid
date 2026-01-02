namespace HexGrid.Tests.Models.Coordinates;

using HexGrid.Models.Coordinates;

[TestFixture]
public class FractionalHexCoordinateTests
{
    [Test]
    public void FromAxialConvertsCorrectly()
    {
        var fractional = FractionalHexCoordinate.FromAxial(2, -1);
        
        Assert.That(fractional.Q, Is.EqualTo(2.0));
        Assert.That(fractional.R, Is.EqualTo(-1.0));
        Assert.That(fractional.S, Is.EqualTo(-1.0));
    }

    [Test]
    public void FromCubeWithValidCoordinatesCreatesInstance()
    {
        var fractional = FractionalHexCoordinate.FromCube(1, -1, 0);
        
        Assert.That(fractional.Q, Is.EqualTo(1.0));
        Assert.That(fractional.R, Is.EqualTo(-1.0));
        Assert.That(fractional.S, Is.EqualTo(0.0));
    }

    [Test]
    public void FromCubeWithInvalidCoordinatesThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => FractionalHexCoordinate.FromCube(1, 1, 1));
    }

    [Test]
    public void ToAxialRoundsToNearestAxialCoordinate()
    {
        var fractional = new FractionalHexCoordinate(1.6, -0.8, -0.8);
        
        var axial = fractional.ToAxial();
        
        Assert.That(axial.Q, Is.EqualTo(2));
        Assert.That(axial.R, Is.EqualTo(-1));
    }

    [Test]
    public void ToCubeRoundsToNearestCubeCoordinate()
    {
        var fractional = new FractionalHexCoordinate(1.7, -0.9, -0.8);
        
        var cube = fractional.ToCube();
        
        Assert.That(cube.Q, Is.EqualTo(2));
        Assert.That(cube.R, Is.EqualTo(-1));
        Assert.That(cube.S, Is.EqualTo(-1));
    }

    [Test]
    public void RoundingPreservesCoordinateConstraint()
    {
        var fractional = new FractionalHexCoordinate(2.3, -1.7, -0.6);
        
        var cube = fractional.ToCube();
        
        Assert.That(cube.Q + cube.R + cube.S, Is.EqualTo(0));
    }

    [Test]
    public void RoundingAdjustsCoordinateWithLargestError()
    {
        var fractional = new FractionalHexCoordinate(1.9, -1.1, -0.8);
        
        var cube = fractional.ToCube();
        
        Assert.That(cube.Q, Is.EqualTo(2));
        Assert.That(cube.R, Is.EqualTo(-1));
        Assert.That(cube.S, Is.EqualTo(-1));
    }

    [Test]
    public void ToAxialViaIntermediateCubeRounding()
    {
        var fractional = new FractionalHexCoordinate(0.5, 0.5, -1.0);
        
        var axial = fractional.ToAxial();
        
        Assert.That(axial.Q + axial.R, Is.EqualTo(-axial.ToCube().S));
    }

    [Test]
    public void ExactIntegerCoordinatesRoundToSameValues()
    {
        var fractional = new FractionalHexCoordinate(3.0, -1.0, -2.0);
        
        var cube = fractional.ToCube();
        
        Assert.That(cube.Q, Is.EqualTo(3));
        Assert.That(cube.R, Is.EqualTo(-1));
        Assert.That(cube.S, Is.EqualTo(-2));
    }
}
