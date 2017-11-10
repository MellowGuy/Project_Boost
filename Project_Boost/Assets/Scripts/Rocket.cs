using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
	public float thrustSpeed = 5f;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		GetInput();
	}

	private void GetInput()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			//thrusting
			rb.AddRelativeForce(Vector3.up);
		}

		if (Input.GetKey(KeyCode.A))
		{
			//rotate left

		}
		else if (Input.GetKey(KeyCode.D))
		{
			//rotate right

		}
	}
}
