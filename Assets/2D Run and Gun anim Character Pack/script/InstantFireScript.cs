using UnityEngine;
using System.Collections;


public class InstantFireScript : MonoBehaviour {
	
	float timer;
	public PlayerControl player;     // Reference to the PlayerControl script.
	public Transform Shooter;			// Reference to the HitPoint.
	public GameObject Bullet; 			// Prefab of the Bullet
	public bool facingRight = true;
	public bool canShoot;
	public float shootVelocity;
	public float ShootCDTime = 0.5f;
	public bool HoldToFire = false;
	private Vector3 theScale;
	private Vector3 pos;

	void OnDrawGizmos()
	{	
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (Shooter.transform.position, 0.3f);
	}
	
	public void Shoot()
	{

		GameObject projectile = (GameObject)Instantiate(Bullet, Shooter.transform.position, Quaternion.identity);		
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 0;		
		Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
		mousePos.x = mousePos.x - objectPos.x;
		mousePos.y = mousePos.y - objectPos.y;
		mousePos.Normalize();
		projectile.GetComponent<Rigidbody2D>().linearVelocity = new Vector2 (shootVelocity * mousePos.x, shootVelocity * mousePos.y + Random.Range (-10.5f, 10.5f));
		canShoot = false;
	
	}

	void Update () 
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 0;
		
		Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
		mousePos.x = mousePos.x - objectPos.x;
		mousePos.y = mousePos.y - objectPos.y;
		mousePos.Normalize();

		float angle = Mathf.Atan2 (mousePos.y, mousePos.x) * Mathf.Rad2Deg;

		shootVelocity = 100;
		ShootCDTime = 0.07f;

		if(angle <= 90f & angle >= -90f ) 
		{
			player.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			this.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
		} 
		else 
		{ 
			player.gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
			this.gameObject.transform.localScale = new Vector3(-1f, -1f, 1f);
			transform.rotation = Quaternion.Euler (new Vector3 (0, 0, -angle));
		}
		
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
	}

}