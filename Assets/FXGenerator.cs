using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXGenerator : MonoBehaviour {

	static FXGenerator instance;

	public GameObject hit;
	public GameObject groundExplosion;

	// Use this for initialization
	void Start () {
		instance = this;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void Hit(Vector3 position)
	{
		Instantiate(instance.hit, position, Quaternion.identity);
	}

	public static void GroundExplosion(Vector3 position)
	{
		Instantiate(instance.groundExplosion, position, Quaternion.identity);
	}
}
