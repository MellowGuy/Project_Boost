using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
	[SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
	[SerializeField] float period = 2f;

	float movementFactor; // 0 for not moving, 1 for fully moved
	Vector3 startingPos;

	// Use this for initialization
	void Start()
	{
		startingPos = transform.position;

	}

	// Update is called once per frame
	void Update()
	{
		if (period <= Mathf.Epsilon)
		{
			return;
		}

		float cycles = Time.time / period; //grows continually from 0

		const float tau = Mathf.PI * 2; //rather than using 2*Pi (6.28)
		float rawSinWave = Mathf.Sin(cycles * tau); //cycles from -1 to +1

		movementFactor = rawSinWave / 2 + 0.5f; //adjust so cycles from 0 to 1 and assigns it to movementFactor
		Vector3 offset = movementVector * movementFactor; //creates offset to apply to position
		transform.position = startingPos + offset; //assigns to position
	}
}
