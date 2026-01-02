namespace HexGrid.Tests.Models.Layout;

using HexGrid.Models.Layout;

[TestFixture]
public class PointDTests
{
    [Test]
    public void ConstructorSetsPropertiesCorrectly()
    {
        var point = new PointD(3.5, 7.2);
        
        Assert.That(point.X, Is.EqualTo(3.5));
        Assert.That(point.Y, Is.EqualTo(7.2));
    }

    [Test]
    public void ConstructorHandlesZeroValues()
    {
        var point = new PointD(0.0, 0.0);
        
        Assert.That(point.X, Is.EqualTo(0.0));
        Assert.That(point.Y, Is.EqualTo(0.0));
    }

    [Test]
    public void ConstructorHandlesNegativeValues()
    {
        var point = new PointD(-5.3, -2.7);
        
        Assert.That(point.X, Is.EqualTo(-5.3));
        Assert.That(point.Y, Is.EqualTo(-2.7));
    }

    [Test]
    public void PropertiesAreMutable()
    {
        var point = new PointD(1.0, 2.0);
        
        point.X = 5.5;
        point.Y = 6.6;
        
        Assert.That(point.X, Is.EqualTo(5.5));
        Assert.That(point.Y, Is.EqualTo(6.6));
    }
}

[TestFixture]
public class PointITests
{
    [Test]
    public void ConstructorSetsPropertiesCorrectly()
    {
        var point = new PointI(3, 7);
        
        Assert.That(point.X, Is.EqualTo(3));
        Assert.That(point.Y, Is.EqualTo(7));
    }

    [Test]
    public void ConstructorHandlesZeroValues()
    {
        var point = new PointI(0, 0);
        
        Assert.That(point.X, Is.EqualTo(0));
        Assert.That(point.Y, Is.EqualTo(0));
    }

    [Test]
    public void ConstructorHandlesNegativeValues()
    {
        var point = new PointI(-5, -2);
        
        Assert.That(point.X, Is.EqualTo(-5));
        Assert.That(point.Y, Is.EqualTo(-2));
    }

    [Test]
    public void PropertiesAreMutable()
    {
        var point = new PointI(1, 2);
        
        point.X = 5;
        point.Y = 6;
        
        Assert.That(point.X, Is.EqualTo(5));
        Assert.That(point.Y, Is.EqualTo(6));
    }
}
