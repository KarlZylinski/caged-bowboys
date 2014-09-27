using UnityEngine;
using System.Collections;

public class BloodLayerSetter : MonoBehaviour
{
	private float _start_time;
	// Use this for initialization
	void Start ()
	{
		var scale = Random.Range(1.0f, 1.9f);
		transform.localScale = new Vector3(scale, scale, scale);
		_start_time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > _start_time + 0.1f)
		{
			gameObject.layer = 22;
		}

		if (Time.time > _start_time + Random.Range(2.0f, 3.5f))
		{
			Destroy(gameObject);
		}
	}
}
