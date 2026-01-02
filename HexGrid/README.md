# Hexagon Coordinates

Hexagonal coordinates are used in many places, particularly in games. Many computer RTS games, board games, and famously the Traveller RPG, use hexagonal coordinates for various map applications.

## Hexagonal Coordinate System

Hexagonal Coordinates can be represented many ways, but this library uses the Cube approach, and the associated Axial and Offset systems.

### Cube Hex Coordinates

The Cube hexagonal coordinate system is so called because it treats hexagons as 3d cubes in visualization (see [RedBlogGames - Cube Hex](https://www.redblobgames.com/grids/hexagons/#coordinates-cube)).

With a 3d cube, you have an x axis, y axis, and z axis. Traditionally (ignoring video game conventions) the x axis corresponds to left/right, the y axis corresponds to forward/back, and the z axis corresponds to up/down.

With Cube hexagonal coordinates, we invert the z axis, and then re-label the axis to avoid confusion:

- x => q
- y => r
- z => s

Additionally, Cube coordinates have an inviolable property that `q + r + s = 0`. You will never see Cube coordinates of `(1, 1, 1)`, but `(1, 0, -1)` would be fine.

### Axial Coordinates

Given that `q+r+s = 0`, we can compute the 3rd coordinate value from any other pair. This gives rise to the Axial coordinate system, where we only properly track `q` and `r`, and leave `s` to be computed on demand as `s = -q-r`.

### Offset Coordinates

Offset coordinates are what people are generally most familiar with. You have a row and a column value.

The trouble with Offset Coordinates is that they are very difficult to deal with for a computer. Humans develop intuitions and can physically count hexes along a path, but mathematically that becomes difficult.

Fortunately, conversion between Offset and Cube or Axial is fairly straightforward. Typically, if coordinates are being displayed, they'll be Offset, but the actual calculations and work being done will use Axial / Cube.

Unfortunately, the conversion changes based on

- Hex orientation (are they pointy topped or flat topped?)
- Offset (even, odd - is it every even column nudged, or every odd column nudged)

Using the offset and orientation, we have four flavors of offsets:

- Flat Topped
  - Even Q (each hex with an even `q` coordinate is nudged "down")
  - Odd Q (each hex with an odd `q` coordinate is nudged "down")
- Pointy Topped
  - Even R (each hex with an even `r` coordinate is nudged "right")
  - Odd R (each hex with an odd `r` coordinate is nudged "right")

#### Converions

- Offset Constants
  - `EVEN = 1`
  - `ODD = -1`

##### Flat Top (Q Offset)

- QOffsetToAxial(hex, offset)
  - $q = hex.col$
  - $r = hex.row - \lfloor \frac{hex.col + offset * (hex.col \& 1)}{2} \rfloor$
- AxialToQOffset(hex, offset)
  - $col = hex.q$
  - $row = hex.r + \lfloor \frac{hex.col + offset * (hex.col \& 1)}{2} \rfloor$

##### Pointy Top (R Offset)

- ROffsetToAxial(hex, offset)
  - $q = hex.col - \lfloor \frac{hex.row + offset * (hex.row \& 1)}{2} \rfloor$
  - $r = hex.row$
- AxialToROffset(hex, offset)
  - $col = hex.q + \lfloor \frac{hex.r + offset * (hex.r \& 1)}{2} \rfloor$
  - $row = hex.r$
