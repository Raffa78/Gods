using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {

	public GameObject monsterPrefab;
	public float direction;

	// Use this for initialization
	void Start () {
		Instantiate(monsterPrefab, transform.position, Quaternion.identity).GetComponent<Monster>().direction = direction;

		StartCoroutine(Spawn());
	}

	IEnumerator Spawn()
	{
		while (true)
		{

			yield return new WaitForSeconds(10.0f);
			Instantiate(monsterPrefab, transform.position, Quaternion.identity).GetComponent<Monster>().direction = direction;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
