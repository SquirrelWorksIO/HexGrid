namespace HexGrid;
using Models.Coordinates;
using Models.Layout;

public static class HexGridGenerator
{
    public static ICollection<AxialHexCoordinate> GenerateRectangularGrid(GridLayout layout, int width, int height, OffsetHexCoordinateType? offsetType = null, AxialHexCoordinate? origin = null)
    {
        var offset = offsetType ?? (layout.Orientation.IsPointy ? OffsetHexCoordinateType.OddR : OffsetHexCoordinateType.OddQ);
        var grid = new List<AxialHexCoordinate>();
        var originCoord = origin ?? new AxialHexCoordinate(0, 0);

        
        for (var row = 0; row < height; row++)
        {
            for (var col = 0; col < width; col++)
            {
                var offsetCoord = new OffsetHexCoordinate(col, row);
                var axialCoord = offsetCoord.ToAxial(offset);
                grid.Add(new AxialHexCoordinate(axialCoord.Q + originCoord.Q, axialCoord.R + originCoord.R));
            }
        }

        return grid;
    }

    public static ICollection<AxialHexCoordinate> GenerateRectangularOffsetGrid(GridLayout layout, int width, int height, OffsetHexCoordinateType offsetType, PointI? origin = null)
    {
        var grid = new List<AxialHexCoordinate>();
        var originCoord = origin ?? new PointI(0, 0);
        for (var row = 0; row < height; row++)
        {
            for (var col = 0; col < width; col++)
            {
                var offsetCoord = new OffsetHexCoordinate(col, row);
                var axialCoord = offsetCoord.ToAxial(offsetType);
                grid.Add(new AxialHexCoordinate(axialCoord.Q + originCoord.X, axialCoord.R + originCoord.Y));
            }
        }
        return grid;
    }

    public static ICollection<AxialHexCoordinate> GenerateHexagonalGrid(GridLayout layout, int radius, AxialHexCoordinate? origin = null)
    {
        var diameter = radius * 2 + 1;
        var grid = new List<AxialHexCoordinate>();
        var originCoord = origin ?? new AxialHexCoordinate(0, 0);
        for (var r = -radius; r <= radius; r++)
        {
            var rowIndex = r + radius;
            var qMin = Math.Max(-radius, -r - radius);
            var qMax = Math.Min(radius, -r + radius);
            var rowWidth = qMax - qMin + 1;
            for (var q = qMin; q <= qMax; q++)
            {
                var colIndex = q - qMin;
                grid.Add(new AxialHexCoordinate(q + originCoord.Q, r + originCoord.R));
            }
        }
        return grid;
    }

    public static ICollection<AxialHexCoordinate> GenerateTriangularGrid(GridLayout layout, int size, bool invert=false, AxialHexCoordinate? origin = null)
    {
        var originCoord = origin ?? new AxialHexCoordinate(0, 0);

        var grid = layout.Orientation.IsPointy
            ? (invert ? GeneratePointyTriangleInverted(size, originCoord) : GeneratePointyTriangle(size, originCoord))
            : (invert ? GenerateFlatTriangleInverted(size, originCoord) : GenerateFlatTriangle(size, originCoord));
        
        return grid;
    }

    public static ICollection<AxialHexCoordinate> GenerateParallelogramGrid(GridLayout layout, int width, int height, ParallelogramOrientation orientation, AxialHexCoordinate? origin = null)
    {
        var originCoord = origin ?? new AxialHexCoordinate(0, 0);
        return orientation switch
        {
            ParallelogramOrientation.QR => GenerateQRParallelogram(width, height, originCoord),
            ParallelogramOrientation.SQ => GenerateSQParallelogram(width, height, originCoord),
            ParallelogramOrientation.RS => GenerateRSParallelogram(width, height, originCoord),
            _ => throw new ArgumentOutOfRangeException(nameof(orientation), "Invalid ParallelogramOrientation.")
        };
    }

    public static ICollection<AxialHexCoordinate> GenerateWorldMap(GridLayout layout, int WorldSize, AxialHexCoordinate? origin = null)
    {
        var originCoord = origin ?? new AxialHexCoordinate(0, 0);
        var z = WorldSize - 1;
        
        // Collect all triangles from all rows
        List<ICollection<AxialHexCoordinate>> allTriangles = [];
        
        // Row 1: 5 inverted triangles
        var t1A = new AxialHexCoordinate(originCoord.Q, originCoord.R + z);
        var t6A = new AxialHexCoordinate(originCoord.Q - z, originCoord.R + 2 * z);
        for(var i = 0; i < 5; i++)
        {
            var tOrigin = new AxialHexCoordinate(t1A.Q + i * z, t1A.R);
            var triangle = GenerateTriangularGrid(layout, WorldSize, invert: true, tOrigin);
            allTriangles.Add(triangle);
        }
        
        // Row 2: 5 normal triangles + 5 inverted triangles
        for (var i = 0; i < 5; i++)
        {
            var tOrigin = new AxialHexCoordinate(t6A.Q + i * z, t6A.R);
            var triangle = GenerateTriangularGrid(layout, WorldSize, invert: true, tOrigin);
            allTriangles.Add(triangle);
        }
        for(var i = 0; i < 5; i++)
        {
            var tOrigin = new AxialHexCoordinate(t1A.Q + (i * z), t1A.R);
            var triangle = GenerateTriangularGrid(layout, WorldSize, invert: false, tOrigin);
            allTriangles.Add(triangle);
        }

        // Row 3: 5 normal triangles
        for(var i = 0; i < 5; i++)
        {
            var tOrigin = new AxialHexCoordinate(t6A.Q + i * z, t6A.R);
            var triangle = GenerateTriangularGrid(layout, WorldSize, invert: false, tOrigin);
            allTriangles.Add(triangle);
        }
        
        // Merge all triangles into a single grid, removing duplicates
        return MergeTrianglesIntoGrid(allTriangles);
    }
    
    private static ICollection<AxialHexCoordinate> MergeTrianglesIntoGrid(List<ICollection<AxialHexCoordinate>> triangles)
    {
        // Use HashSet to track unique hexes
        var hexSet = new HashSet<AxialHexCoordinate>();
        var hexList = new List<AxialHexCoordinate>();
        
        // Collect all unique hexes from all triangles
        foreach(var triangle in triangles)
        {
            foreach(var hex in triangle)
            {
                if (hexSet.Add(hex))
                {
                    hexList.Add(hex);
                }
            }
        }
        
        // Group hexes by Q coordinate and sort
        var groupedByQ = hexList
            .GroupBy(h => h.Q)
            .OrderBy(g => g.Key)
            .ToArray();
        
        // Build the final 2D array
        var grid = new List<AxialHexCoordinate>();
        for (var i = 0; i < groupedByQ.Length; i++)
        {
            var x = groupedByQ[i].OrderBy(h => h.R).ToList();
            grid.AddRange(x);
        }
        
        return grid;
    }

    private static ICollection<AxialHexCoordinate> GenerateQRParallelogram(int width, int height, AxialHexCoordinate origin)
    {
        var grid = new List<AxialHexCoordinate>();
        for (var r = 0; r < height; r++)
        {
            for (var q = 0; q < width; q++)
            {
                grid.Add(new AxialHexCoordinate(q + origin.Q, r + origin.R));
            }
        }
        return grid;
    }

    private static ICollection<AxialHexCoordinate> GeneratePointyTriangle(int size, AxialHexCoordinate origin)
    {
        var grid = new List<AxialHexCoordinate>();
        for(var q = 0; q < size; q++)
        {
            for(var r = 0; r < size - q; r++)
            {
                grid.Add(new AxialHexCoordinate(q + origin.Q, r + origin.R));
            }
        }
        return grid;
    }

    private static ICollection<AxialHexCoordinate> GeneratePointyTriangleInverted(int size, AxialHexCoordinate origin)
    {
        
        var grid = new List<AxialHexCoordinate>();

        for(var q = 0; q < size; q++)
        {
            for(var r = 0; r >= -q; r--)
            {
                var _r = Math.Abs(r);
                var hex = new AxialHexCoordinate(q + origin.Q, r + origin.R);
                grid.Add(hex);
            }
        }
        return grid;
    }

    private static ICollection<AxialHexCoordinate> GenerateFlatTriangle(int size, AxialHexCoordinate origin)
    {
        var grid = new List<AxialHexCoordinate>();
        for (var r = 0; r < size; r++)
        {
            for (var q = 0; q < size - r; q++)
            {
                grid.Add(new AxialHexCoordinate(q + origin.Q, r + origin.R));
            }
        }
        return grid;
    }

    private static ICollection<AxialHexCoordinate> GenerateFlatTriangleInverted(int size, AxialHexCoordinate origin)
    {
        var grid = new List<AxialHexCoordinate>();
        for (var q = 0; q < size; q++)
        {
            for (var r = 0; r < size - q; r++)
            {
                grid.Add(new AxialHexCoordinate(q + origin.Q, (size - q - r) + origin.R));
            }
        }
        return grid;
    }

    private static ICollection<AxialHexCoordinate> GenerateSQParallelogram(int width, int height, AxialHexCoordinate origin)
    {
        var grid = new List<AxialHexCoordinate>();
        for (var s = 0; s < height; s++)
        {
            for (var q = 0; q < width; q++)
            {
                var cube = new CubHexCoordinate(q + origin.Q, -q - s + origin.R, s);
                grid.Add(cube.ToAxial());
            }
        }
        return grid;
    }

    private static ICollection<AxialHexCoordinate> GenerateRSParallelogram(int width, int height, AxialHexCoordinate origin)
    {
        var grid = new List<AxialHexCoordinate>();
        for (var r = 0; r < height; r++)
        {
            for (var s = 0; s < width; s++)
            {
                var cube = new CubHexCoordinate( -r - s + origin.Q, r + origin.R, s);
                grid.Add(cube.ToAxial());
            }
        }
        return grid;
    }
}

public enum ParallelogramOrientation
{
    QR,
    SQ,
    RS
}