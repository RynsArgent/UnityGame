using UnityEngine;
using System.Collections;

public class ChangeColour : MonoBehaviour {
	Renderer renderBody;
	Rigidbody unitBody;
	Unit unitScript;

	// Use this for initialization
	void Start () {
		renderBody = GetComponentInChildren<Renderer>();
		unitBody = GetComponentInChildren<Rigidbody>();
		unitScript = GetComponentInChildren<Unit>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R))
			renderBody.material.color = Color.red;
		if (Input.GetKeyDown(KeyCode.G))
			renderBody.material.color = Color.green;
		if (Input.GetKeyDown(KeyCode.B))
			renderBody.material.color = Color.blue;

		unitBody.velocity = Vector3.zero;
		if (Input.GetKey (KeyCode.W))
			unitBody.velocity += new Vector3(0f, 0f, unitScript.moveSpeed);
		if (Input.GetKey (KeyCode.A))
			unitBody.velocity += new Vector3(-unitScript.moveSpeed, 0f, 0f);
		if (Input.GetKey (KeyCode.S))
			unitBody.velocity += new Vector3(0f, 0f, -unitScript.moveSpeed);
		if (Input.GetKey (KeyCode.D))
			unitBody.velocity += new Vector3(unitScript.moveSpeed, 0f, 0f);
	}
}
