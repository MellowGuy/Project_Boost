using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
	#region SerializeFields
	[SerializeField] float levelLoadDelay = 3f;
	[SerializeField] float thrustSpeed;
	[SerializeField] float rotationSpeed;

	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip successSound;
	[SerializeField] AudioClip deathSound;

	[SerializeField] ParticleSystem mainEngineParticles;
	[SerializeField] ParticleSystem successParticles;
	[SerializeField] ParticleSystem deathParticles;
	#endregion

	Rigidbody rb;
	AudioSource audioSource;
	ShipMetricDisplay shipMetrics;
	Collider coll;

	enum State { Alive, Dying, Transcending }
	State state = State.Alive;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		shipMetrics = GameObject.Find("Stats").GetComponent<ShipMetricDisplay>();
		coll = GetComponent<Collider>();
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
		shipMetrics.SetStats(shipVelocity, shipAngle);

		if (Debug.isDebugBuild)
		{
			GetDebugInput();
		}
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
		Invoke("LoadFirstScene", levelLoadDelay);
	}

	private void StartSuccessSequence()
	{
		Debug.Log("Success!!!");
		audioSource.Stop();
		state = State.Transcending;
		audioSource.PlayOneShot(successSound);
		successParticles.Play();
		Invoke("LoadNextScene", levelLoadDelay);
	}

	private void LoadNextScene()
	{
		//gets current and next scene index
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		int nextSceneIndex = currentSceneIndex + 1;

		if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
		{
			LoadFirstScene();
		}
		else
		{
			SceneManager.LoadScene(currentSceneIndex + 1);
		}
		Debug.LogWarning("SceneIndex:" + nextSceneIndex);
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
		rb.angularVelocity = Vector3.zero; //remove rotation due to physics

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
	}

	//adds some debug keys 'L' to LoadNextScene, 'C' to toggle collider
	private void GetDebugInput()
	{
		if (Input.GetKeyDown(KeyCode.L))
			LoadNextScene();

		if (Input.GetKeyDown(KeyCode.C))
			coll.enabled = !coll.enabled;
	}
}