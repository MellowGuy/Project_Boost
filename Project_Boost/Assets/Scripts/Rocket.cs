using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
	[SerializeField] float thrustSpeed;
	[SerializeField] float rotationSpeed;
	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip successSound;
	[SerializeField] AudioClip deathSound;

	[SerializeField] ParticleSystem mainEngineParticles;
	[SerializeField] ParticleSystem successParticles;
	[SerializeField] ParticleSystem deathParticles;

	Rigidbody rb;
	AudioSource audioSource;
	ShipMetricDisplay metrics;

	enum State { Alive, Dying, Transcending }
	State state = State.Alive;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		metrics = GameObject.Find("Stats").GetComponent<ShipMetricDisplay>();
	}

	// Update is called once per frame
	void Update()
	{//TODO somewhere stop sound on death
		if (state == State.Alive)
		{
			RespondToThrustInput();
			RespondToRotateInput();
		}
		//calculates velocity/angle and gives to metrics SetStats function.
		Vector3 shipVelocity = rb.velocity;
		float shipAngle = transform.rotation.z;
		metrics.SetStats(shipVelocity, shipAngle);
	}

	void OnCollisionEnter(Collision collision)
	{
		if (state != State.Alive) { return; }//Ignores collisions when NOT alive.

		switch (collision.gameObject.tag)
		{
			case "Friendly":
				Debug.Log("Friendly");
				break;
			case "Finish":
				StartSuccessSequence();
				break;
			case "Death":
				StartDeathSequence();
				break;
			default:
				StartDeathSequence();
				break;
		}
	}

	private void StartDeathSequence()
	{
		Debug.Log("Dying");
		audioSource.Stop();
		state = State.Dying;
		audioSource.PlayOneShot(deathSound);
		deathParticles.Play();
		Invoke("LoadFirstScene", 2f);
	}

	private void StartSuccessSequence()
	{
		Debug.Log("Success!!!");
		audioSource.Stop();
		state = State.Transcending;
		audioSource.PlayOneShot(successSound);
		successParticles.Play();
		Invoke("LoadNextScene", 2f);
	}

	private void LoadNextScene()
	{
		SceneManager.LoadScene(1); //TODO allow for more than 2 levels
	}

	private void LoadFirstScene()
	{
		SceneManager.LoadScene(0); //TODO allow for more than 2 levels
	}

	private void RespondToThrustInput()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			ApplyThrust();
		}
		else
		{
			audioSource.Stop();
			mainEngineParticles.Stop();
		}
	}

	private void ApplyThrust()
	{
		//thrusting
		rb.AddRelativeForce(Vector3.up * thrustSpeed);
		if (!audioSource.isPlaying)
		{
			audioSource.PlayOneShot(mainEngine);
		}
		if (!mainEngineParticles.isPlaying)
		{
			mainEngineParticles.Play();
		}
	}

	private void RespondToRotateInput()
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
