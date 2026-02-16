using UnityEngine;
using System.Collections;

public class CameraFollow2D : MonoBehaviour {

	public float damping = 1.5f;
	public Vector2 offset = new Vector2(2f, 1f);
	public Transform player;
	private int lastX;

	void Start ()
	{
		offset = new Vector2(Mathf.Abs(offset.x), offset.y);
	}

	void Update () 
	{
		if(player)
		{
			int currentX = Mathf.RoundToInt(player.position.x);
			lastX = Mathf.RoundToInt(player.position.x);

			Vector3 target;
				target = new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z);
			Vector3 currentPosition = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
			transform.position = currentPosition;
		}
	}
}
