using UnityEngine;
using System.Collections;

public class RunningCharacter : MonoBehaviour
{
	public float maxHealth = 1.0f;
	private float currentHealth;

	[HideInInspector]
	public bool facingRight = true;         // For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;               // Condition for whether the player should jump.
	protected bool falling = false;
	private Vector3 prevFeetPosition;

	public float maxSpeed = 5f;             // The fastest the player can travel in the x axis.
	public AudioClip[] jumpClips;           // Array of clips for when the player jumps.
	public float jumpForce = 1000f;         // Amount of force added when the player jumps.
	
	protected Transform feet;
	protected bool grounded = false;          // Whether or not the player is grounded.
	protected float fallInputCooldown = 0.0f;
	protected Animator anim;                  // Reference to the player's animator component.

	protected bool shoot = false;
	protected bool shootCooldownExpired = true; //don't shoot again if shoot button is pressed during the first part of the shoot animation (from start till projectile is fired)
	protected bool canShoot = true;
	protected bool shooting = false;
	protected bool canMove = true;

	public GameObject daggerPrefab;

	public AnimationCurve velocityCurve;
	public float acceleration = 1.0f;
	public float deceleration = 1.0f;
	protected float velocityCurveTime = 0.0f;

	void Awake()
	{
		// Setting up references.
		feet = transform;
		anim = GetComponent<Animator>();
		prevFeetPosition = feet.position;

		currentHealth = maxHealth;
	}

	protected virtual bool ShouldJump()
	{
		return false;
	}
	protected virtual bool ShouldFire()
	{
		return false;
	}
	protected virtual bool ShouldDropDown()
	{
		return false;
	}
	protected virtual float GetHorizontalInput()
	{
		return 1.0f;
	}
	protected virtual void WillShoot()
	{
		return;
	}
	protected virtual void OnLanded()
	{
		return;
	}

	void Update()
	{
		

		// If the jump button is pressed and the player is grounded then the player should jump.
		if (ShouldJump() && grounded)
			jump = true;

		if (ShouldFire() && shootCooldownExpired)
		{
			shoot = true;
			jump = false;
		}

	}

	public virtual void Shoot()
	{
	}
	
	void FixedUpdate ()
	{
		Vector3 newFeetPosition = feet.position;
		
		bool wasGrounded = grounded;

		Vector2 vel = GetComponent<Rigidbody2D>().velocity;

		RaycastHit2D hit;
		hit = Physics2D.Linecast(prevFeetPosition, newFeetPosition, 1 << LayerMask.NameToLayer("Ground"));

		Vector2 tempPosition = hit.point;

		//hero drop down from grounded
		if(grounded &&
			!anim.GetCurrentAnimatorStateInfo(0).IsName("FallRecovery") &&
			!anim.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
		{
			if(ShouldDropDown() && fallInputCooldown <= 0.0f)
			{
				fallInputCooldown = 0.2f;
				grounded = false;
				anim.SetTrigger("Falling");
				falling = true;
				GetComponent<Rigidbody2D>().gravityScale = 1.0f;
			}
		}

		if ((newFeetPosition - prevFeetPosition).y <= 0 && fallInputCooldown <= 0.0f)
		{

			if (hit.collider != null)
			{
				if(hit.fraction != 0)
				{
					if(hit.normal.Equals(Vector2.up))
						grounded = true;
				}
				else
				{
					RaycastHit2D groundCheckHit = Physics2D.Linecast(hit.point + 0.05f *Vector2.up, hit.point - 0.05f * Vector2.up, 1 << LayerMask.NameToLayer("Ground"));
					if(groundCheckHit.fraction != 0)
					{
						grounded = true;
						tempPosition = groundCheckHit.point;
					}
					else
					{
						grounded = false;
					}
				}
			}
			
			else if(hit.collider == null)
			{
				grounded = false;
				anim.SetTrigger("Falling");
				falling = true;
				GetComponent<Rigidbody2D>().gravityScale = 1.0f;
			}
		}
		else
		{
			grounded = false;
			fallInputCooldown -= Time.fixedDeltaTime;
		}

		//if landed
		if (!wasGrounded && grounded)
		{
			shooting = false;
			shootCooldownExpired = true;

			newFeetPosition = tempPosition;
			transform.position = newFeetPosition;
			GetComponent<Rigidbody2D>().gravityScale = 0;
			vel.y = 0;
			GetComponent<Rigidbody2D>().velocity = vel;

			OnLanded();
		}

		prevFeetPosition = newFeetPosition;

		// Cache the horizontal input.
		float h = GetHorizontalInput();
		//Vector2 vel = GetComponent<Rigidbody2D>().velocity;

		if (shoot && canShoot)
		{
			shoot = false;
			WillShoot();
		}
		
		shooting = anim.GetCurrentAnimatorStateInfo(0).IsName("Shoot") || anim.GetCurrentAnimatorStateInfo(0).IsName("ShootAir");

		if (!shooting)
		{
			if (h > 0 && !facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (h < 0 && facingRight)
				// ... flip the player.
				Flip();
		}

		if (grounded)
		{
			falling = false;

			anim.SetBool("Grounded", true);
			anim.SetBool("Falling", false);

			if (!shooting)
			{
				anim.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));

				//this "if" makes the run animation stop when releasing the run button even if the player has ground velocity.
				//it's done to give the feel that the char slides on ground to stop himself
				if (h * GetComponent<Rigidbody2D>().velocity.x > 0.0f && canMove)
				{
					anim.SetBool("Run", true);
				}
				else
				{
					anim.SetBool("Run", false);
				}
			}
			
			float velSign = Mathf.Sign(vel.x);

			//if already running and input direction have same verse
			if (h * vel.x > 0 && !shooting && canMove)
				velocityCurveTime += acceleration * Time.fixedDeltaTime;
			//restart accelerating from still towards input direction
			else if (vel.x == 0 && h != 0 && !shooting && canMove)
			{
				velocityCurveTime += acceleration * Time.fixedDeltaTime;
				velSign = h;
			}
			else
				velocityCurveTime -= deceleration * Time.fixedDeltaTime;

			velocityCurveTime = Mathf.Clamp(velocityCurveTime, 0, velocityCurve.keys[velocityCurve.length - 1].time);

			vel.x = velSign * velocityCurve.Evaluate(velocityCurveTime) * maxSpeed;

			GetComponent<Rigidbody2D>().velocity = vel;
			
		}
		else
		{
			anim.SetBool("Run", false);
			anim.SetBool("Grounded", false);
		}
		

		if(jump && !shooting && canMove)
		{
			// Set the Jump animator trigger parameter.
			anim.SetTrigger("Jump");
			gameObject.SendMessage("Jumped");

			// Add a vertical force to the player.
			GetComponent<Rigidbody2D>().gravityScale = 1.0f;
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
			falling = false;
			canMove = false;
		}

		if (vel.y < -5 && !falling && !grounded)
		{
			anim.SetTrigger("Falling");
			falling = true;
		}
	}
	

	
	protected void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	//Called by animator event
	public void RecoveredFromFall()
	{
		canMove = true;
		canShoot = true;
	}
	//Called by animator event
	public void StartRecoveryFromFall()
	{
		canMove = false;
		canShoot = false;
	}

	public void ApplyDamage(float damage)
	{
		currentHealth -= damage;

		if (currentHealth <= 0)
		{
			FXGenerator.GroundExplosion(transform.position);
			Destroy(gameObject);
		}
	}
}
