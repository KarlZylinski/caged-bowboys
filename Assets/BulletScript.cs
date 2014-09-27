using UnityEngine;

public class BulletScript : MonoBehaviour
{
	public float SlowdownDistance = 0.5f;
	public GameObject HitThing;

	public void OnCollisionEnter2D(Collision2D other)
	{
		var ht = (GameObject) Instantiate(HitThing);
		ht.transform.position = transform.position;
		Destroy(gameObject);
	}/*

	public void Update()
	{
		var tracked = FindObjectsOfType<TrackedByCamera>();

		var nearest = float.MaxValue;

		for (var i = 1; i < tracked.Length; ++i)
		{
			var t = tracked[i];
			var v = t.transform.position - transform.position;
			var d = v.magnitude;
			
			if (d < nearest)
				nearest = d;
		}

		print(nearest);

		var slofdown = nearest <= SlowdownDistance
			? Mathf.Clamp(nearest / SlowdownDistance, 0.3f, 1.0f)
			: 1.0f;

		print(slofdown);
		Time.timeScale = slofdown;
	}*/
}
