using UnityEngine;
using System.Collections;

public enum World {Actual, Book};

public class Movement : MonoBehaviour {
	// Movement 
	protected static readonly float max_speed = 4f;
	protected static readonly float move_force = 1f;
	protected static readonly float jump_force = 150f;
	protected static readonly float ascend_force = jump_force * 15f;
	protected static readonly float ascend_decrease_fact = 1.3f;
	protected static readonly float ascend_time = 1f;
	protected Transform ground_check;			// A position marking where to check if the player is grounded.
	protected bool grounded = false;			// Whether hits the ground
	protected bool pressed = false;
	protected float jump_time = 0f; // How long since the start of the jump
	protected bool ascending = false; // Is in the until now uninterrupted jump

	// On-display text
	protected static readonly float MIN_DISPLAY_TIME = 0.5f;
	protected float display_timer = 0f;
	protected TextMesh text;
	
	// Sprite data
	protected Animator animator;	

	// Game mechanics
	protected bool suspended = false;
	protected Vector3 last_position; // Where the player got suspended
	protected Vector2 last_velocity; // Which velocity the player had
	protected World current_world; // In which world the character currently is
	protected float respawn_x = 0f; // Where will the player respawn
	protected bool crashed = false;

	void Awake()
	{
		// Setting up references.
		ground_check = transform.Find("ground_check");
		text = GameObject.Find("display_text").GetComponent<TextMesh>();
		animator = GetComponent<Animator>();
	}

	
	void Start () {
		current_world = World.Actual;
	}

	void resume_game() {
		if (crashed) {
			transform.position = new Vector3(respawn_x, 0, 0);
		} else {
			rigidbody2D.velocity = last_velocity;
		}
		text.text = "";
		Time.timeScale = 1f;

		suspended = crashed = false;
	}

	void Update () {
		if (suspended)  {
			if (display_timer > 0f) {
				display_timer -= 0.01f;
			}
			rigidbody2D.velocity = new Vector2(0f, 0f);
			transform.position = last_position;
			if (Input.GetButton("Jump")) {
				resume_game();
			} 
			return;
		}
	
		grounded = Physics2D.Linecast(transform.position, ground_check.position, 1 << LayerMask.NameToLayer("Ground"));

		animator.SetBool("Switch", false);
		if (Input.GetButtonDown("Jump")) {
			current_world = (World) System.Convert.ToInt32(!System.Convert.ToBoolean(current_world));
			animator.SetBool("Switch", true);
			if (grounded) {
				rigidbody2D.AddForce(new Vector2(0f, jump_force));
				ascending = true;
			}
		} if (ascending && Input.GetButton("Jump")) {
			jump_time += Time.deltaTime;
			float effect = Mathf.Pow(Mathf.Max(ascend_time - jump_time, 0) * Time.deltaTime, ascend_decrease_fact); 
			rigidbody2D.AddForce(new Vector2(0f, ascend_force * effect));
		}
		else if (Input.GetButtonUp("Jump")) {
			ascending = false;
			jump_time = 0f;
		}
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


	}

	void display(string new_text) {
		text.text = new_text;
		last_position = transform.position;
		display_timer = MIN_DISPLAY_TIME;
		suspended = true;
		last_velocity = rigidbody2D.velocity;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if ((other.gameObject.tag == "GroundA" && current_world == World.Book) || (other.gameObject.tag == "GroundB" && current_world == World.Actual)) {
			display ("crashed");
			crashed = true;
		}
		if (other.gameObject.tag == "Milestone") {
			display(other.GetComponent<SignText>().caption);
			other.GetComponent<BoxCollider2D>().enabled = false;
			respawn_x = other.transform.position.x;
		}
	}

}
