using UnityEngine;
using System.Collections;

public class AISimpleChase : MonoBehaviour {

	public GameObject target;

	Rigidbody unitBody;
	Unit unitScript;
	Rigidbody targetBody;
	NavMeshAgent navAgent;

	// Use this for initialization
	void Start () {
		unitBody = GetComponentInChildren<Rigidbody>();
		unitScript = GetComponentInChildren<Unit>();
		targetBody = target.GetComponentInChildren<Rigidbody>();
		navAgent = GetComponentInChildren<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 pos = (targetBody.transform.position - unitBody.transform.position).normalized;
		//unitBody.velocity = pos * unitScript.moveSpeed;

		navAgent.SetDestination(targetBody.position);
	}
}
