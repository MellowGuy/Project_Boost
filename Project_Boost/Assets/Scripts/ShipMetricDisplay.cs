using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipMetricDisplay : MonoBehaviour
{
	private Vector3 shipVelocity;
	private float shipAngle;
	Text screenText;

	// Use this for initialization
	void Start()
	{
		screenText = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update()
	{
		screenText.text = @"Velocity: " + shipVelocity.ToString() +
			"\nAngle: " + shipAngle;
	}

	public void SetStats(Vector3 _shipVelocity, float _shipAngle)
	{
		shipVelocity = _shipVelocity;
		shipAngle = _shipAngle;
	}
}
