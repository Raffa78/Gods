using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour {

	public float speed = 10.0f;
	// Use this for initialization
	void Start () {
		//dagger has 8 sprites
		GetComponent<Animator>().SetFloat("CycleOffset", Random.value * 8.0f);
		GetComponent<Rigidbody2D>().velocity = transform.right * speed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		FXGenerator.Hit(transform.position);
		collision.GetComponentInParent<RunningCharacter>().ApplyDamage(1.0f);
		Destroy(gameObject);
	}
}
