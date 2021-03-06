//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;

#region GridSearchAStar
/////////////////////////////////////////////////
/// A-Star Algorithm on Grid Search Spaces
/// Also uses Directional information for pruning
/// 
class GridSearchAStar : GridSearch
{
	#region BaseMembers
	////////////////
	/// Base Members
	///

	// Parameter Data Members
	protected GridPoint source;
	protected GridPoint destination;

	// Helper Data Member During Execution
	protected Dictionary<int, GridState> explored;
	protected PriorityQueue<GridState> frontier;
	
	// Return Data Members
	protected GridPath path;
	#endregion

	#region GettersSetters
	/////////////////////
	/// Getters & Setters
	///

	public GridPoint Source
	{
		get { return source; }
	}
	public GridPoint Destination
	{
		get { return destination; }
	}
	public Dictionary<int, GridState> Explored
	{
		get { return explored; }
	}
	public PriorityQueue<GridState> Frontier
	{
		get { return frontier; }
	}
	public GridPath Path
	{
		get { return path; }
	}
	#endregion
	
	#region BaseConstructor
	/////////////////////
	/// Base Constructors
	///

	public GridSearchAStar(GridMap grid, GridPoint source, GridPoint destination)
		: base(grid)
	{
		this.source = source;
		this.destination = destination;

		explored = new Dictionary<int, GridState>();
		frontier = new PriorityQueue<GridState>();
		path = new GridPath();
	}
	#endregion
	
	#region MainFunctions
	//////////////////
	/// Main Functions
	/// 

	// Assigns to the path member variable the most preferred path towards the goal.
	// If no path is found, the path stores the route which reaches the location
	// that estimates the closest distance to the goal (using Heuristic)
	public void SetPath()
	{
		explored.Clear();
		frontier.Clear();
		path.Clear();

		// Initialize at starting node
		frontier.Add(new GridState(grid, null, source, destination));
		GridState cur;
		GridState tracker = null;

		// Continue until there are no nodes left to explore
		while (!frontier.IsEmpty())
		{
			cur = frontier.Peek();
			frontier.Remove();

			// Avoid re-visiting an explored node
			if (explored.ContainsKey(cur.Hash))
				continue;
			explored.Add (cur.Hash, cur);

			// Maintains estimated closest node towards the goal
			if (tracker == null || cur.Heuristic < tracker.Heuristic)
				tracker = cur;
			// Break loop if we found goal
			if (cur.Location.X == destination.X && cur.Location.Z == destination.Z)
				break;

			// Add new nodes reachable from current state
			List<GridState> nstates = cur.Expand;
			foreach (GridState next in nstates)
			{
				// Avoid re-visiting an explored node, if new node, add to frontier
				if (!explored.ContainsKey(next.Hash))
					frontier.Add(next);
			}
		}

		// Backtrack from destination node following previous pointers back to source
		// to finalize our path. Set it to forward order by reversing the generated result
		while (tracker != null)
		{
			path.Add(tracker.Location);
			tracker = tracker.Previous;
		}
		path.Reverse();
	}
	#endregion
}
#endregion
