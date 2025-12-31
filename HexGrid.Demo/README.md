# HexGrid.Demo

An interactive Blazor WebAssembly demo showcasing the capabilities of the HexGrid.Lib library.

## 🚀 Running the Demo

### Prerequisites
- .NET 10.0 SDK or later

### Run Locally

```bash
dotnet run --project HexGrid.Demo/HexGrid.Demo.csproj
```

Then navigate to `http://localhost:5298` in your browser.

### Build for Production

```bash
dotnet publish HexGrid.Demo/HexGrid.Demo.csproj -c Release
```

The output will be in `HexGrid.Demo/bin/Release/net10.0/browser-wasm/publish/wwwroot/`.

## 📚 Features

### Interactive Demo Page
- **Multiple Grid Types**: Rectangular, Hexagonal, Triangular, and Parallelogram grids
- **Orientation Toggle**: Switch between Pointy-Top and Flat-Top orientations
- **Dynamic Parameters**: Adjust grid size, hex size, and other parameters in real-time
- **Visual Feedback**: Click on any hex to highlight it and see its coordinates
- **Coordinate Display**: Toggle visibility of axial coordinates (Q, R)

### Code Examples Page
- Comprehensive code samples demonstrating common use cases:
  - Creating basic rectangular grids
  - Generating hexagonal grids
  - Coordinate conversion (hex ↔ pixel)
  - Finding hex neighbors and calculating distances
  - Working with triangular grids
  - Getting polygon corners for rendering

## 🎨 Technology Stack

- **Blazor WebAssembly**: Client-side web UI framework
- **Bootstrap 5**: UI styling and components
- **SVG**: Vector graphics for hex rendering
- **HexGrid.Lib**: The core hexagonal grid library

## 📖 Using the Demo

### Interactive Demo

1. Select a grid type from the dropdown
2. Adjust parameters using the sliders
3. Toggle orientation between Pointy and Flat top
4. Click on any hex to select it
5. Watch the grid regenerate in real-time

### Understanding Coordinates

The demo displays **Axial Coordinates** for each hex:
- **Q**: Column coordinate (horizontal offset)
- **R**: Row coordinate (vertical offset)
- **S**: Cube coordinate (derived: S = -Q - R)

## 🔧 Project Structure

```
HexGrid.Demo/
├── Components/
│   └── HexGridVisualization.razor   # Reusable hex grid SVG renderer
├── Pages/
│   ├── Home.razor                    # Interactive demo
│   └── Examples.razor                # Code examples
├── Layout/
│   └── NavMenu.razor                 # Navigation menu
├── wwwroot/
│   └── css/
│       └── hexgrid-demo.css         # Custom styling
└── _Imports.razor                    # Global using directives
```

## 🎯 Key Components

### HexGridVisualization Component

A reusable Blazor component that renders hex grids using SVG:

```razor
<HexGridVisualization 
    Grid="@myGrid" 
    Width="900" 
    Height="650" 
    ShowCoordinates="true"
    HexColor="#2196F3"
    StrokeColor="#1976D2" />
```

**Parameters:**
- `Grid`: The HexGrid object to render
- `Width`/`Height`: SVG viewport dimensions
- `ShowCoordinates`: Toggle coordinate labels
- `HexColor`: Fill color for hexagons
- `StrokeColor`: Border color for hexagons

## 💡 Tips

- **Performance**: For large grids, consider disabling coordinate display
- **Responsiveness**: The demo works on mobile devices, though larger grids are best viewed on desktop
- **Customization**: Edit the CSS files to change colors and styling
- **Learning**: Use the Examples page as a quick reference for common patterns

## 📝 License

This demo is part of the HexGrid project and follows the same license.

## 🔗 Related

- [HexGrid.Lib Documentation](../README.md)
- [Main Repository](../)
