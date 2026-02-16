using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
	public Animator anim;					// Reference to the player's animator component.
    public float score;
    private GameObject star;
	float groundRadius = 0.85f;
	

    void Awake()
	{
		// Setting up references.
		anim = GetComponent<Animator>();
        anim.SetBool("ground", false);
		anim.SetBool("shoot Bool", true);
    }

	void Update()
	{
        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, 1 << LayerMask.NameToLayer("Ground"));
        anim.SetBool("ground", grounded);
        // If the jump button is pressed and the player is grounded then the player should jump.
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }
    }

	void FixedUpdate ()
	{
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");
		// The Speed animator parameter is set to the absolute value of the horizontal input.
		anim.SetFloat("Speed", Mathf.Abs(h));
		// Set the value in the animator rise / fall speed
		anim.SetFloat ("vSpeed", GetComponent<Rigidbody2D>().linearVelocity.y);
        if (!grounded)
            return;
		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * GetComponent<Rigidbody2D>().linearVelocity.x < maxSpeed)
			// ... add a force to the player.
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);

		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(GetComponent<Rigidbody2D>().linearVelocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().linearVelocity.x) * maxSpeed, GetComponent<Rigidbody2D>().linearVelocity.y);
		// If the player should jump...
		if(jump)
		{
			// Set the Jump animator trigger parameter.
			anim.SetTrigger("Jump");
			// Add a vertical force to the player.
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
         
        }
	}
	
    void OnTriggerEnter2D(Collider2D col)
    {
        // 
        if ((col.gameObject.name == "dieCollider") || (col.gameObject.name == "saw"))
        {
            // .. stop the camera tracking the player

            anim.SetTrigger("Die");

        }
        // Destroy the star.
        if (col.gameObject.name == "star")
        {
            score++;
            Destroy(col.gameObject);
        }

    }

}
