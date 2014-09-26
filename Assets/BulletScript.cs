using UnityEngine;

public class BulletScript : MonoBehaviour {
	private Collider2D _trigger;

	public void Start()
	{
		_trigger = GetComponent<Collider2D>();
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		_trigger.isTrigger = false;
		_trigger.gameObject.layer = 17;
	}
}
