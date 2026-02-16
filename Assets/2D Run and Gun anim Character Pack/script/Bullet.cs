using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{



	void Start () 
	{
		// Destroy the rocket after 2 seconds if it doesn't get destroyed before then.
		Destroy(gameObject, 2);
	}



}
