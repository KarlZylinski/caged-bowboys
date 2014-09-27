using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour
{
	private float _end_time;

	// Use this for initialization
	void Start ()
	{
		//_end_time = Time.time + audio.clip.length + 2.0f;
		_end_time = Time.time + 60.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > _end_time)
		{
			Application.LoadLevel(3);
		}

		if (Input.GetKeyDown(KeyCode.Escape))
			Application.LoadLevel(0);
	}
}
