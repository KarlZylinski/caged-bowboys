using UnityEngine;

public class BulletScript : MonoBehaviour
{
	public GameObject HitThing;

	public void OnCollisionEnter2D(Collision2D other)
	{
		var ht = (GameObject) Instantiate(HitThing);
		ht.transform.position = transform.position;
		Destroy(gameObject);
	}
}
