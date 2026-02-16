using UnityEngine;
using System.Collections;

public class PlayerControl01 : MonoBehaviour
{

	float timer;
	public bool facingRight = true;			// For determining which way the player is currently facing.
	public bool jump = false;				// Condition for whether the player should jump.
	public float moveForce = 0f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
	public Animator anim;					// Reference to the player's animator component.
    public float score;
	public Transform Shooter;			// Reference to the HitPoint.
	public GameObject Bullet; 			// Prefab of the Bullet
	public bool canShoot;
	public float shootVelocity;
	public float ShootCDTime = 0.5f;
    private GameObject star;
	float groundRadius = 0.85f;
	public float speed = 20f;				// The speed the rocket will fire at.
	
void OnDrawGizmos()
	{	
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (Shooter.transform.position, 0.3f);
	}
    void Awake()
	{
		// Setting up references.
		anim = GetComponent<Animator>();
        anim.SetBool("ground", false);
		anim.SetBool("shoot Bool", true);
    }
	
	
	public void Shoot()
	{

			if(facingRight)
			{
				// ... instantiate the rocket facing right and set it's velocity to the right. 
				GameObject projectile = (GameObject)Instantiate(Bullet, Shooter.transform.position, Quaternion.Euler(new Vector3(0,0,0)));
				projectile.GetComponent<Rigidbody2D>().linearVelocity = new Vector2 (speed, 0);
				
				//Rigidbody2D bulletInstance = Instantiate(Bullet, Shooter.transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				//bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(shootVelocity , 0);
			}
			else
			{
				// Otherwise instantiate the rocket facing left and set it's velocity to the left.
				//Rigidbody2D bulletInstance = Instantiate(Bullet, Shooter.transform.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				//bulletInstance.velocity = new Vector2(-speed, 0);
				GameObject projectile = (GameObject)Instantiate(Bullet, Shooter.transform.position, Quaternion.Euler(new Vector3(0,0,180f)));
				projectile.GetComponent<Rigidbody2D>().linearVelocity = new Vector2 (-speed, 0);
			}
		
	
	
	
	
		//GameObject projectile = (GameObject)Instantiate(Bullet, Shooter.transform.position, Quaternion.identity);	
		//mousePos.Normalize();		
		//projectile.GetComponent<Rigidbody2D>().velocity = new Vector2 (shootVelocity * Shooter.transform.position.x, shootVelocity * Shooter.transform.position.y + Random.Range (-10.5f, 10.5f));
		//projectile.GetComponent<Rigidbody2D>().velocity = new Vector2 (shootVelocity, shootVelocity + Random.Range (-10.5f, 10.5f));
		canShoot = false;
	
	}

	void Update()
	{
			if (!canShoot) 
		{
			timer -= Time.deltaTime;
		}
		
		if (timer <= 0) 
		{
			canShoot = true;
			timer = ShootCDTime;
		}
		// If the fire button is pressed...
		if (Input.GetButton ("Fire1") && canShoot ) 
		{
			Shoot ();
		}
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

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * GetComponent<Rigidbody2D>().linearVelocity.x < maxSpeed)
			// ... add a force to the player.
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);

		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(GetComponent<Rigidbody2D>().linearVelocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().linearVelocity.x) * maxSpeed, GetComponent<Rigidbody2D>().linearVelocity.y);

		// If the input is moving the player right and the player is facing left...
		if(h > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && facingRight)
			// ... flip the player.
			Flip();

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
	
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
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
