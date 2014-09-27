using UnityEngine;

public class BulletScript : MonoBehaviour {
	private Collider2D _trigger;

	public void Start()
	{
		_trigger = GetComponent<Collider2D>();
	}

	public void OnColliderEnter2D(Collision2D other)
	{
		if (!(other.gameObject.layer >= 9 && other.gameObject.layer <= 16))
		{
			_trigger.isTrigger = false;
			_trigger.gameObject.layer = 17;
		}
	}
}
