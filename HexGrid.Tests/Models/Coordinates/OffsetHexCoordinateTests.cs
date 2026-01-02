namespace HexGrid.Tests.Models.Coordinates;

using HexGrid.Models.Coordinates;

[TestFixture]
public class OffsetHexCoordinateTests
{
    [Test]
    public void ConstructorSetsPropertiesCorrectly()
    {
        var offset = new OffsetHexCoordinate(3, 5);
        
        Assert.That(offset.Col, Is.EqualTo(3));
        Assert.That(offset.Row, Is.EqualTo(5));
    }

    [Test]
    public void FromAxialWithEvenQConvertsCorrectly()
    {
        var axial = new AxialHexCoordinate(2, 1);
        
        var offset = OffsetHexCoordinate.FromAxial(axial, OffsetHexCoordinateType.EvenQ);
        
        Assert.That(offset.Col, Is.EqualTo(2));
        Assert.That(offset.Row, Is.EqualTo(2));
    }

    [Test]
    public void FromAxialWithOddQConvertsCorrectly()
    {
        var axial = new AxialHexCoordinate(1, 1);
        
        var offset = OffsetHexCoordinate.FromAxial(axial, OffsetHexCoordinateType.OddQ);
        
        Assert.That(offset.Col, Is.EqualTo(1));
        Assert.That(offset.Row, Is.EqualTo(1));
    }

    [Test]
    public void FromAxialWithEvenRConvertsCorrectly()
    {
        var axial = new AxialHexCoordinate(1, 2);
        
        var offset = OffsetHexCoordinate.FromAxial(axial, OffsetHexCoordinateType.EvenR);
        
        Assert.That(offset.Col, Is.EqualTo(2));
        Assert.That(offset.Row, Is.EqualTo(2));
    }

    [Test]
    public void FromAxialWithOddRConvertsCorrectly()
    {
        var axial = new AxialHexCoordinate(1, 1);
        
        var offset = OffsetHexCoordinate.FromAxial(axial, OffsetHexCoordinateType.OddR);
        
        Assert.That(offset.Col, Is.EqualTo(1));
        Assert.That(offset.Row, Is.EqualTo(1));
    }

    [Test]
    public void ToAxialWithEvenQConvertsCorrectly()
    {
        var offset = new OffsetHexCoordinate(2, 2);
        
        var axial = offset.ToAxial(OffsetHexCoordinateType.EvenQ);
        
        Assert.That(axial.Q, Is.EqualTo(2));
        Assert.That(axial.R, Is.EqualTo(1));
    }
    [Test]
    public void ToAxialWithOddQConvertsCorrectly()
    {
        var offset = new OffsetHexCoordinate(1, 1);
        
        var axial = offset.ToAxial(OffsetHexCoordinateType.OddQ);
        
        Assert.That(axial.Q, Is.EqualTo(1));
        Assert.That(axial.R, Is.EqualTo(1));
    }

    [Test]
    public void ToAxialWithEvenRConvertsCorrectly()
    {
        var offset = new OffsetHexCoordinate(2, 2);
        
        var axial = offset.ToAxial(OffsetHexCoordinateType.EvenR);
        
        Assert.That(axial.Q, Is.EqualTo(1));
        Assert.That(axial.R, Is.EqualTo(2));
    }

    [Test]
    public void ToAxialWithOddRConvertsCorrectly()
    {
        var offset = new OffsetHexCoordinate(1, 1);
        
        var axial = offset.ToAxial(OffsetHexCoordinateType.OddR);
        
        Assert.That(axial.Q, Is.EqualTo(1));
        Assert.That(axial.R, Is.EqualTo(1));
    }

    [Test]
    public void FromCubeConvertsCorrectly()
    {
        var cube = new CubHexCoordinate(2, -1, -1);
        
        var offset = OffsetHexCoordinate.FromCube(cube, OffsetHexCoordinateType.EvenQ);
        
        Assert.That(offset.Col, Is.EqualTo(2));
    }

    [Test]
    public void ToCubeConvertsCorrectly()
    {
        var offset = new OffsetHexCoordinate(2, 1);
        
        var cube = offset.ToCube(OffsetHexCoordinateType.EvenQ);
        
        Assert.That(cube.Q + cube.R + cube.S, Is.EqualTo(0));
    }

    [Test]
    public void StaticToAxialMethodConvertsCorrectly()
    {
        var axial = OffsetHexCoordinate.ToAxial(2, 2, OffsetHexCoordinateType.EvenQ);
        
        Assert.That(axial.Q, Is.EqualTo(2));
        Assert.That(axial.R, Is.EqualTo(1));
    }

    [Test]
    public void StaticToCubeMethodConvertsCorrectly()
    {
        var cube = OffsetHexCoordinate.ToCube(2, 2, OffsetHexCoordinateType.EvenQ);
        
        Assert.That(cube.Q + cube.R + cube.S, Is.EqualTo(0));
    }

    [Test]
    public void RoundTripConversionPreservesValueEvenQ()
    {
        var original = new AxialHexCoordinate(3, -1);
        
        var offset = OffsetHexCoordinate.FromAxial(original, OffsetHexCoordinateType.EvenQ);
        var result = offset.ToAxial(OffsetHexCoordinateType.EvenQ);
        
        Assert.That(result, Is.EqualTo(original));
    }

    [Test]
    public void RoundTripConversionPreservesValueOddR()
    {
        var original = new AxialHexCoordinate(2, 3);
        
        var offset = OffsetHexCoordinate.FromAxial(original, OffsetHexCoordinateType.OddR);
        var result = offset.ToAxial(OffsetHexCoordinateType.OddR);
        
        Assert.That(result, Is.EqualTo(original));
    }

    [Test]
    public void StaticFromAxialWithParametersConvertsCorrectly()
    {
        var result = OffsetHexCoordinate.FromAxial(2, 1, OffsetHexCoordinateType.EvenQ);
        
        Assert.That(result.X, Is.EqualTo(2));
        Assert.That(result.Y, Is.EqualTo(2));
    }

    [Test]
    public void StaticFromAxialWithParametersWorksForAllTypes()
    {
        var evenQ = OffsetHexCoordinate.FromAxial(1, 1, OffsetHexCoordinateType.EvenQ);
        var oddQ = OffsetHexCoordinate.FromAxial(1, 1, OffsetHexCoordinateType.OddQ);
        var evenR = OffsetHexCoordinate.FromAxial(1, 1, OffsetHexCoordinateType.EvenR);
        var oddR = OffsetHexCoordinate.FromAxial(1, 1, OffsetHexCoordinateType.OddR);
        
        Assert.That(evenQ.X, Is.EqualTo(1));
        Assert.That(evenQ.Y, Is.EqualTo(2));
        Assert.That(oddQ.X, Is.EqualTo(1));
        Assert.That(oddQ.Y, Is.EqualTo(1));
        Assert.That(evenR.X, Is.EqualTo(2));
        Assert.That(evenR.Y, Is.EqualTo(1));
        Assert.That(oddR.X, Is.EqualTo(1));
        Assert.That(oddR.Y, Is.EqualTo(1));
    }

    [Test]
    public void StaticFromCubeWithParametersConvertsCorrectly()
    {
        var result = OffsetHexCoordinate.FromCube(1, 1, -2, OffsetHexCoordinateType.EvenQ);
        
        Assert.That(result.X, Is.EqualTo(1));
        Assert.That(result.Y, Is.EqualTo(2));
    }

    [Test]
    public void StaticFromCubeWithParametersWorksForAllTypes()
    {
        var evenQ = OffsetHexCoordinate.FromCube(1, 1, -2, OffsetHexCoordinateType.EvenQ);
        var oddQ = OffsetHexCoordinate.FromCube(1, 1, -2, OffsetHexCoordinateType.OddQ);
        var evenR = OffsetHexCoordinate.FromCube(1, 1, -2, OffsetHexCoordinateType.EvenR);
        var oddR = OffsetHexCoordinate.FromCube(1, 1, -2, OffsetHexCoordinateType.OddR);
        
        Assert.That(evenQ.X, Is.EqualTo(1));
        Assert.That(evenQ.Y, Is.EqualTo(2));
        Assert.That(oddQ.X, Is.EqualTo(1));
        Assert.That(oddQ.Y, Is.EqualTo(1));
        Assert.That(evenR.X, Is.EqualTo(2));
        Assert.That(evenR.Y, Is.EqualTo(1));
        Assert.That(oddR.X, Is.EqualTo(1));
        Assert.That(oddR.Y, Is.EqualTo(1));
    }
}
