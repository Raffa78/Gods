using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSounds : MonoBehaviour {

	public AudioClip footStep;
	public AudioClip jump;
	public AudioClip shoot;
	public AudioClip landed;
	
	//called by animation event
	public void PlayFootStep()
	{
		AudioSource.PlayClipAtPoint(footStep, transform.position);
	}
	//called by animation event
	public void PlayShoot()
	{
		AudioSource.PlayClipAtPoint(shoot, transform.position);
	}

	//called by message
	public void Jumped()
	{
		AudioSource.PlayClipAtPoint(jump, transform.position);
	}
	//called by message
	public void Landed()
	{
		AudioSource.PlayClipAtPoint(landed, transform.position);
	}
	
}
