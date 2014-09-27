using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour
{
	public float Speed = -0.3f;
	private bool _has_passed_origo;

	void Start ()
	{
		_has_passed_origo = false;
	}
	
	void Update () {
		transform.position += new Vector3(0, Speed * Time.deltaTime, 0);

		if (transform.position.y < 0 && !_has_passed_origo)
		{
			SpawnCopy();
			_has_passed_origo = true;
		}

		if (transform.position.y < -20.0)
			Destroy(gameObject);
	}

	private void SpawnCopy()
	{
		Instantiate(this, transform.position + new Vector3(0, renderer.bounds.extents.y * 2.0f, 0), Quaternion.identity);
	}
}
