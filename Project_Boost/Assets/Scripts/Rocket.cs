using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
	[SerializeField] float thrustSpeed;
	[SerializeField] float rotationSpeed;

	Rigidbody rb;
	AudioSource audioSource;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		GetInput();
	}

	void OnCollisionEnter(Collision collision)
	{
		switch (collision.gameObject.tag)
		{
			case "Friendly":
				Debug.Log("Friendly");
				break;
			case "Fuel":
				Debug.Log("Fuel");
				break;
			case "Death":
				Debug.Log("Dead");
				break;
			default:
				Debug.LogWarning("DEATH");
				break;
		}
	}


	private void GetInput()
	{
		Thrust();
		Rotate();
	}

	private void Thrust()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			//thrusting
			rb.AddRelativeForce(Vector3.up);
			if (!audioSource.isPlaying)
			{
				audioSource.Play();
			}
		}
		else
		{
			audioSource.Stop();
		}
	}

	private void Rotate()
	{

		rb.freezeRotation = true; // take manual control of rotation
		if (Input.GetKey(KeyCode.A))
		{
			//rotate left
			transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

		}
		else if (Input.GetKey(KeyCode.D))
		{
			//rotate right
			transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);

		}

		rb.freezeRotation = false; //resume physics control of rotation
	}
}
