using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

	private Vector3 result;

	// Use this for initialization
	void Start () {
		result = Mathf.Cos (Mathf.PI/4) * Vector3.right + Mathf.Sin (Mathf.PI/4) * Vector3.forward;
        Debug.Log(result);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine (Vector3.zero, Vector3.right);
		Gizmos.DrawLine (Vector3.zero, result);
	}
}
