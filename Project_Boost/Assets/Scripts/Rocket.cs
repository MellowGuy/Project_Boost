using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
	[SerializeField] float thrustSpeed;
	[SerializeField] float rotationSpeed;

	Rigidbody rb;
	AudioSource audioSource;
	ShipMetricDisplay metrics;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		metrics = GameObject.Find("Stats").GetComponent<ShipMetricDisplay>();
	}

	// Update is called once per frame
	void Update()
	{
		GetInput();
		Vector3 shipVelocity = rb.velocity;
		float shipAngle = transform.rotation.z;
		metrics.SetStats(shipVelocity, shipAngle);

	}

	void OnCollisionEnter(Collision collision)
	{
		switch (collision.gameObject.tag)
		{
			case "Friendly":
				Debug.Log("Friendly");
				break;
			case "Finish":
				Debug.LogWarning("Finished!");
				SceneManager.LoadScene(1);
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
