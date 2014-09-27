using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		var music = GameObject.FindGameObjectsWithTag("Music");

		foreach (var m in music)
		{
			if (m != gameObject)
				Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
