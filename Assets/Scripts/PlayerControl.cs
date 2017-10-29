using UnityEngine;
using System.Collections;

public class PlayerControl : RunningCharacter
{
	protected override bool ShouldJump()
	{
		return Input.GetButtonDown("Jump");
	}
	protected override bool ShouldFire()
	{
		return Input.GetButtonDown("Fire1");
	}
	protected override bool ShouldDropDown()
	{
		return Input.GetAxis("Vertical") < 0;
	}
	protected override float GetHorizontalInput()
	{
		return Input.GetAxis("Horizontal");
	}
	protected override void WillShoot()
	{
		anim.SetTrigger("Shoot");
		shootCooldownExpired = false;
		shooting = true;
		return;
	}
	
	//Called by animator event
	public override void Shoot()
	{
		float w = 0;
		if (!facingRight)
			w = 1.0f;

		GameObject dagger = Instantiate(daggerPrefab, transform.Find("shootPoint").position, new Quaternion(0, w, 0, 0));

		shootCooldownExpired = true;
	}

	//Called by animator event
	public void ShootEnd()
	{
		if (!anim.GetBool("Shoot"))
			shooting = false;

		float h = Input.GetAxis("Horizontal");

		if (h > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (h < 0 && facingRight)
			// ... flip the player.
			Flip();
	}
	
	protected override void OnLanded()
	{
		gameObject.SendMessage("Landed");
	}


}
