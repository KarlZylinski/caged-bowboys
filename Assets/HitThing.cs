using UnityEngine;
using System.Collections;

public class HitThing : MonoBehaviour {
	public Sprite SecondSprite;
	private float _start_time;
	private bool _second_set;
	

	public void Awake ()
	{
		_second_set = false;
		_start_time = Time.time;
	}
	
	public void Update () {
		if (Time.time > _start_time + 0.2f && !_second_set)
		{
			GetComponent<SpriteRenderer>().sprite = SecondSprite;
			_second_set = true;
		}
		
		if (Time.time > _start_time + 0.4f)
			Destroy(gameObject);
	}
}
