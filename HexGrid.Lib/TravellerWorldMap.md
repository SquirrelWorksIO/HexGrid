# Traveller World Map

The world map for the Traveller RPG (Mongoose, 2nd Edition) is an unwrapped icosahedron (d20).

It features 20 triangles composed of pointy-topped hexagons in three rows.

The first row is 5 upward pointing triangles. The second row is 5 each of upward and downward facing triangles (10 total)

The last row is 5 downward facing triangles.

There are two general approaches to the map:

- Fixed number of hexagons per triangle, variable kilometers per hexagon
- Fixed kilometers per hexagonm, variable hexagons per triangle

Each triangle is defined as

```txt
  B
 / \
A---C
```

Where `A` is the origin hex of the triangle, `B`, is the top (or bottom) corner, and `C` is the rightmost corner.

## Triangle Ids

For ease of reference, we will number each triangle numerically and in order.

- Row 1: 1-5
- Row 2:
  - Upwards: 6, 8, 10, 12, 14
  - Downwards: 7, 9, 11, 13, 15
- Row 3: 16 - 20

## Fixed Hexagons (Method 1)

The standard world map is a consistent 7 hexagons per side of each triangles. For this scenario, the width of a hexagon is defined as

$Hex.Width = \frac{World.Size \times 1,600 \times \pi}{35}$

That is roughly 1,000 km per hex on a Size 7 world.

## Fixed Width (Method 2)

With this method, the width of the hexagon is fixed at 1,000 km per hex and the number of hexagons per side of the triangle is equal to the world size.

## Building The Grid

Assuming no additional offset, that indexing starts at 0 (such that origin is at [0, 0, 0]), we can deduce a few general properties of the hexagon coordinates of the triangles.

### Assumption 1

When determining where a triangle begins where it shares a corner with another triangle, there is a consistent offset of `Size - 1`. This is due to each triangle being `Size` hexagons to a side, and the 1st hexagon of the next triangle starts at the last hexagon of the first triangle (hence the `-1`)

We will define this constant as

- $Z = Size - 1$

#### Result

Each corner will be some combination of $(\pm Z, \pm Z)$ offset of `A`

- Corner `B`
  - Upwards Triangle: $(A.q + Z, A.r - Z )$
  - Downwards Triange: $(A.q, A.r + Z)$
- Corner `C`: $(A.q + Z, A.r)$

Conversely, we can derive `A` from either `B` or `C`

- From Corner `B`
  - Upwards Triangle: $(B.q - Z, B.r + Z)$
  - Downards Triange: $(B.q, B.r - Z)$
- From Corner `C`: $(B.q - Z, B.r)$

### Assumption 2

Corner `B` of Triangle 1 (T1) will be located at `(Z, 0)`

#### Consequences

$$\begin{align}
T1.A & = (T1.B.q - Z, T1.B.r + z)\\
& = (Z - Z, 0 + Z)\\
& = (0, Z)
\end{align}$$

$$\begin{align}
T6.B & = T1.A\\
T6.A & = (T1.B.q - Z, T1.B.r + Z)\\
& = (0 - Z, Z + Z)\\
& = (-Z, 2Z)
\end{align}$$

$$\begin{align}
T7.A & = T1.A\\
T16.A & = T6.A
\end{align}$$

With this we can compute the origins of all 20 triangles:

|Triangle|Origin (`A`)|
|-|-|
|1| (0, Z) |
|2| (Z, Z) |
|3| (2Z, Z) |
|4| (3Z, Z) |
|5| (4Z, Z) |
|6| (-Z, 2Z) |
|7| (0, Z) |
|8| (0, 2Z) |
|9| (Z, Z) |
|10| (Z, 2Z) |
|11| (2Z, Z) |
|12| (2Z, 2Z) |
|13| (3Z, Z) |
|14| (3Z, 2Z) |
|15| (4Z, Z) |
|16| (-Z, 2Z) |
|17| (0, 2Z) |
|18| (Z, 2Z) |
|19| (2Z, 2Z) |
|20| (3Z, 2Z) |
