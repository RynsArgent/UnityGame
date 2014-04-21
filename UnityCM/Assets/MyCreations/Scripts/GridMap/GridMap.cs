using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#region GridPoint
/// <summary>
/// GridPoint Class
/// Point class definition for discrete coordinates for grid.
/// </summary>
class GridPoint
{
	private int x;
	private int z;

	public int X
	{
		get { return x; }
		set { x = value; }
	}
	
	public int Z
	{
		get { return z; }
		set { z = value; }
	}

	public GridPoint(int x, int z)
	{
		this.x = x;
		this.z = z;
	}
}
#endregion

#region GridDirection
enum Direction { 
	DIRECTION_NONE,
	DIRECTION_NORTHWEST,
	DIRECTION_NORTH,
	DIRECTION_NORTHEAST,
	DIRECTION_EAST,
	DIRECTION_SOUTHEAST,
	DIRECTION_SOUTH,
	DIRECTION_SOUTHWEST,
	DIRECTION_WEST
};
#endregion

#region GridCell
/// <summary>
/// GridCell Class
/// contains information at a specific cell on the grid
/// </summary>
enum GridCellType { NONE, GROUND, WALL };
class GridCell
{
	private GridCellType cellType;

	public GridCellType CellType
	{
		get { return cellType; }
		set { cellType = value; }
	}

	public GridCell(GridCellType type)
	{
		cellType = type;
	}
}
#endregion

#region GridPath
/// <summary>
/// GridPath Class
/// contains a list of GridPoints which map a path on the GridMap
/// </summary>
class GridPath : List<GridPoint>
{
}
#endregion

#region GridMap
/// <summary>
/// Grid - main class that translates world coordinates to index coordinates
/// </summary>
class GridMap 
{
	#region DataMembers
	////////////////
	/// Data Members
	///
	private int minX;
	private int maxX;
	private int minZ;
	private int maxZ;
	private List<List<GridCell> > grid;
	#endregion

	#region GettersSetters
	/////////////////////
	/// Getters & Setters
	///

	// Returns the discrete size in a certain dimension
	public int Width
	{
		get { return maxX - minX + 1; }
	}
	public int Depth
	{
		get { return maxZ - minZ + 1; }
	}

	// Converts the game space coordinates into grid index coordinates
	public GridPoint Vector3ToGridPoint(Vector3 transform)
	{
		return new GridPoint((int)System.Math.Round(transform.x) - minX, (int)System.Math.Round(transform.z) - minZ);
	}
	
	// Returns the direction from point p to point q
	public Direction GetDirection(GridPoint p, GridPoint q)
	{
		if (p.X < q.X)
		{
			if (p.Z < q.Z)
				return Direction.DIRECTION_NORTHEAST;
			else if (p.Z > q.Z)
				return Direction.DIRECTION_SOUTHEAST;
			else
				return Direction.DIRECTION_EAST;
		}
		else if (p.X > q.X)
		{
			if (p.Z < q.Z)
				return Direction.DIRECTION_NORTHWEST;
			else if (p.Z > q.Z)
				return Direction.DIRECTION_SOUTHWEST;
			else
				return Direction.DIRECTION_WEST;
		}
		else
		{
			if (p.Z < q.Z)
				return Direction.DIRECTION_NORTH;
			else if (p.Z > q.Z)
				return Direction.DIRECTION_SOUTH;
			else
				return Direction.DIRECTION_NONE;
		}
	}
	
	// Returns adjacent points that are within the grid.
	public List<GridPoint> GetAvailableAdjacentPoints(GridPoint p)
	{
		List<GridPoint> ret = new List<GridPoint>();
		GridPoint np;
		np = new GridPoint(p.X - 1, p.Z - 1);
		if (withinBounds(np)) ret.Add(np);
		np = new GridPoint(p.X - 1, p.Z);
		if (withinBounds(np)) ret.Add(np);
		np = new GridPoint(p.X - 1, p.Z + 1);
		if (withinBounds(np)) ret.Add(np);
		np = new GridPoint(p.X, p.Z - 1);
		if (withinBounds(np)) ret.Add(np);
		np = new GridPoint(p.X, p.Z + 1);
		if (withinBounds(np)) ret.Add(np);
		np = new GridPoint(p.X + 1, p.Z - 1);
		if (withinBounds(np)) ret.Add(np);
		np = new GridPoint(p.X + 1, p.Z);
		if (withinBounds(np)) ret.Add(np);
		np = new GridPoint(p.X + 1, p.Z + 1);
		if (withinBounds(np)) ret.Add(np);
		return ret;
	}

	// Returns adjacent points that are within the grid. This also optimized
	// through additional information given Direction. This is particularly useful 
	// for searches which can guarantee going backwards is never a better route

	// For example: If the user goes north, there is no need to return 
	// any direction to the south. 
	public List<GridPoint> GetAvailableAdjacentPoints(GridPoint p, Direction dir)
	{
		List<GridPoint> ret = new List<GridPoint>();
		GridPoint np;
		switch (dir)
		{
		case Direction.DIRECTION_NONE:
			np = new GridPoint(p.X - 1, p.Z - 1);
			if (withinBounds(np)) ret.Add(np);
			np = new GridPoint(p.X - 1, p.Z);
			if (withinBounds(np)) ret.Add(np);
			np = new GridPoint(p.X - 1, p.Z + 1);
			if (withinBounds(np)) ret.Add(np);
			np = new GridPoint(p.X, p.Z - 1);
			if (withinBounds(np)) ret.Add(np);
			np = new GridPoint(p.X, p.Z + 1);
			if (withinBounds(np)) ret.Add(np);
			np = new GridPoint(p.X + 1, p.Z - 1);
			if (withinBounds(np)) ret.Add(np);
			np = new GridPoint(p.X + 1, p.Z);
			if (withinBounds(np)) ret.Add(np);
			np = new GridPoint(p.X + 1, p.Z + 1);
			if (withinBounds(np)) ret.Add(np);
			break;
		case Direction.DIRECTION_NORTHWEST:
			np = new GridPoint(p.X - 1, p.Z);
			if (withinBounds(np)) ret.Add(np);
			np = new GridPoint(p.X - 1, p.Z + 1);
			if (withinBounds(np)) ret.Add(np);
			np = new GridPoint(p.X, p.Z + 1);
			if (withinBounds(np)) ret.Add(np);
			break;
		case Direction.DIRECTION_NORTH:
			np = new GridPoint(p.X, p.Z + 1);
			if (withinBounds(np)) ret.Add(np);
			break;
		case Direction.DIRECTION_NORTHEAST:
			np = new GridPoint(p.X, p.Z + 1);
			if (withinBounds(np)) ret.Add(np);
			np = new GridPoint(p.X + 1, p.Z + 1);
			if (withinBounds(np)) ret.Add(np);
			np = new GridPoint(p.X + 1, p.Z);
			if (withinBounds(np)) ret.Add(np);
			break;
		case Direction.DIRECTION_EAST:
			np = new GridPoint(p.X + 1, p.Z);
			if (withinBounds(np)) ret.Add(np);
			break;
		case Direction.DIRECTION_SOUTHEAST:
			np = new GridPoint(p.X + 1, p.Z);
			if (withinBounds(np)) ret.Add(np);
			np = new GridPoint(p.X + 1, p.Z - 1);
			if (withinBounds(np)) ret.Add(np);
			np = new GridPoint(p.X, p.Z - 1);
			if (withinBounds(np)) ret.Add(np);
			break;
		case Direction.DIRECTION_SOUTH:
			np = new GridPoint(p.X, p.Z - 1);
			if (withinBounds(np)) ret.Add(np);
			break;
		case Direction.DIRECTION_SOUTHWEST:
			np = new GridPoint(p.X, p.Z - 1);
			if (withinBounds(np)) ret.Add(np);
			np = new GridPoint(p.X - 1, p.Z - 1);
			if (withinBounds(np)) ret.Add(np);
			np = new GridPoint(p.X - 1, p.Z);
			if (withinBounds(np)) ret.Add(np);
			break;
		case Direction.DIRECTION_WEST:
			np = new GridPoint(p.X - 1, p.Z);
			if (withinBounds(np)) ret.Add(np);
			break;
		}
		return ret;
	}
	#endregion

	#region Constructors
	////////////////
	/// Constructors
	/// 
	public GridMap()
	{
		minX = int.MaxValue;
		maxX = int.MinValue;
		minZ = int.MaxValue;
		maxZ = int.MinValue;
		grid = new List<List<GridCell> >();
	}
	#endregion

	#region PrivateMembers
	///////////////////////////
	// Private Helper Functions
	//

	private bool withinBounds(GridPoint p)
	{
		return p.X >= 0 && p.X < Width && p.Z >= 0 && p.Z < Depth;
	}

	// Reads in a list of game objects and updates the min-max bounds of the grid
	private void updateBounds(List<GameObject> objs)
	{
		foreach (GameObject obj in objs)
		{
			int ox = (int)System.Math.Round(obj.transform.position.x);
			int oz = (int)System.Math.Round(obj.transform.position.z);
			if (ox < minX)
				minX = ox;				
			if (ox > maxX)
				maxX = ox;
			if (oz < minZ)
				minZ = oz;				
			if (oz > maxZ)
				maxZ = oz;
		}
	}

	private void updateGrid(List<GameObject> objs, GridCellType type)
	{
		foreach (GameObject obj in objs)
		{
			GridPoint p = Vector3ToGridPoint(obj.transform.position);
			grid[p.X][p.Z].CellType = type;
		}
	}
	#endregion
	
	#region UnityFunctions
	///////////////////////////
	// Functions that use Unity
	//

	public void Init()
	{
		minX = int.MaxValue;
		maxX = int.MinValue;
		minZ = int.MaxValue;
		maxZ = int.MinValue;
		
		Debug.Log ("Reading Stage Data");

		// Retrieve stage game objects from scene
		List<GameObject> pathables = new List<GameObject>(GameObject.FindGameObjectsWithTag("Pathable"));
		List<GameObject> unpathables = new List<GameObject>(GameObject.FindGameObjectsWithTag("Unpathable"));
		
		Debug.Log ("Computing Bounds");

		// Determine min max bounds in space
		updateBounds(pathables);
		updateBounds(unpathables);
		
		Debug.Log ("Allocating Grid");
		
		Debug.Log ("BoundsX " + minX.ToString ());
		Debug.Log ("BoundsX " + maxX.ToString ());
		Debug.Log ("BoundsZ " + minZ.ToString ());
		Debug.Log ("BoundsZ " + maxZ.ToString ());

		// Initialize 2-D Grid 
		grid = new List<List<GridCell> >();
		int width = Width;
		int depth = Depth;
		for (int i = 0; i < width; ++i)
		{
			List<GridCell> list = new List<GridCell>();
			for (int j = 0; j < depth; ++j)
			{
				list.Add(new GridCell(GridCellType.NONE));
			}
			grid.Add(list);
		}
		
		Debug.Log ("Setting Up Grid Cells");

		// Update cell types reading in game objects
		updateGrid(pathables, GridCellType.GROUND);
		updateGrid(unpathables, GridCellType.WALL);

		Debug.Log ("Grid Initialized!");
	}
	#endregion
}
#endregion
