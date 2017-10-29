using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoop : MonoBehaviour {

	AudioSource audio;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();	
	}

	float loopTime = 2.0f;
	float elapsedTime = 0.0f;

	// Update is called once per frame
	void Update () {

		elapsedTime += Time.deltaTime;

		if(elapsedTime > loopTime)
		{
			audio.Play();
			elapsedTime = 0.0f;
		}
	}
}
