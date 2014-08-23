using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {
	protected float max_speed = 3f;
	protected float move_force = 1f;
	protected float jump_force = 150f;
	protected Transform ground_check;			// A position marking where to check if the player is grounded.
	protected bool grounded = false;			// Whether or not the player is grounded.
	protected bool pressed = false;

	protected Animator animator;

	void Awake()
	{
		// Setting up references.
		ground_check = transform.Find("ground_check");

		animator = GetComponent<Animator>();
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		}

	void FixedUpdate() {
		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(rigidbody2D.velocity.x < max_speed)
			// ... add a force to the player.
			rigidbody2D.AddForce(Vector2.right * move_force);
		
		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(rigidbody2D.velocity.x) > max_speed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * max_speed, rigidbody2D.velocity.y);

		Debug.Log(rigidbody2D.velocity.y);
		grounded = Physics2D.Linecast(transform.position, ground_check.position, 1 << LayerMask.NameToLayer("Ground"));

		animator.SetBool("Jump", false);
		animator.SetBool("Switch", false);
		if (Input.GetButtonDown("Jump")) {
			if (grounded) {
				rigidbody2D.AddForce(new Vector2(0f, jump_force));
				animator.SetBool("Jump", true);
			} else 
				animator.SetBool("Switch", true);
		}

		/*if (Input.GetButton("Jump")) {
			if (rigidbody2D.velocity.y >= -0.01f) {
				// Add a vertical force to the player.
	 			if (rigidbody2D.velocity.y < 0.001f)
					rigidbody2D.AddForce(new Vector2(0f, jump_force));
				else
					rigidbody2D.AddForce(new Vector2(0f, rigidbody2D.velocity.y * 0.02f * jump_force));
			}
		} */
	}
}
