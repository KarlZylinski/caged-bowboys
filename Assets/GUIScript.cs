using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		var cp = Camera.main.transform.position;
		transform.position = new Vector3(cp.x, cp.y, 0);
	}
}
