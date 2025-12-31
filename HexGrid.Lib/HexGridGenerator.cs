namespace HexGrid.Lib.Models;
using Coordinates;
using Layout;

public static class HexGridGenerator
{
    public static AxialHexCoordinate[][] GenerateRectangularGrid(GridLayout layout, int width, int height, AxialHexCoordinate? origin = null)
    {
        var grid = new AxialHexCoordinate[height][];
        var originCoord = origin ?? new AxialHexCoordinate(0, 0);
        for (var r = 0; r < height; r++)
        {
            grid[r] = new AxialHexCoordinate[width];
            for (var q = 0; q < width; q++)
            {
                grid[r][q] = new AxialHexCoordinate(q + originCoord.Q, r + originCoord.R);
            }
        }
        return grid;
    }

    public static AxialHexCoordinate[][] GenerateRectangularOffsetGrid(GridLayout layout, int width, int height, OffsetHexCoordinateType offsetType, PointI? origin = null)
    {
        var grid = new AxialHexCoordinate[height][];
        var originCoord = origin ?? new PointI(0, 0);
        for (var row = 0; row < height; row++)
        {
            grid[row] = new AxialHexCoordinate[width];
            for (var col = 0; col < width; col++)
            {
                var offsetCoord = new OffsetHexCoordinate(col, row);
                var axialCoord = offsetCoord.ToAxial(offsetType);
                grid[row][col] = new AxialHexCoordinate(axialCoord.Q + originCoord.X, axialCoord.R + originCoord.Y);
            }
        }
        return grid;
    }

    public static AxialHexCoordinate[][] GenerateHexagonalGrid(GridLayout layout, int radius, AxialHexCoordinate? origin = null)
    {
        var diameter = radius * 2 + 1;
        var grid = new AxialHexCoordinate[diameter][];
        var originCoord = origin ?? new AxialHexCoordinate(0, 0);
        for (var r = -radius; r <= radius; r++)
        {
            var rowIndex = r + radius;
            var qMin = Math.Max(-radius, -r - radius);
            var qMax = Math.Min(radius, -r + radius);
            var rowWidth = qMax - qMin + 1;
            grid[rowIndex] = new AxialHexCoordinate[rowWidth];
            for (var q = qMin; q <= qMax; q++)
            {
                var colIndex = q - qMin;
                grid[rowIndex][colIndex] = new AxialHexCoordinate(q + originCoord.Q, r + originCoord.R);
            }
        }
        return grid;
    }

    public static AxialHexCoordinate[][] GenerateTriangularGrid(GridLayout layout, int size, bool invert=false, AxialHexCoordinate? origin = null)
    {
        var originCoord = origin ?? new AxialHexCoordinate(0, 0);

        var grid = layout.Orientation.IsPointy
            ? (invert ? GeneratePointyTriangleInverted(size, originCoord) : GeneratePointyTriangle(size, originCoord))
            : (invert ? GenerateFlatTriangleInverted(size, originCoord) : GenerateFlatTriangle(size, originCoord));
        
        return grid;
    }

    public static AxialHexCoordinate[][] GenerateParallelogramGrid(GridLayout layout, int width, int height, ParallelogramOrientation orientation, AxialHexCoordinate? origin = null)
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

    public static AxialHexCoordinate[][] GenerateWorldMap(GridLayout layout, int WorldSize, AxialHexCoordinate? origin = null)
    {
        var originCoord = origin ?? new AxialHexCoordinate(0, 0);
        var z = WorldSize - 1;
        
        // Collect all triangles from all rows
        List<AxialHexCoordinate[][]> allTriangles = [];
        
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
    
    private static AxialHexCoordinate[][] MergeTrianglesIntoGrid(List<AxialHexCoordinate[][]> triangles)
    {
        // Use HashSet to track unique hexes
        var hexSet = new HashSet<AxialHexCoordinate>();
        var hexList = new List<AxialHexCoordinate>();
        
        // Collect all unique hexes from all triangles
        foreach(var triangle in triangles)
        {
            foreach(var row in triangle)
            {
                foreach(var hex in row)
                {
                    if (hexSet.Add(hex))
                    {
                        hexList.Add(hex);
                    }
                }
            }
        }
        
        // Group hexes by Q coordinate and sort
        var groupedByQ = hexList
            .GroupBy(h => h.Q)
            .OrderBy(g => g.Key)
            .ToArray();
        
        // Build the final 2D array
        var grid = new AxialHexCoordinate[groupedByQ.Length][];
        for (var i = 0; i < groupedByQ.Length; i++)
        {
            grid[i] = groupedByQ[i].OrderBy(h => h.R).ToArray();
        }
        
        return grid;
    }

    private static AxialHexCoordinate[][] GenerateQRParallelogram(int width, int height, AxialHexCoordinate origin)
    {
        var grid = new AxialHexCoordinate[height][];
        for (var r = 0; r < height; r++)
        {
            grid[r] = new AxialHexCoordinate[width];
            for (var q = 0; q < width; q++)
            {
                grid[r][q] = new AxialHexCoordinate(q + origin.Q, r + origin.R);
            }
        }
        return grid;
    }

    private static AxialHexCoordinate[][] GeneratePointyTriangle(int size, AxialHexCoordinate origin)
    {
        var grid = new AxialHexCoordinate[size][];
        for(var q = 0; q < size; q++)
        {
            grid[q] = new AxialHexCoordinate[size - q];
            for(var r = 0; r < size - q; r++)
            {
                grid[q][r] = new AxialHexCoordinate(q + origin.Q, r + origin.R);
            }
        }
        return grid;
    }

    private static AxialHexCoordinate[][] GeneratePointyTriangleInverted(int size, AxialHexCoordinate origin)
    {
        
        var grid = new AxialHexCoordinate[size][];

        for(var q = 0; q < size; q++)
        {
            grid[q] = new AxialHexCoordinate[q+1];
            for(var r = 0; r >= -q; r--)
            {
                var _r = Math.Abs(r);
                var hex = new AxialHexCoordinate(q + origin.Q, r + origin.R);
                grid[q][_r] = hex;
            }
        }
        return grid;
    }

    private static AxialHexCoordinate[][] GenerateFlatTriangle(int size, AxialHexCoordinate origin)
    {
        var grid = new AxialHexCoordinate[size][];
        for (var r = 0; r < size; r++)
        {
            grid[r] = new AxialHexCoordinate[size - r + 1];
            for (var q = 0; q <= size - r; q++)
            {
                grid[r][q] = new AxialHexCoordinate(q + origin.Q, r + origin.R);
            }
        }
        return grid;
    }

    private static AxialHexCoordinate[][] GenerateFlatTriangleInverted(int size, AxialHexCoordinate origin)
    {
        var grid = new AxialHexCoordinate[size][];
        for (var q = 0; q < size; q++)
        {
            grid[q] = new AxialHexCoordinate[size - q + 1];
            for (var r = 0; r < size - q; r++)
            {
                grid[q][r] = new AxialHexCoordinate(q + origin.Q, (size - q - r) + origin.R);
            }
        }
        return grid;
    }

    private static AxialHexCoordinate[][] GenerateSQParallelogram(int width, int height, AxialHexCoordinate origin)
    {
        var grid = new AxialHexCoordinate[height][];
        for (var s = 0; s < height; s++)
        {
            grid[s] = new AxialHexCoordinate[width];
            for (var q = 0; q < width; q++)
            {
                var cube = new CubHexCoordinate(q + origin.Q, -q - s + origin.R, s);
                grid[s][q] = cube.ToAxial();
            }
        }
        return grid;
    }

    private static AxialHexCoordinate[][] GenerateRSParallelogram(int width, int height, AxialHexCoordinate origin)
    {
        var grid = new AxialHexCoordinate[height][];
        for (var r = 0; r < height; r++)
        {
            grid[r] = new AxialHexCoordinate[width];
            for (var s = 0; s < width; s++)
            {
                var cube = new CubHexCoordinate( -r - s + origin.Q, r + origin.R, s);
                grid[r][s] = cube.ToAxial();
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