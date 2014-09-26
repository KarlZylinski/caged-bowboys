using UnityEngine;
using System.Collections;

public class HeadScript : MonoBehaviour {
	private Collider2D _trigger;
	
	void Start ()
	{
		_trigger = GetComponent<Collider2D>();
		gameObject.layer = 18 + int.Parse(transform.parent.GetComponent<PlayerControl>().PlayerNum) - 1;
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Bullet")
		{
			Destroy(transform.parent.gameObject);
		}
	}
}
