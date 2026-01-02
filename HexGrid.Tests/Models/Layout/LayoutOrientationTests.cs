namespace HexGrid.Tests.Models.Layout;

using HexGrid.Models.Layout;

[TestFixture]
public class LayoutOrientationTests
{
    [Test]
    public void PointyOrientationHasCorrectProperties()
    {
        var orientation = LayoutOrientation.Pointy;
        
        Assert.That(orientation.F, Has.Length.EqualTo(4));
        Assert.That(orientation.B, Has.Length.EqualTo(4));
        Assert.That(orientation.StartAngle, Is.EqualTo(0.5));
        Assert.That(orientation.IsPointy, Is.True);
    }

    [Test]
    public void FlatOrientationHasCorrectProperties()
    {
        var orientation = LayoutOrientation.Flat;
        
        Assert.That(orientation.F, Has.Length.EqualTo(4));
        Assert.That(orientation.B, Has.Length.EqualTo(4));
        Assert.That(orientation.StartAngle, Is.EqualTo(0.0));
        Assert.That(orientation.IsPointy, Is.False);
    }

    [Test]
    public void PointyOrientationStartAngleCorrect()
    {
        var orientation = LayoutOrientation.Pointy;
        
        Assert.That(orientation.StartAngle, Is.EqualTo(0.5).Within(1e-6));
    }

    [Test]
    public void FlatOrientationStartAngleCorrect()
    {
        var orientation = LayoutOrientation.Flat;
        
        Assert.That(orientation.StartAngle, Is.EqualTo(0.0).Within(1e-6));
    }

    [Test]
    public void IsPointyPropertyCorrectForPointy()
    {
        var orientation = LayoutOrientation.Pointy;
        
        Assert.That(orientation.IsPointy, Is.True);
    }

    [Test]
    public void IsPointyPropertyCorrectForFlat()
    {
        var orientation = LayoutOrientation.Flat;
        
        Assert.That(orientation.IsPointy, Is.False);
    }

    [Test]
    public void PointyForwardMatrixHasExpectedValues()
    {
        var orientation = LayoutOrientation.Pointy;
        
        Assert.That(orientation.F[0], Is.EqualTo(Math.Sqrt(3.0)).Within(1e-6));
        Assert.That(orientation.F[1], Is.EqualTo(Math.Sqrt(3.0) / 2.0).Within(1e-6));
        Assert.That(orientation.F[2], Is.EqualTo(0.0).Within(1e-6));
        Assert.That(orientation.F[3], Is.EqualTo(3.0 / 2.0).Within(1e-6));
    }

    [Test]
    public void FlatForwardMatrixHasExpectedValues()
    {
        var orientation = LayoutOrientation.Flat;
        
        Assert.That(orientation.F[0], Is.EqualTo(3.0 / 2.0).Within(1e-6));
        Assert.That(orientation.F[1], Is.EqualTo(0.0).Within(1e-6));
        Assert.That(orientation.F[2], Is.EqualTo(Math.Sqrt(3.0) / 2.0).Within(1e-6));
        Assert.That(orientation.F[3], Is.EqualTo(Math.Sqrt(3.0)).Within(1e-6));
    }

    [Test]
    public void PointyBackwardMatrixHasExpectedValues()
    {
        var orientation = LayoutOrientation.Pointy;
        
        Assert.That(orientation.B[0], Is.EqualTo(Math.Sqrt(3.0) / 3.0).Within(1e-6));
        Assert.That(orientation.B[1], Is.EqualTo(-1.0 / 3.0).Within(1e-6));
        Assert.That(orientation.B[2], Is.EqualTo(0.0).Within(1e-6));
        Assert.That(orientation.B[3], Is.EqualTo(2.0 / 3.0).Within(1e-6));
    }

    [Test]
    public void FlatBackwardMatrixHasExpectedValues()
    {
        var orientation = LayoutOrientation.Flat;
        
        Assert.That(orientation.B[0], Is.EqualTo(2.0 / 3.0).Within(1e-6));
        Assert.That(orientation.B[1], Is.EqualTo(0.0).Within(1e-6));
        Assert.That(orientation.B[2], Is.EqualTo(-1.0 / 3.0).Within(1e-6));
        Assert.That(orientation.B[3], Is.EqualTo(Math.Sqrt(3.0) / 3.0).Within(1e-6));
    }
}
