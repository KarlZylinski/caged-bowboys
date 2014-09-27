using UnityEngine;

public class BulletScript : MonoBehaviour
{
	public float DieTimeMin = 9.0f;
	public float DieTimeMax = 12.0f;
	private Collider2D _trigger;
	private float _die_time;

	public void Start()
	{
		_trigger = GetComponent<Collider2D>();
		_die_time = Time.time + Random.Range(DieTimeMin, DieTimeMax);
	}

	public void OnColliderEnter2D(Collision2D other)
	{
		if (!(other.gameObject.layer >= 9 && other.gameObject.layer <= 16))
		{
			_trigger.isTrigger = false;
			_trigger.gameObject.layer = 17;
		}
	}

	public void Update()
	{
		if (Time.time > _die_time)
		{
			Destroy(gameObject);
		}
	}
}
