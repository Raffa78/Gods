using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : RunningCharacter {

	public float direction;

	protected override float GetHorizontalInput()
	{
		return direction;
	}
}
