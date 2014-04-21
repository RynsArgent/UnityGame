using UnityEngine;
using System.Collections;

public class GridGen : MonoBehaviour {
	GridMap grid;

	// Use this for initialization
	void Start () {
		grid = new GridMap();
		grid.Init();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
