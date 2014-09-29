using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour
{
	private float _end_time;
	private TextMesh _time_left_text;

	// Use this for initialization
	void Start ()
	{
		//_end_time = Time.time + audio.clip.length + 2.0f;
		_end_time = Time.time + 60.0f;
		_time_left_text = GameObject.Find("TimeLeftText").GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > _end_time)
		{
			Application.LoadLevel(3);
		}

		var time_left = Time.time - _end_time;
		_time_left_text.text = (Mathf.Abs((int)time_left)).ToString();

		if (Input.GetKeyDown(KeyCode.Escape))
			Application.LoadLevel(0);
	}
}
